using UnityEngine;
using UnityEditor;

namespace LevelSystem
{
    [CustomPropertyDrawer(typeof(SequenceSpawner))]
    public class SequenceSpawnerPropertyDrawer : PropertyDrawer
    {
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

            var foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;

                var sequenceSO = new SerializedObject(sequenceSpawner);
                sequenceSO.Update();

                var currentY = position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                var enemiesProp = sequenceSO.FindProperty("_enemies");
                var shuffleSpawnOrderProp = sequenceSO.FindProperty("_shuffleSpawnOrder");
                var spawnAmountProp = sequenceSO.FindProperty("_spawnAmount");
                var intervalProp = sequenceSO.FindProperty("_interval");

                if (enemiesProp != null)
                {
                    var headerRect = new Rect(position.x, currentY, position.width * 0.7f, EditorGUIUtility.singleLineHeight);
                    var size = enemiesProp.arraySize == 0 ? "must not be empty" : enemiesProp.arraySize.ToString();

                    EditorGUI.LabelField(headerRect, $"Enemies ({size})", EditorStyles.boldLabel);

                    currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                    EditorGUI.indentLevel++;
                    for (int i = 0; i < enemiesProp.arraySize; i++)
                    {
                        var elementProp = enemiesProp.GetArrayElementAtIndex(i);
                        var elementRect = new Rect(position.x, currentY, position.width - 25, EditorGUIUtility.singleLineHeight);
                        var deleteRect = new Rect(position.x + position.width - 20, currentY, 20, EditorGUIUtility.singleLineHeight);

                        EditorGUI.PropertyField(elementRect, elementProp, GUIContent.none);

                        if (GUI.Button(deleteRect, "-"))
                        {
                            enemiesProp.DeleteArrayElementAtIndex(i);
                            break;
                        }

                        currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    }
                    EditorGUI.indentLevel--;

                    var addRect = new Rect(position.x, currentY, 100, EditorGUIUtility.singleLineHeight);
                    if (GUI.Button(addRect, "Add Enemy"))
                    {
                        enemiesProp.arraySize++;
                    }
                    currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }

                if (shuffleSpawnOrderProp != null)
                {
                    var shuffleRect = new Rect(position.x, currentY, position.width, EditorGUIUtility.singleLineHeight);
                    EditorGUI.PropertyField(shuffleRect, shuffleSpawnOrderProp, new GUIContent("Shuffle Spawn Order"));
                    currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }

                if (spawnAmountProp != null)
                {
                    var spawnAmountRect = new Rect(position.x, currentY, position.width, EditorGUIUtility.singleLineHeight);
                    EditorGUI.PropertyField(spawnAmountRect, spawnAmountProp, new GUIContent("Spawn Amount"));
                    currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }

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

            if (property.objectReferenceValue is not SequenceSpawner sequenceSpawner) return EditorGUIUtility.singleLineHeight;

            var sequenceSO = new SerializedObject(sequenceSpawner);
            var enemiesProp = sequenceSO.FindProperty("_enemies");

            float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            if (enemiesProp != null)
            {
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                height += enemiesProp.arraySize * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }

            height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            height += EditorGUIUtility.singleLineHeight;

            return height;
        }
    }
}