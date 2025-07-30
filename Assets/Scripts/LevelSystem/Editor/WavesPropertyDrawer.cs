using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

namespace LevelSystem
{
    [CustomPropertyDrawer(typeof(List<Wave>))]
    public class WavesPropertyDrawer : PropertyDrawer
    {
        private WavePropertyDrawer _waveDrawer = new WavePropertyDrawer();

        const float MarginVertical = 8f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var currentY = position.y;

            var headerRect = new Rect(position.x, currentY, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(headerRect, "Waves", EditorStyles.boldLabel);
            currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            EditorGUI.indentLevel++;

            for (int i = 0; i < property.arraySize; i++)
            {
                var waveProperty = property.GetArrayElementAtIndex(i);

                var height = _waveDrawer.GetPropertyHeight(waveProperty, new GUIContent($"Wave {i + 1}"));
                var totalHeight = height + MarginVertical;

                var boxRect = new Rect(position.x, currentY, position.width, totalHeight);
                GUI.Box(boxRect, "", EditorStyles.helpBox);


                var headerWaveRect = new Rect(position.x, currentY + MarginVertical, position.width - 25, EditorGUIUtility.singleLineHeight);
                var headerStyle = new GUIStyle(EditorStyles.boldLabel);
                headerStyle.fontSize = 14;
                EditorGUI.LabelField(headerWaveRect, $"Wave {i + 1}", headerStyle);

                var deleteMinusButtonRect = new Rect(position.x + position.width - 25, currentY + MarginVertical, 20, EditorGUIUtility.singleLineHeight);
                if (GUI.Button(deleteMinusButtonRect, "-"))
                {
                    var wave = waveProperty.objectReferenceValue as Wave;
                    if (wave != null)
                    {
                        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(wave));
                    }
                    property.DeleteArrayElementAtIndex(i);
                    property.serializedObject.ApplyModifiedProperties();
                    break;
                }

                var addPlusButtonRect = new Rect(position.x + position.width - 25 * 2, currentY + MarginVertical, 20, EditorGUIUtility.singleLineHeight);
                DrawAddButton(property, addPlusButtonRect, "+");

                var contentY = currentY + MarginVertical * 2 + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                var contentHeight = height - EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing;
                var contentRect = new Rect(position.x, contentY, position.width, contentHeight);

                DrawWaveContent(contentRect, waveProperty);

                currentY += totalHeight + EditorGUIUtility.standardVerticalSpacing * 2;
            }

            var addButtonRect = new Rect(position.x, currentY, position.width, 30);
            DrawAddButton(property, addButtonRect, "Add Wave");

            EditorGUI.indentLevel--;

            EditorGUI.EndProperty();
        }

        private static void DrawAddButton(SerializedProperty property, Rect addRect, string label)
        {
            if (GUI.Button(addRect, label))
            {
                var level = property.serializedObject.targetObject as Level;
                var wave = LevelAssetFactory.CreateWave(level);
                property.arraySize++;
                var newWaveProp = property.GetArrayElementAtIndex(property.arraySize - 1);
                newWaveProp.objectReferenceValue = wave;

                level.UpdateSceneContext();

                property.serializedObject.ApplyModifiedProperties();
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        private void DrawWaveContent(Rect position, SerializedProperty waveProperty)
        {
            var wave = waveProperty.objectReferenceValue as Wave;
            if (wave == null) return;

            _waveDrawer.DrawWaveContent(position, wave);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            for (int i = 0; i < property.arraySize; i++)
            {
                var waveProperty = property.GetArrayElementAtIndex(i);
                var waveHeight = _waveDrawer.GetPropertyHeight(waveProperty, new GUIContent($"Wave {i + 1}"));
                height += waveHeight + MarginVertical * 2 + EditorGUIUtility.standardVerticalSpacing;
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }

            height += EditorGUIUtility.standardVerticalSpacing;

            return height;
        }
    }
}