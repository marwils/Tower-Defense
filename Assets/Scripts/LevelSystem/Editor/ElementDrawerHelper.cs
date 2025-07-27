using UnityEditor;

using UnityEngine;

namespace LevelSystem
{
    public static class ElementDrawerHelper
    {
        private static WaveRoutesPropertyDrawer _waveRoutesDrawer;

        // Einmalige Initialisierung beim ersten Zugriff
        private static WaveRoutesPropertyDrawer WaveRoutesDrawer
        {
            get
            {
                if (_waveRoutesDrawer == null)
                    _waveRoutesDrawer = new WaveRoutesPropertyDrawer();
                return _waveRoutesDrawer;
            }
        }

        public static void DrawElement(Rect position, SerializedProperty elementProp, GUIContent label, AbstractWaveElement element)
        {
            if (element is WaveRoutes)
            {
                // WaveRoutes wird jetzt direkt ohne zusätzliche Box gerendert
                WaveRoutesDrawer.OnGUI(position, elementProp, label);
            }
            else if (element != null)
            {
                // Direkte Property-Darstellung ohne ScriptableObject-Referenz
                DrawInlineProperties(position, elementProp, label, element);
            }
            else
            {
                EditorGUI.PropertyField(position, elementProp, label);
            }
        }

        public static float GetElementHeight(SerializedProperty elementProp, GUIContent label, AbstractWaveElement element)
        {
            if (element is WaveRoutes)
            {
                return WaveRoutesDrawer.GetPropertyHeight(elementProp, label);
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

        // Bestehende Methoden bleiben unverändert...
        public static void DrawSequenceElement(Rect position, SerializedProperty elementProp, GUIContent label, AbstractSequenceElement element)
        {
            if (element != null)
            {
                DrawInlineSequenceProperties(position, elementProp, label, element);
            }
            else
            {
                EditorGUI.PropertyField(position, elementProp, label);
            }
        }

        public static float GetSequenceElementHeight(SerializedProperty elementProp, GUIContent label, AbstractSequenceElement element)
        {
            if (element != null)
            {
                return GetInlineSequencePropertiesHeight(elementProp, label, element);
            }
            else
            {
                return EditorGUIUtility.singleLineHeight;
            }
        }

        private static void DrawInlineProperties(Rect position, SerializedProperty elementProp, GUIContent label, AbstractWaveElement element)
        {
            var currentY = position.y;

            // Header
            var headerRect = new Rect(position.x, currentY, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(headerRect, label, EditorStyles.boldLabel);
            currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            EditorGUI.indentLevel++;

            var elementSO = new SerializedObject(element);
            elementSO.Update();

            // Zeichne alle serialisierten Properties
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

        private static float GetInlinePropertiesHeight(SerializedProperty elementProp, GUIContent label, AbstractWaveElement element)
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

        private static void DrawInlineSequenceProperties(Rect position, SerializedProperty elementProp, GUIContent label, AbstractSequenceElement element)
        {
            var currentY = position.y;

            // Header
            var headerRect = new Rect(position.x, currentY, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(headerRect, label, EditorStyles.boldLabel);
            currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            EditorGUI.indentLevel++;

            var elementSO = new SerializedObject(element);
            elementSO.Update();

            // Zeichne alle serialisierten Properties
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

        private static float GetInlineSequencePropertiesHeight(SerializedProperty elementProp, GUIContent label, AbstractSequenceElement element)
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