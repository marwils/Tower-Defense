using UnityEditor;
using UnityEngine;
using System;
using System.Linq;

namespace LevelSystem
{
    [CustomPropertyDrawer(typeof(Wave))]
    public class WavePropertyDrawer : PropertyDrawer
    {
        private Type[] _waveElementTypes;
        private string[] _waveElementTypeNames;

        private void InitializeTypes()
        {
            if (_waveElementTypes == null)
            {
                _waveElementTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(asm => asm.GetTypes())
                    .Where(t => t.IsSubclassOf(typeof(AbstractWaveElement)) && !t.IsAbstract)
                    .ToArray();

                _waveElementTypeNames = _waveElementTypes
                    .Select(t => ObjectNames.NicifyVariableName(t.Name))
                    .ToArray();
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            InitializeTypes();

            EditorGUI.BeginProperty(position, label, property);

            var wave = property.objectReferenceValue as Wave;
            if (wave == null)
            {
                EditorGUI.PropertyField(position, property, label);
                EditorGUI.EndProperty();
                return;
            }

            var currentY = position.y;

            // Wave header
            var headerRect = new Rect(position.x, currentY, position.width, EditorGUIUtility.singleLineHeight);
            var headerStyle = new GUIStyle(EditorStyles.boldLabel);
            headerStyle.fontSize = 14;
            EditorGUI.LabelField(headerRect, label.text, headerStyle);
            currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            // Content area
            var contentRect = new Rect(position.x, currentY, position.width, position.height - (currentY - position.y));
            DrawWaveContent(contentRect, property, wave);

            EditorGUI.EndProperty();
        }

        // Neue öffentliche Methode für das Zeichnen nur des Inhalts
        public void DrawWaveContent(Rect position, SerializedProperty property, Wave wave)
        {
            if (wave == null) return;

            InitializeTypes();

            var currentY = position.y;
            var indentedRect = new Rect(position.x, currentY, position.width, position.height);

            var waveSO = new SerializedObject(wave);
            waveSO.Update();

            currentY = indentedRect.y;

            // Draw title
            var titleProp = waveSO.FindProperty("_title");
            if (titleProp != null)
            {
                var titleRect = new Rect(indentedRect.x, currentY, indentedRect.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(titleRect, titleProp, new GUIContent("Title"));
                currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }

            // Wave Elements Header
            var elementsHeaderRect = new Rect(indentedRect.x, currentY, indentedRect.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(elementsHeaderRect, "Elements", EditorStyles.boldLabel);
            currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            // Draw elements
            var elementsProp = waveSO.FindProperty("_waveElements");
            if (elementsProp != null)
            {
                for (int i = 0; i < elementsProp.arraySize; i++)
                {
                    var elementProp = elementsProp.GetArrayElementAtIndex(i);
                    var element = elementProp.objectReferenceValue as AbstractWaveElement;

                    string elementLabel = element != null ? ObjectNames.NicifyVariableName(element.GetType().Name) : $"Element {i + 1}";
                    var elementGuiLabel = new GUIContent(elementLabel);

                    var elementHeight = ElementDrawerHelper.GetElementHeight(elementProp, elementGuiLabel, element);
                    var elementRect = new Rect(indentedRect.x, currentY, indentedRect.width, elementHeight);
                    var deleteRect = new Rect(indentedRect.x + indentedRect.width - 20, currentY, 20, EditorGUIUtility.singleLineHeight);

                    ElementDrawerHelper.DrawElement(elementRect, elementProp, elementGuiLabel, element);

                    // Delete button
                    if (GUI.Button(deleteRect, "-"))
                    {
                        if (element != null)
                        {
                            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(element));
                        }
                        elementsProp.DeleteArrayElementAtIndex(i);
                        waveSO.ApplyModifiedProperties();
                        EditorUtility.SetDirty(wave);
                        break;
                    }

                    currentY += elementHeight + EditorGUIUtility.standardVerticalSpacing;
                }

                // Add Element Button
                var addButtonRect = new Rect(indentedRect.x, currentY, 150, EditorGUIUtility.singleLineHeight);
                if (GUI.Button(addButtonRect, "Add Element"))
                {
                    ShowAddElementMenu(elementsProp, wave);
                }
            }

            if (waveSO.hasModifiedProperties)
            {
                waveSO.ApplyModifiedProperties();
                EditorUtility.SetDirty(wave);
            }
        }

        // Bestehende Methoden bleiben unverändert...
        private void ShowAddElementMenu(SerializedProperty elementsProperty, Wave wave)
        {
            var menu = new GenericMenu();

            for (int i = 0; i < _waveElementTypes.Length; i++)
            {
                var elementType = _waveElementTypes[i];
                var typeName = _waveElementTypeNames[i];

                menu.AddItem(new GUIContent(typeName), false, () =>
                {
                    var element = LevelAssetFactory.CreateWaveElement(elementType, wave);
                    elementsProperty.arraySize++;
                    var newElementProp = elementsProperty.GetArrayElementAtIndex(elementsProperty.arraySize - 1);
                    newElementProp.objectReferenceValue = element;

                    elementsProperty.serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(wave);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                });
            }

            menu.ShowAsContext();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.objectReferenceValue == null)
            {
                return EditorGUIUtility.singleLineHeight;
            }

            var wave = property.objectReferenceValue as Wave;
            if (wave == null) return EditorGUIUtility.singleLineHeight;

            var waveSO = new SerializedObject(wave);
            var elementsProp = waveSO.FindProperty("_waveElements");

            float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Header
            height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Title
            height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Elements header

            if (elementsProp != null)
            {
                // Calculate height for each element
                for (int i = 0; i < elementsProp.arraySize; i++)
                {
                    var elementProp = elementsProp.GetArrayElementAtIndex(i);
                    var element = elementProp.objectReferenceValue as AbstractWaveElement;

                    string elementLabel = element != null ? ObjectNames.NicifyVariableName(element.GetType().Name) : $"Element {i + 1}";
                    var elementGuiLabel = new GUIContent(elementLabel);

                    height += ElementDrawerHelper.GetElementHeight(elementProp, elementGuiLabel, element) + EditorGUIUtility.standardVerticalSpacing;
                }

                // Add button height
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }

            return height;
        }
    }
}