using UnityEditor;

using UnityEngine;

namespace MarwilsTD.LevelSystem
{
    public static class ElementDrawerHelper
    {
        private static EnemySpawnerPropertyDrawer _sequenceSpawnerDrawer;
        private static SpawnPlanPropertyDrawer _spawnPlanDrawer;

        private static EnemySpawnerPropertyDrawer SequenceSpawnerDrawer
        {
            get
            {
                return _sequenceSpawnerDrawer ??= new EnemySpawnerPropertyDrawer();
            }
        }

        private static SpawnPlanPropertyDrawer SpawnPlanDrawer
        {
            get
            {
                return _spawnPlanDrawer ??= new SpawnPlanPropertyDrawer();
            }
        }

        public static void DrawElement(Rect position, SerializedProperty elementProp, GUIContent label, WaveElementConfiguration element)
        {
            if (element is SpawnPlanConfiguration)
            {
                DrawSpawnPlan(position, elementProp, label, element as SpawnPlanConfiguration);
            }
            else if (element != null)
            {
                DrawInlineProperties(position, elementProp, label, element);
            }
            else
            {
                EditorGUI.PropertyField(position, elementProp, label);
            }
        }

        public static float GetElementHeight(SerializedProperty elementProp, GUIContent label, WaveElementConfiguration element)
        {
            if (element is SpawnPlanConfiguration)
            {
                return GetSpawnPlanHeight(elementProp, label, element as SpawnPlanConfiguration);
            }
            else if (element != null)
            {
                return GetInlinePropertiesHeight(elementProp, label, element);
            }
            else
            {
                return EditorGUIUtility.singleLineHeight;
            }
        }

        public static void DrawSequenceElement(Rect position, SerializedProperty elementProp, GUIContent label, SequenceElementConfiguration element)
        {
            if (element is EnemySpawnerConfiguration)
            {
                SequenceSpawnerDrawer.OnGUI(position, elementProp, label);
            }
            else if (element != null)
            {
                DrawInlineSequenceProperties(position, elementProp, label, element);
            }
            else
            {
                EditorGUI.PropertyField(position, elementProp, label);
            }
        }

        public static float GetSequenceElementHeight(SerializedProperty elementProp, GUIContent label, SequenceElementConfiguration element)
        {
            if (element is EnemySpawnerConfiguration)
            {
                return SequenceSpawnerDrawer.GetPropertyHeight(elementProp, label);
            }
            else if (element != null)
            {
                return GetInlineSequencePropertiesHeight(elementProp, label, element);
            }
            else
            {
                return EditorGUIUtility.singleLineHeight;
            }
        }

        private static void DrawSpawnPlan(Rect position, SerializedProperty elementProp, GUIContent label, SpawnPlanConfiguration spawnPlan)
        {
            SpawnPlanDrawer.OnGUI(position, elementProp, label);
        }

        private static float GetSpawnPlanHeight(SerializedProperty elementProp, GUIContent label, SpawnPlanConfiguration spawnPlan)
        {
            return SpawnPlanDrawer.GetPropertyHeight(elementProp, label);
        }

        private static void DrawInlineProperties(Rect position, SerializedProperty elementProp, GUIContent label, WaveElementConfiguration element)
        {
            var currentY = position.y;
            var headerRect = new Rect(position.x, currentY, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(headerRect, label, EditorStyles.boldLabel);
            currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            EditorGUI.indentLevel++;

            var elementSO = new SerializedObject(element);
            elementSO.Update();
            var iterator = elementSO.GetIterator();
            iterator.NextVisible(true); // Skip m_Script

            while (iterator.NextVisible(false))
            {
                var propHeight = EditorGUI.GetPropertyHeight(iterator, true);
                var propRect = new Rect(position.x, currentY, position.width, propHeight);
                EditorGUI.PropertyField(propRect, iterator, true);
                currentY += propHeight + EditorGUIUtility.standardVerticalSpacing;
            }

            if (elementSO.hasModifiedProperties)
            {
                elementSO.ApplyModifiedProperties();
                EditorUtility.SetDirty(element);
            }

            EditorGUI.indentLevel--;
        }

        private static float GetInlinePropertiesHeight(SerializedProperty elementProp, GUIContent label, WaveElementConfiguration element)
        {
            float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Header

            var elementSO = new SerializedObject(element);
            var iterator = elementSO.GetIterator();
            iterator.NextVisible(true); // Skip m_Script

            while (iterator.NextVisible(false))
            {
                height += EditorGUI.GetPropertyHeight(iterator, true) + EditorGUIUtility.standardVerticalSpacing;
            }

            return height;
        }

        private static void DrawInlineSequenceProperties(Rect position, SerializedProperty elementProp, GUIContent label, SequenceElementConfiguration element)
        {
            var currentY = position.y;
            var headerRect = new Rect(position.x, currentY, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(headerRect, label, EditorStyles.boldLabel);
            currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            EditorGUI.indentLevel++;

            var elementSO = new SerializedObject(element);
            elementSO.Update();
            var iterator = elementSO.GetIterator();
            iterator.NextVisible(true); // Skip m_Script

            while (iterator.NextVisible(false))
            {
                var propHeight = EditorGUI.GetPropertyHeight(iterator, true);
                var propRect = new Rect(position.x, currentY, position.width, propHeight);
                EditorGUI.PropertyField(propRect, iterator, true);
                currentY += propHeight + EditorGUIUtility.standardVerticalSpacing;
            }

            if (elementSO.hasModifiedProperties)
            {
                elementSO.ApplyModifiedProperties();
                EditorUtility.SetDirty(element);
            }

            EditorGUI.indentLevel--;
        }

        private static float GetInlineSequencePropertiesHeight(SerializedProperty elementProp, GUIContent label, SequenceElementConfiguration element)
        {
            float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Header

            var elementSO = new SerializedObject(element);
            var iterator = elementSO.GetIterator();
            iterator.NextVisible(true); // Skip m_Script

            while (iterator.NextVisible(false))
            {
                height += EditorGUI.GetPropertyHeight(iterator, true) + EditorGUIUtility.standardVerticalSpacing;
            }

            return height;
        }
    }
}