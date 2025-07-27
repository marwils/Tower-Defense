using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

namespace LevelSystem
{
    [CustomPropertyDrawer(typeof(SequenceSpawner))]
    public class SequenceSpawnerPropertyDrawer : PropertyDrawer
    {
        private string[] _sequenceTypeNames;
        private Type[] _sequenceTypes;

        public SequenceSpawnerPropertyDrawer()
        {
            _sequenceTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(asm => asm.GetTypes())
                .Where(t => t.IsSubclassOf(typeof(AbstractSequenceElement)) && !t.IsAbstract)
                .ToArray();
            _sequenceTypeNames = _sequenceTypes.Select(t => t.Name).ToArray();
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var sequenceSpawner = property.objectReferenceValue as SequenceSpawner;
            if (sequenceSpawner == null)
            {
                EditorGUI.PropertyField(position, property, label);
                EditorGUI.EndProperty();
                return;
            }

            // Create foldout
            var foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;

                var sequenceSO = new SerializedObject(sequenceSpawner);
                sequenceSO.Update();

                var currentY = position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                // Draw properties
                var enemyPrefabsProp = sequenceSO.FindProperty("_enemyPrefabs");
                var shuffleSpawnOrderProp = sequenceSO.FindProperty("_shuffleSpawnOrder");
                var spawnAmountProp = sequenceSO.FindProperty("_spawnAmount");
                var intervalProp = sequenceSO.FindProperty("_interval");

                // Enemy Prefabs - Custom List Implementation
                if (enemyPrefabsProp != null)
                {
                    // Header mit Size Control
                    var headerRect = new Rect(position.x, currentY, position.width * 0.7f, EditorGUIUtility.singleLineHeight);
                    var sizeRect = new Rect(position.x + position.width * 0.7f, currentY, position.width * 0.3f, EditorGUIUtility.singleLineHeight);
                    var size = enemyPrefabsProp.arraySize == 0 ? "must not be empty" : enemyPrefabsProp.arraySize.ToString();

                    EditorGUI.LabelField(headerRect, $"Enemy Prefabs ({size})", EditorStyles.boldLabel);

                    currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                    // Draw array elements
                    EditorGUI.indentLevel++;
                    for (int i = 0; i < enemyPrefabsProp.arraySize; i++)
                    {
                        var elementProp = enemyPrefabsProp.GetArrayElementAtIndex(i);
                        var elementRect = new Rect(position.x, currentY, position.width - 25, EditorGUIUtility.singleLineHeight);
                        var deleteRect = new Rect(position.x + position.width - 20, currentY, 20, EditorGUIUtility.singleLineHeight);

                        // Zeichne nur das Object Field ohne Label - verwende GUIContent.none
                        EditorGUI.PropertyField(elementRect, elementProp, GUIContent.none);

                        if (GUI.Button(deleteRect, "-"))
                        {
                            enemyPrefabsProp.DeleteArrayElementAtIndex(i);
                            break;
                        }

                        currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    }
                    EditorGUI.indentLevel--;

                    // Add button
                    var addRect = new Rect(position.x, currentY, 100, EditorGUIUtility.singleLineHeight);
                    if (GUI.Button(addRect, "Add Element"))
                    {
                        enemyPrefabsProp.arraySize++;
                    }
                    currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }

                // Shuffle Spawn Order
                if (shuffleSpawnOrderProp != null)
                {
                    var shuffleRect = new Rect(position.x, currentY, position.width, EditorGUIUtility.singleLineHeight);
                    EditorGUI.PropertyField(shuffleRect, shuffleSpawnOrderProp, new GUIContent("Shuffle Spawn Order"));
                    currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }

                // Spawn Amount
                if (spawnAmountProp != null)
                {
                    var spawnAmountRect = new Rect(position.x, currentY, position.width, EditorGUIUtility.singleLineHeight);
                    EditorGUI.PropertyField(spawnAmountRect, spawnAmountProp, new GUIContent("Spawn Amount"));
                    currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }

                // Interval
                if (intervalProp != null)
                {
                    var intervalRect = new Rect(position.x, currentY, position.width, EditorGUIUtility.singleLineHeight);
                    EditorGUI.PropertyField(intervalRect, intervalProp, new GUIContent("Interval in seconds"));
                }

                if (sequenceSO.hasModifiedProperties)
                {
                    sequenceSO.ApplyModifiedProperties();
                    EditorUtility.SetDirty(sequenceSpawner);
                }

                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!property.isExpanded || property.objectReferenceValue == null)
            {
                return EditorGUIUtility.singleLineHeight;
            }

            var sequenceSpawner = property.objectReferenceValue as SequenceSpawner;
            if (sequenceSpawner == null) return EditorGUIUtility.singleLineHeight;

            var sequenceSO = new SerializedObject(sequenceSpawner);
            var enemyPrefabsProp = sequenceSO.FindProperty("_enemyPrefabs");

            float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Foldout + spacing

            if (enemyPrefabsProp != null)
            {
                // Header mit Size
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                // Array elements
                height += enemyPrefabsProp.arraySize * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);

                // Add button
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }

            height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Shuffle Spawn Order
            height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Spawn Amount
            height += EditorGUIUtility.singleLineHeight; // Interval (ohne extra spacing am Ende)

            return height;
        }

        private string SplitCamelCase(string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, "(\\B[A-Z])", " $1");
        }
    }
}