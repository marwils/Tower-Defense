using UnityEditor;
using UnityEngine;
using System;
using System.Linq;

namespace LevelSystem
{
    [CustomPropertyDrawer(typeof(WaveRoutes))]
    public class WaveRoutesPropertyDrawer : PropertyDrawer
    {
        private Type[] _sequenceElementTypes;
        private string[] _sequenceElementTypeNames;

        private void InitializeTypes()
        {
            if (_sequenceElementTypes == null)
            {
                _sequenceElementTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(asm => asm.GetTypes())
                    .Where(t => t.IsSubclassOf(typeof(AbstractSequenceElement)) && !t.IsAbstract)
                    .ToArray();

                _sequenceElementTypeNames = _sequenceElementTypes
                    .Select(t => ObjectNames.NicifyVariableName(t.Name))
                    .ToArray();
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            InitializeTypes();

            EditorGUI.BeginProperty(position, label, property);

            var waveRoutes = property.objectReferenceValue as WaveRoutes;
            if (waveRoutes == null)
            {
                EditorGUI.PropertyField(position, property, label);
                EditorGUI.EndProperty();
                return;
            }

            var currentY = position.y;

            // WaveRoutes header
            var headerRect = new Rect(position.x, currentY, position.width, EditorGUIUtility.singleLineHeight);
            var headerStyle = new GUIStyle(EditorStyles.boldLabel);
            headerStyle.fontSize = 14;
            EditorGUI.LabelField(headerRect, label.text, headerStyle);
            currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            // Content area (indented)
            var indentedRect = new Rect(position.x, currentY, position.width, position.height - (currentY - position.y));

            var waveRoutesSO = new SerializedObject(waveRoutes);
            waveRoutesSO.Update();

            currentY = indentedRect.y;

            // Draw routes
            var routesProp = waveRoutesSO.FindProperty("_routes");
            if (routesProp != null)
            {
                for (int i = 0; i < routesProp.arraySize; i++)
                {
                    var routeProp = routesProp.GetArrayElementAtIndex(i);
                    var route = routeProp.objectReferenceValue as WaveRoute;

                    string routeLabel = route != null ? $"Route {i + 1}" : $"Route {i + 1} (Missing)";
                    var routeGuiLabel = new GUIContent(routeLabel);

                    // Berechne Höhe für Route-Inhalt (ohne Box)
                    var routeContentHeight = GetWaveRouteContentHeight(routeProp, routeGuiLabel);
                    const float margin = 8f;

                    // KORRIGIERT: Box mit voller Breite (wie bei Waves)
                    var routeBoxRect = new Rect(indentedRect.x, currentY, indentedRect.width, routeContentHeight);

                    // Draw background box für Route
                    GUI.Box(routeBoxRect, "", EditorStyles.helpBox);

                    // Route header mit Minus-Button INNERHALB der Box
                    var routeHeaderRect = new Rect(indentedRect.x + margin, currentY + margin, indentedRect.width, EditorGUIUtility.singleLineHeight);
                    var deleteRect = new Rect(indentedRect.x + indentedRect.width - 20, currentY + margin, 20, EditorGUIUtility.singleLineHeight);

                    currentY += margin;

                    var routeHeaderStyle = new GUIStyle(EditorStyles.boldLabel);
                    routeHeaderStyle.fontSize = 12;
                    EditorGUI.LabelField(routeHeaderRect, routeLabel, routeHeaderStyle);

                    // Delete Button in der Header-Zeile INNERHALB der Box
                    if (GUI.Button(deleteRect, "-"))
                    {
                        if (route != null)
                        {
                            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(route));
                        }
                        routesProp.DeleteArrayElementAtIndex(i);
                        waveRoutesSO.ApplyModifiedProperties();
                        EditorUtility.SetDirty(waveRoutes);
                        break;
                    }

                    // Content area für Route (unter dem Header)
                    var routeContentY = currentY + margin + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    var routeContentHeightAdjusted = routeContentHeight - EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing;
                    var routeContentRect = new Rect(indentedRect.x, routeContentY, indentedRect.width, routeContentHeightAdjusted);

                    // Zeichne Route-Inhalt (ohne den Header, da wir den schon gezeichnet haben)
                    DrawWaveRouteContent(routeContentRect, routeProp, route);

                    currentY += routeContentHeight + EditorGUIUtility.standardVerticalSpacing;
                }

                // Add Route Button
                var addButtonRect = new Rect(indentedRect.x, currentY, 150, EditorGUIUtility.singleLineHeight);
                if (GUI.Button(addButtonRect, "Add Route"))
                {
                    var route = LevelAssetFactory.CreateWaveRoute(waveRoutes);
                    routesProp.arraySize++;
                    var newRouteProp = routesProp.GetArrayElementAtIndex(routesProp.arraySize - 1);
                    newRouteProp.objectReferenceValue = route;
                    waveRoutesSO.ApplyModifiedProperties();
                    EditorUtility.SetDirty(waveRoutes);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }

            if (waveRoutesSO.hasModifiedProperties)
            {
                waveRoutesSO.ApplyModifiedProperties();
                EditorUtility.SetDirty(waveRoutes);
            }

            EditorGUI.EndProperty();
        }

        private void DrawWaveRouteContent(Rect position, SerializedProperty routeProp, WaveRoute route)
        {
            if (route == null) return;

            var currentY = position.y;
            const float leftIndent = 5f;
            var indentedRect = new Rect(position.x + leftIndent, currentY, position.width - leftIndent, position.height);

            var routeSO = new SerializedObject(route);
            routeSO.Update();

            currentY = indentedRect.y;

            // Draw WaveRoute properties
            var spawnPointProp = routeSO.FindProperty("_spawnPoint");
            var targetPointProp = routeSO.FindProperty("_targetPoint");
            var sequenceElementsProp = routeSO.FindProperty("_sequenceElements");

            // Spawn Point
            if (spawnPointProp != null)
            {
                var spawnPointRect = new Rect(indentedRect.x, currentY, indentedRect.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(spawnPointRect, spawnPointProp, new GUIContent("Spawn Point"));
                currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }

            // Target Point
            if (targetPointProp != null)
            {
                var targetPointRect = new Rect(indentedRect.x, currentY, indentedRect.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(targetPointRect, targetPointProp, new GUIContent("Target Point"));
                currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }

            // Sequence Elements Section
            DrawSequenceElementsSection(indentedRect, routeSO, sequenceElementsProp, ref currentY, route);

            if (routeSO.hasModifiedProperties)
            {
                routeSO.ApplyModifiedProperties();
                EditorUtility.SetDirty(route);
            }
        }

        private void DrawSequenceElementsSection(Rect position, SerializedObject routeSO, SerializedProperty sequenceElementsProp, ref float currentY, WaveRoute route)
        {
            // Sequence Elements Header
            var headerRect = new Rect(position.x, currentY, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(headerRect, "Sequence Elements", EditorStyles.boldLabel);
            currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            if (sequenceElementsProp != null)
            {
                // Draw each sequence element
                for (int i = 0; i < sequenceElementsProp.arraySize; i++)
                {
                    var elementProp = sequenceElementsProp.GetArrayElementAtIndex(i);
                    var element = elementProp.objectReferenceValue as AbstractSequenceElement;

                    string elementLabel = element != null ?
                        $"{ObjectNames.NicifyVariableName(element.GetType().Name)} {i + 1}" :
                        $"Element {i + 1} (Missing)";

                    var elementHeight = GetSequenceElementHeight(elementProp, new GUIContent(elementLabel), element);
                    var elementRect = new Rect(position.x, currentY, position.width - 25, elementHeight);
                    var deleteRect = new Rect(position.x + position.width - 20, currentY, 20, EditorGUIUtility.singleLineHeight);

                    // Draw the sequence element with inline properties
                    DrawSequenceElement(elementRect, elementProp, new GUIContent(elementLabel), element);

                    // Delete button
                    if (GUI.Button(deleteRect, "-"))
                    {
                        if (element != null)
                        {
                            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(element));
                        }
                        sequenceElementsProp.DeleteArrayElementAtIndex(i);
                        routeSO.ApplyModifiedProperties();
                        EditorUtility.SetDirty(route);
                        break;
                    }

                    currentY += elementHeight + EditorGUIUtility.standardVerticalSpacing;
                }

                // Show message if no elements
                if (sequenceElementsProp.arraySize == 0)
                {
                    var noElementsRect = new Rect(position.x, currentY, position.width, EditorGUIUtility.singleLineHeight);
                    var oldColor = GUI.color;
                    GUI.color = new Color(1f, 1f, 0.8f);
                    GUI.Box(noElementsRect, "", EditorStyles.helpBox);
                    GUI.color = oldColor;

                    var labelStyle = new GUIStyle(EditorStyles.label);
                    labelStyle.fontStyle = FontStyle.Italic;
                    EditorGUI.LabelField(noElementsRect, "No Sequence Elements assigned", labelStyle);
                    currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }

                // Add Element Button
                var addButtonRect = new Rect(position.x, currentY, 150, EditorGUIUtility.singleLineHeight);
                if (GUI.Button(addButtonRect, "Add Element"))
                {
                    ShowAddElementMenu(sequenceElementsProp, route);
                }
            }
        }

        private void DrawSequenceElement(Rect position, SerializedProperty elementProp, GUIContent label, AbstractSequenceElement element)
        {
            if (element != null)
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
            else
            {
                EditorGUI.PropertyField(position, elementProp, label);
            }
        }

        private float GetSequenceElementHeight(SerializedProperty elementProp, GUIContent label, AbstractSequenceElement element)
        {
            if (element != null)
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
            else
            {
                return EditorGUIUtility.singleLineHeight;
            }
        }

        private void ShowAddElementMenu(SerializedProperty sequenceElementsProp, WaveRoute route)
        {
            var menu = new GenericMenu();

            for (int i = 0; i < _sequenceElementTypes.Length; i++)
            {
                var elementType = _sequenceElementTypes[i];
                var typeName = _sequenceElementTypeNames[i];

                menu.AddItem(new GUIContent(typeName), false, () =>
                {
                    var element = LevelAssetFactory.CreateSequenceElement(elementType, route);

                    // Add to array
                    sequenceElementsProp.arraySize++;
                    var newElementProp = sequenceElementsProp.GetArrayElementAtIndex(sequenceElementsProp.arraySize - 1);
                    newElementProp.objectReferenceValue = element;

                    sequenceElementsProp.serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(route);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                });
            }

            menu.ShowAsContext();
        }

        private float GetWaveRouteContentHeight(SerializedProperty routeProp, GUIContent label)
        {
            InitializeTypes();

            if (routeProp.objectReferenceValue == null)
            {
                return EditorGUIUtility.singleLineHeight;
            }

            var route = routeProp.objectReferenceValue as WaveRoute;
            if (route == null) return EditorGUIUtility.singleLineHeight;

            var routeSO = new SerializedObject(route);
            var sequenceElementsProp = routeSO.FindProperty("_sequenceElements");

            float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Route Header
            height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Spawn Point
            height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Target Point
            height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Elements Header

            if (sequenceElementsProp != null)
            {
                if (sequenceElementsProp.arraySize == 0)
                {
                    height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }
                else
                {
                    for (int i = 0; i < sequenceElementsProp.arraySize; i++)
                    {
                        var elementProp = sequenceElementsProp.GetArrayElementAtIndex(i);
                        var element = elementProp.objectReferenceValue as AbstractSequenceElement;
                        string elementLabel = element != null ?
                            $"{ObjectNames.NicifyVariableName(element.GetType().Name)} {i + 1}" :
                            $"Element {i + 1} (Missing)";

                        height += GetSequenceElementHeight(elementProp, new GUIContent(elementLabel), element) + EditorGUIUtility.standardVerticalSpacing;
                    }
                }

                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Add button
            }

            return height;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.objectReferenceValue == null)
            {
                return EditorGUIUtility.singleLineHeight;
            }

            var waveRoutes = property.objectReferenceValue as WaveRoutes;
            if (waveRoutes == null) return EditorGUIUtility.singleLineHeight;

            var waveRoutesSO = new SerializedObject(waveRoutes);
            var routesProp = waveRoutesSO.FindProperty("_routes");

            float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Header
            height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Routes header

            if (routesProp != null)
            {
                const float margin = 8f;
                for (int i = 0; i < routesProp.arraySize; i++)
                {
                    var routeProp = routesProp.GetArrayElementAtIndex(i);
                    var route = routeProp.objectReferenceValue as WaveRoute;
                    string routeLabel = route != null ? $"Route {i + 1}" : $"Route {i + 1} (Missing)";

                    var routeContentHeight = GetWaveRouteContentHeight(routeProp, new GUIContent(routeLabel));
                    var totalRouteHeight = routeContentHeight + margin * 2; // Box-Margins

                    height += totalRouteHeight + EditorGUIUtility.standardVerticalSpacing;
                }

                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Add button
            }

            return height;
        }
    }
}