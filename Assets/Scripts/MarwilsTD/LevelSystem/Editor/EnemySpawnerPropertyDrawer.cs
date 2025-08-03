using UnityEngine;
using UnityEditor;

namespace MarwilsTD.LevelSystem
{
    [CustomPropertyDrawer(typeof(EnemySpawnerConfiguration))]
    public class EnemySpawnerPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var spawner = property.objectReferenceValue as EnemySpawnerConfiguration;
            if (spawner == null)
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

                var spawnerSO = new SerializedObject(spawner);
                spawnerSO.Update();

                float currentY = position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                var enemiesProp = spawnerSO.FindProperty("_enemies");
                var shuffleSpawnOrderProp = spawnerSO.FindProperty("_shuffleSpawnOrder");
                var spawnAmountProp = spawnerSO.FindProperty("_spawnAmount");
                var intervalProp = spawnerSO.FindProperty("_interval");

                if (enemiesProp != null)
                {
                    var enemiesRect = new Rect(position.x, currentY, position.width, EditorGUI.GetPropertyHeight(enemiesProp, true));
                    EditorGUI.PropertyField(enemiesRect, enemiesProp, new GUIContent("Enemies"), true);
                    currentY += EditorGUI.GetPropertyHeight(enemiesProp, true) + EditorGUIUtility.standardVerticalSpacing;
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
                    currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }

                if (spawnerSO.hasModifiedProperties)
                {
                    spawnerSO.ApplyModifiedProperties();
                    EditorUtility.SetDirty(spawner);
                }

                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!property.isExpanded || property.objectReferenceValue == null)
                return EditorGUIUtility.singleLineHeight;

            var spawner = property.objectReferenceValue as EnemySpawnerConfiguration;
            if (spawner == null)
                return EditorGUIUtility.singleLineHeight;

            var spawnerSO = new SerializedObject(spawner);
            var enemiesProp = spawnerSO.FindProperty("_enemies");
            var shuffleSpawnOrderProp = spawnerSO.FindProperty("_shuffleSpawnOrder");
            var spawnAmountProp = spawnerSO.FindProperty("_spawnAmount");
            var intervalProp = spawnerSO.FindProperty("_interval");

            float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Foldout

            if (enemiesProp != null)
                height += EditorGUI.GetPropertyHeight(enemiesProp, true) + EditorGUIUtility.standardVerticalSpacing;

            if (shuffleSpawnOrderProp != null)
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            if (spawnAmountProp != null)
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            if (intervalProp != null)
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            return height;
        }
    }
}