using UnityEditor;
using UnityEngine;
using System.Linq;

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
            serializedObject.Update();

            // Title
            EditorGUILayout.PropertyField(_titleProperty);

            // Waves Header
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Waves", EditorStyles.boldLabel);

            if (_wavesProperty != null)
            {
                EditorGUI.indentLevel++;

                // Draw existing waves using PropertyDrawer
                for (int i = 0; i < _wavesProperty.arraySize; i++)
                {
                    var waveProperty = _wavesProperty.GetArrayElementAtIndex(i);

                    // Berechne Höhe und Rect für den PropertyDrawer + Box
                    var height = _waveDrawer.GetPropertyHeight(waveProperty, new GUIContent($"Wave {i + 1}"));
                    const float marginVertical = 8f;
                    var totalHeight = height + marginVertical * 2; // Box-Margins
                    var rect = GUILayoutUtility.GetRect(0, totalHeight);

                    // Draw background box
                    var boxRect = new Rect(rect.x, rect.y, rect.width, totalHeight);
                    GUI.Box(boxRect, "", EditorStyles.helpBox);

                    // Wave header mit Minus-Button
                    var headerRect = new Rect(rect.x, rect.y + marginVertical, rect.width, EditorGUIUtility.singleLineHeight);
                    var deleteRect = new Rect(rect.x + rect.width - 20, rect.y + marginVertical, 20, EditorGUIUtility.singleLineHeight);

                    var headerStyle = new GUIStyle(EditorStyles.boldLabel);
                    headerStyle.fontSize = 14;
                    EditorGUI.LabelField(headerRect, $"Wave {i + 1}", headerStyle);

                    // Delete Button in der Header-Zeile
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

                    // Content area für PropertyDrawer (unter dem Header)
                    var contentY = rect.y + marginVertical * 2 + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    var contentHeight = height - EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing;
                    float marginLeft = 0;
                    var contentRect = new Rect(rect.x + marginLeft, contentY, rect.width - marginLeft * 2, contentHeight);

                    // Nutze WavePropertyDrawer für den Wave-Inhalt
                    DrawWaveContentUsingPropertyDrawer(contentRect, waveProperty);
                }

                // Add Wave Button
                EditorGUILayout.Space();
                if (GUILayout.Button("Add Wave", GUILayout.Height(30)))
                {
                    var wave = LevelAssetFactory.CreateWave(target);
                    _wavesProperty.arraySize++;
                    var newWaveProp = _wavesProperty.GetArrayElementAtIndex(_wavesProperty.arraySize - 1);
                    newWaveProp.objectReferenceValue = wave;
                    serializedObject.ApplyModifiedProperties();
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }

                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawWaveContentUsingPropertyDrawer(Rect position, SerializedProperty waveProperty)
        {
            var wave = waveProperty.objectReferenceValue as Wave;
            if (wave == null) return;

            _waveDrawer.DrawWaveContent(position, waveProperty, wave);
        }
    }
}