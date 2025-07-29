using UnityEditor;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelSystem
{
    [CustomEditor(typeof(Level))]
    public class LevelEditor : Editor
    {
        private SerializedProperty _titleProperty;
        private SerializedProperty _wavesProperty;
        private WavePropertyDrawer _waveDrawer;

        private void OnEnable()
        {
            _titleProperty = serializedObject.FindProperty("_title");
            _wavesProperty = serializedObject.FindProperty("_waves");
            _waveDrawer = new WavePropertyDrawer();
        }

        public override void OnInspectorGUI()
        {
            var level = target as Level;

            // Block serialization if scene context is invalid
            if (level.IsSerializationBlocked && level.IsInDifferentScene())
            {
                DrawBlockedSerializationWarning(level);
                return;
            }

            serializedObject.Update();

            EditorGUILayout.PropertyField(_titleProperty);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Waves", EditorStyles.boldLabel);

            if (_wavesProperty != null)
            {
                EditorGUI.indentLevel++;

                for (int i = 0; i < _wavesProperty.arraySize; i++)
                {
                    var waveProperty = _wavesProperty.GetArrayElementAtIndex(i);

                    var height = _waveDrawer.GetPropertyHeight(waveProperty, new GUIContent($"Wave {i + 1}"));
                    const float marginVertical = 8f;
                    var totalHeight = height + marginVertical * 2;
                    var rect = GUILayoutUtility.GetRect(0, totalHeight);

                    var boxRect = new Rect(rect.x, rect.y, rect.width, totalHeight);
                    GUI.Box(boxRect, "", EditorStyles.helpBox);

                    var headerRect = new Rect(rect.x, rect.y + marginVertical, rect.width, EditorGUIUtility.singleLineHeight);
                    var deleteRect = new Rect(rect.x + rect.width - 20, rect.y + marginVertical, 20, EditorGUIUtility.singleLineHeight);

                    var headerStyle = new GUIStyle(EditorStyles.boldLabel);
                    headerStyle.fontSize = 14;
                    EditorGUI.LabelField(headerRect, $"Wave {i + 1}", headerStyle);

                    if (GUI.Button(deleteRect, "-"))
                    {
                        var wave = waveProperty.objectReferenceValue as Wave;
                        if (wave != null)
                        {
                            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(wave));
                        }
                        _wavesProperty.DeleteArrayElementAtIndex(i);
                        serializedObject.ApplyModifiedProperties();
                        break;
                    }

                    var contentY = rect.y + marginVertical * 2 + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    var contentHeight = height - EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing;
                    float marginLeft = 0;
                    var contentRect = new Rect(rect.x + marginLeft, contentY, rect.width - marginLeft * 2, contentHeight);

                    DrawWaveContentUsingPropertyDrawer(contentRect, waveProperty);
                }

                EditorGUILayout.Space();
                if (GUILayout.Button("Add Wave", GUILayout.Height(30)))
                {
                    var wave = LevelAssetFactory.CreateWave(target);
                    _wavesProperty.arraySize++;
                    var newWaveProp = _wavesProperty.GetArrayElementAtIndex(_wavesProperty.arraySize - 1);
                    newWaveProp.objectReferenceValue = wave;

                    level.UpdateSceneContext();

                    serializedObject.ApplyModifiedProperties();
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }

                EditorGUI.indentLevel--;
            }

            if (GUI.changed && !level.IsSerializationBlocked)
            {
                level.UpdateSceneContext();
            }

            if (!level.IsSerializationBlocked)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void DrawBlockedSerializationWarning(Level level)
        {
            var currentScene = SceneManager.GetActiveScene().name;

            EditorGUILayout.Space();

            var oldColor = GUI.backgroundColor;
            GUI.backgroundColor = new Color(0.8f, 0.1f, 0.1f, 0.9f);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            var headerStyle = new GUIStyle(EditorStyles.boldLabel);
            headerStyle.fontSize = 16;
            headerStyle.normal.textColor = Color.white;

            EditorGUILayout.LabelField("🚫 SERIALIZATION BLOCKED", headerStyle);

            var warningStyle = new GUIStyle(EditorStyles.label);
            warningStyle.wordWrap = true;
            warningStyle.fontSize = 12;
            warningStyle.normal.textColor = Color.white;

            EditorGUILayout.LabelField(
                $"This Level cannot be modified or saved because it was created in scene " +
                $"'{level.LastModifiedBySceneName}' but you are currently in scene '{currentScene}'.\n\n" +
                $"Scene-specific references (Spawn/Target Points) would be invalid. " +
                $"Please choose an action below to continue:",
                warningStyle
            );

            EditorGUILayout.Space(10);

            EditorGUILayout.BeginVertical();

            var oldBgColor = GUI.backgroundColor;
            GUI.backgroundColor = new Color(0.2f, 0.8f, 0.2f, 1f);
            if (GUILayout.Button("UPDATE TO CURRENT SCENE", GUILayout.Height(35)))
            {
                level.UpdateSceneContext();
                EditorUtility.SetDirty(level);
                AssetDatabase.SaveAssets();
                Debug.Log($"Level '{level.name}' updated to scene '{currentScene}'");
            }
            GUI.backgroundColor = oldBgColor;

            EditorGUILayout.Space(5);

            GUI.backgroundColor = new Color(0.3f, 0.6f, 0.9f, 1f);
            if (GUILayout.Button($"SWITCH TO SCENE '{level.LastModifiedBySceneName}'", GUILayout.Height(30)))
            {
                if (EditorApplication.isPlaying)
                {
                    EditorUtility.DisplayDialog("Cannot Switch Scene",
                        "Cannot switch scenes while in Play Mode.", "OK");
                }
                else
                {
                    var scenePath = GetScenePath(level.LastModifiedBySceneName);
                    if (!string.IsNullOrEmpty(scenePath))
                    {
                        if (UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                        {
                            UnityEditor.SceneManagement.EditorSceneManager.OpenScene(scenePath);
                        }
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Scene Not Found",
                            $"Could not find scene '{level.LastModifiedBySceneName}' in build settings.", "OK");
                    }
                }
            }
            GUI.backgroundColor = oldBgColor;

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();

            GUI.backgroundColor = oldColor;

            EditorGUILayout.Space();

            GUI.enabled = false;
            EditorGUILayout.LabelField("All other controls are disabled until scene context is resolved.", EditorStyles.helpBox);
            GUI.enabled = true;
        }

        private string GetScenePath(string sceneName)
        {
            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                var scene = EditorBuildSettings.scenes[i];
                if (scene.path.Contains(sceneName))
                {
                    return scene.path;
                }
            }
            return null;
        }

        private void DrawWaveContentUsingPropertyDrawer(Rect position, SerializedProperty waveProperty)
        {
            var wave = waveProperty.objectReferenceValue as Wave;
            if (wave == null) return;

            _waveDrawer.DrawWaveContent(position, waveProperty, wave);
        }

        public override bool HasPreviewGUI()
        {
            var level = target as Level;
            return !level.IsSerializationBlocked && base.HasPreviewGUI();
        }
    }
}