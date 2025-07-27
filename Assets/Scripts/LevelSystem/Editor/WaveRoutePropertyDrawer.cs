using UnityEditor;
using UnityEngine;
using System;
using System.Linq;

namespace LevelSystem
{
    [CustomPropertyDrawer(typeof(WaveRoute))]
    public class WaveRoutePropertyDrawer : PropertyDrawer
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

            var waveRoute = property.objectReferenceValue as WaveRoute;
            if (waveRoute == null)
            {
                EditorGUI.PropertyField(position, property, label);
                EditorGUI.EndProperty();
                return;
            }

            // Foldout
            var foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;

                var waveRouteSO = new SerializedObject(waveRoute);
                waveRouteSO.Update();

                var currentY = position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                // Draw WaveRoute properties
                var spawnPointIdProp = waveRouteSO.FindProperty("_spawnPointId");
                var targetPointIdProp = waveRouteSO.FindProperty("_targetPointId");
                var sequenceElementsProp = waveRouteSO.FindProperty("_sequenceElements");

                // Spawn Point ID Dropdown
                if (spawnPointIdProp != null)
                {
                    if (string.IsNullOrEmpty(spawnPointIdProp.stringValue) || spawnPointIdProp.stringValue == "None")
                    {
                        GUIHelper.DrawWarning("No Spawn Point assigned", position, ref currentY);
                    }

                    var spawnPointRect = new Rect(position.x, currentY, position.width, EditorGUIUtility.singleLineHeight);
                    DrawSpawnNameDropdown(spawnPointRect, spawnPointIdProp);
                    currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }

                // Target Point ID Dropdown
                if (targetPointIdProp != null)
                {
                    if (string.IsNullOrEmpty(targetPointIdProp.stringValue) || targetPointIdProp.stringValue == "None")
                    {
                        GUIHelper.DrawWarning("No Target Point assigned", position, ref currentY);
                    }
                    var targetPointRect = new Rect(position.x, currentY, position.width, EditorGUIUtility.singleLineHeight);
                    DrawTargetNameDropdown(targetPointRect, targetPointIdProp);
                    currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }

                // Sequence Elements Section
                DrawSequenceElementsSection(position, waveRouteSO, sequenceElementsProp, ref currentY, waveRoute);

                if (waveRouteSO.hasModifiedProperties)
                {
                    waveRouteSO.ApplyModifiedProperties();
                    EditorUtility.SetDirty(waveRoute);
                }

                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        private void DrawSpawnNameDropdown(Rect position, SerializedProperty spawnPointIdProp)
        {
            // Hole alle verfügbaren Spawn Point IDs
            var availableIds = GetAvailableSpawnPointNames();
            var currentId = spawnPointIdProp.stringValue;
            var currentIndex = System.Array.IndexOf(availableIds, currentId);

            // Füge "None" Option hinzu
            var displayOptions = new string[availableIds.Length + 1];
            displayOptions[0] = "None";
            System.Array.Copy(availableIds, 0, displayOptions, 1, availableIds.Length);

            // Adjustiere Index für "None" Option
            var displayIndex = currentIndex >= 0 ? currentIndex + 1 : 0;

            EditorGUI.BeginChangeCheck();
            displayIndex = EditorGUI.Popup(position, "Spawn Point", displayIndex, displayOptions);
            if (EditorGUI.EndChangeCheck())
            {
                if (displayIndex == 0)
                {
                    spawnPointIdProp.stringValue = "";
                }
                else if (displayIndex > 0 && displayIndex <= availableIds.Length)
                {
                    spawnPointIdProp.stringValue = availableIds[displayIndex - 1];
                }
            }

            // Zeige Warnung wenn ID nicht existiert
            if (!string.IsNullOrEmpty(currentId) && currentIndex < 0)
            {
                var warningRect = new Rect(position.x + position.width - 20, position.y, 20, position.height);
                var oldColor = GUI.color;
                GUI.color = Color.yellow;
                EditorGUI.LabelField(warningRect, "⚠", EditorStyles.boldLabel);
                GUI.color = oldColor;
            }
        }

        private void DrawTargetNameDropdown(Rect position, SerializedProperty targetPointIdProp)
        {
            // Hole alle verfügbaren Target Point IDs
            var availableIds = GetAvailableTargetPointNames();
            var currentId = targetPointIdProp.stringValue;
            var currentIndex = System.Array.IndexOf(availableIds, currentId);

            // Füge "None" Option hinzu
            var displayOptions = new string[availableIds.Length + 1];
            displayOptions[0] = "None";
            System.Array.Copy(availableIds, 0, displayOptions, 1, availableIds.Length);

            // Adjustiere Index für "None" Option
            var displayIndex = currentIndex >= 0 ? currentIndex + 1 : 0;

            EditorGUI.BeginChangeCheck();
            displayIndex = EditorGUI.Popup(position, "Target Point", displayIndex, displayOptions);
            if (EditorGUI.EndChangeCheck())
            {
                if (displayIndex == 0)
                {
                    targetPointIdProp.stringValue = "";
                }
                else if (displayIndex > 0 && displayIndex <= availableIds.Length)
                {
                    targetPointIdProp.stringValue = availableIds[displayIndex - 1];
                }
            }

            // Zeige Warnung wenn ID nicht existiert
            if (!string.IsNullOrEmpty(currentId) && currentIndex < 0)
            {
                var warningRect = new Rect(position.x + position.width - 20, position.y, 20, position.height);
                var oldColor = GUI.color;
                GUI.color = Color.yellow;
                EditorGUI.LabelField(warningRect, "⚠", EditorStyles.boldLabel);
                GUI.color = oldColor;
            }
        }

        private string[] GetAvailableSpawnPointNames()
        {
            var spawnPoints = UnityEngine.Object.FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);
            return spawnPoints.Where(sp => !string.IsNullOrEmpty(sp.gameObject.name))
                              .Select(sp => sp.gameObject.name)
                              .Distinct()
                              .OrderBy(id => id)
                              .ToArray();
        }

        private string[] GetAvailableTargetPointNames()
        {
            var targetPoints = UnityEngine.Object.FindObjectsByType<TargetPoint>(FindObjectsSortMode.None);
            return targetPoints.Where(tp => !string.IsNullOrEmpty(tp.gameObject.name))
                               .Select(tp => tp.gameObject.name)
                               .Distinct()
                               .OrderBy(id => id)
                               .ToArray();
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

                    var elementHeight = ElementDrawerHelper.GetSequenceElementHeight(elementProp, new GUIContent(elementLabel), element);
                    var elementRect = new Rect(position.x, currentY, position.width - 25, elementHeight);
                    var deleteRect = new Rect(position.x + position.width - 20, currentY, 20, EditorGUIUtility.singleLineHeight);

                    // Draw the sequence element with inline properties
                    ElementDrawerHelper.DrawSequenceElement(elementRect, elementProp, new GUIContent(elementLabel), element);

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
                    GUIHelper.DrawWarning("No Sequence Elements assigned", position, ref currentY);
                }

                // Add Element Button
                var addButtonRect = new Rect(position.x, currentY, 150, EditorGUIUtility.singleLineHeight);
                if (GUI.Button(addButtonRect, "Add Element"))
                {
                    ShowAddElementMenu(sequenceElementsProp, route);
                }
            }
        }

        private void ShowAddElementMenu(SerializedProperty sequenceElementsProp, WaveRoute waveRoute)
        {
            var menu = new GenericMenu();

            for (int i = 0; i < _sequenceElementTypes.Length; i++)
            {
                var elementType = _sequenceElementTypes[i];
                var typeName = _sequenceElementTypeNames[i];

                menu.AddItem(new GUIContent(typeName), false, () =>
                {
                    var element = LevelAssetFactory.CreateSequenceElement(elementType, waveRoute);

                    // Add to array
                    sequenceElementsProp.arraySize++;
                    var newElementProp = sequenceElementsProp.GetArrayElementAtIndex(sequenceElementsProp.arraySize - 1);
                    newElementProp.objectReferenceValue = element;

                    sequenceElementsProp.serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(waveRoute);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                });
            }

            menu.ShowAsContext();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!property.isExpanded || property.objectReferenceValue == null)
            {
                return EditorGUIUtility.singleLineHeight;
            }

            var waveRoute = property.objectReferenceValue as WaveRoute;
            if (waveRoute == null) return EditorGUIUtility.singleLineHeight;

            var waveRouteSO = new SerializedObject(waveRoute);

            var spawnPointIdProp = waveRouteSO.FindProperty("_spawnPointId");
            var targetPointIdProp = waveRouteSO.FindProperty("_targetPointId");
            var sequenceElementsProp = waveRouteSO.FindProperty("_sequenceElements");

            float height = EditorGUIUtility.singleLineHeight; // Foldout
            height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Spawn Point ID
            if (string.IsNullOrEmpty(spawnPointIdProp.stringValue) || spawnPointIdProp.stringValue == "None")
            {
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Target Point ID
            if (string.IsNullOrEmpty(targetPointIdProp.stringValue) || targetPointIdProp.stringValue == "None")
            {
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Elements Header

            if (sequenceElementsProp != null)
            {
                if (sequenceElementsProp.arraySize == 0)
                {
                    // Height for "No elements" message
                    height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }
                else
                {
                    // Height for all sequence elements
                    for (int i = 0; i < sequenceElementsProp.arraySize; i++)
                    {
                        var elementProp = sequenceElementsProp.GetArrayElementAtIndex(i);
                        var element = elementProp.objectReferenceValue as AbstractSequenceElement;
                        string elementLabel = element != null ?
                            $"{ObjectNames.NicifyVariableName(element.GetType().Name)} {i + 1}" :
                            $"Element {i + 1} (Missing)";

                        height += ElementDrawerHelper.GetSequenceElementHeight(elementProp, new GUIContent(elementLabel), element) + EditorGUIUtility.standardVerticalSpacing;
                    }
                }

                // Add button height
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }

            return height;
        }
    }
}