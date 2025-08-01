using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

namespace LevelSystem
{
    [CustomPropertyDrawer(typeof(List<Wave>))]
    public class WavesPropertyDrawer : PropertyDrawer
    {
        private WavePropertyDrawer _waveDrawer = new WavePropertyDrawer();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var currentY = position.y;

            EditorGUI.indentLevel++;

            for (int i = 0; i < property.arraySize; i++)
            {
                var waveProperty = property.GetArrayElementAtIndex(i);

                var wave = waveProperty.objectReferenceValue as Wave;
                if (wave == null)
                {
                    Debug.LogWarning($"Wave at index {i} is null. Skipping rendering.");
                    continue;
                }

                var headerTitle = $"Wave {i + 1}";
                if (!string.IsNullOrEmpty(wave.Title) && wave.Title != headerTitle)
                {
                    headerTitle += $" - {wave.Title}";
                }

                var height = _waveDrawer.GetPropertyHeight(waveProperty, new GUIContent(headerTitle));
                var totalHeight = height + Constants.MarginVertical * 2 + Constants.MarginVertical;

                var boxRect = new Rect(position.x, currentY, position.width, totalHeight);
                GUI.Box(boxRect, "", EditorStyles.helpBox);


                var headerWaveRect = new Rect(position.x, currentY + Constants.MarginVertical, position.width - 20 - Constants.MarginVertical, EditorGUIUtility.singleLineHeight);
                var headerStyle = new GUIStyle(EditorStyles.boldLabel);
                headerStyle.fontSize = 14;
                EditorGUI.LabelField(headerWaveRect, headerTitle, headerStyle);

                var deleteMinusButtonRect = new Rect(position.x + position.width - 20 - Constants.MarginVertical, currentY + Constants.MarginVertical, 20, EditorGUIUtility.singleLineHeight);
                if (GUI.Button(deleteMinusButtonRect, "-"))
                {
                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(wave));
                    property.DeleteArrayElementAtIndex(i);
                    property.serializedObject.ApplyModifiedProperties();
                    break;
                }

                var addPlusButtonRect = new Rect(position.x + position.width - 25 * 2, currentY + Constants.MarginVertical, 20, EditorGUIUtility.singleLineHeight);
                DrawAddButton(property, addPlusButtonRect, "+");

                var contentY = currentY + Constants.MarginVertical * 2 + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                var contentHeight = height - EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing;
                var contentRect = new Rect(position.x + Constants.MarginHorizontal, contentY, position.width - Constants.MarginHorizontal * 2, contentHeight);

                DrawWaveContent(contentRect, waveProperty);

                currentY += totalHeight + Constants.MarginVertical;
            }

            var addButtonRect = new Rect(position.x, currentY, position.width, EditorGUIUtility.singleLineHeight);
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
                wave.Title = $"Wave {property.arraySize + 1}";
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
            float height = 0;

            for (int i = 0; i < property.arraySize; i++)
            {
                var waveProperty = property.GetArrayElementAtIndex(i);
                var waveHeight = _waveDrawer.GetPropertyHeight(waveProperty, new GUIContent($"Wave"));
                height += waveHeight + Constants.MarginVertical * 4; // Wave elements, header, and spacing
            }

            height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // "Add Wave"-Button

            return height;
        }
    }
}