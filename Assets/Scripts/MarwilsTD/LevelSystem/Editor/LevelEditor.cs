using UnityEditor;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace MarwilsTD.LevelSystem
{
    [CustomEditor(typeof(Level))]
    public class LevelEditor : Editor
    {
        private SerializedProperty _titleProperty;
        private SerializedProperty _routesProperty;
        private SerializedProperty _wavesProperty;
        private RoutesPropertyDrawer _routesPropertyDrawer = new RoutesPropertyDrawer();
        private WavesPropertyDrawer _wavesPropertyDrawer = new WavesPropertyDrawer();

        private bool _showRoutes = true;
        private bool _showWaves = true;

        private void OnEnable()
        {
            _titleProperty = serializedObject.FindProperty("_title");
            _routesProperty = serializedObject.FindProperty("_routes");
            _wavesProperty = serializedObject.FindProperty("_waves");
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

            EditorGUILayout.Space();
            var headerStyle = new GUIStyle(EditorStyles.boldLabel);
            headerStyle.fontSize = 16;
            EditorGUILayout.LabelField("Level Configuration", headerStyle);
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_titleProperty);
            EditorGUILayout.Space();

            if (_routesProperty != null)
            {
                var foldoutStyle = new GUIStyle(EditorStyles.foldout);
                foldoutStyle.fontSize = 14;
                foldoutStyle.fontStyle = FontStyle.Bold;

                _showRoutes = EditorGUILayout.Foldout(_showRoutes, "Routes", true, foldoutStyle);
                if (_showRoutes)
                {
                    EditorGUILayout.Space();
                    var height = _routesPropertyDrawer.GetPropertyHeight(_routesProperty, new GUIContent($"Routes"));
                    var rect = GUILayoutUtility.GetRect(0, height);
                    _routesPropertyDrawer.OnGUI(rect, _routesProperty, new GUIContent($"Routes"));
                }
            }

            EditorGUILayout.Space();

            if (_wavesProperty != null)
            {
                var foldoutStyle = new GUIStyle(EditorStyles.foldout);
                foldoutStyle.fontSize = 14;
                foldoutStyle.fontStyle = FontStyle.Bold;

                _showWaves = EditorGUILayout.Foldout(_showWaves, "Waves", true, foldoutStyle);
                if (_showWaves)
                {
                    EditorGUILayout.Space();
                    var height = _wavesPropertyDrawer.GetPropertyHeight(_wavesProperty, new GUIContent("Waves"));
                    var rect = GUILayoutUtility.GetRect(0, height);
                    _wavesPropertyDrawer.OnGUI(rect, _wavesProperty, new GUIContent("Waves"));
                }
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

        public override bool HasPreviewGUI()
        {
            var level = target as Level;
            return !level.IsSerializationBlocked && base.HasPreviewGUI();
        }
    }

    static class Constants
    {
        public const float MarginVertical = 8f;
        public const float MarginHorizontal = 10f;
    }
}