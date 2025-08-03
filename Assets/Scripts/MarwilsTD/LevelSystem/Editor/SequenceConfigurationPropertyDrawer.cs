using UnityEditor;
using UnityEngine;
using System.Linq;

namespace MarwilsTD.LevelSystem
{
    [CustomPropertyDrawer(typeof(SequenceConfiguration))]
    public class SpawnPlanSequencePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var sequence = property.objectReferenceValue as SequenceConfiguration;
            if (sequence == null)
            {
                EditorGUI.PropertyField(position, property, label);
                EditorGUI.EndProperty();
                return;
            }

            var sequenceSO = new SerializedObject(sequence);
            sequenceSO.Update();

            float currentY = position.y;
            var routeProp = sequenceSO.FindProperty("_route");
            if (routeProp != null)
            {
                var parentLevel = FindParentLevel(sequence);
                RouteConfiguration[] routes = parentLevel != null ? parentLevel.Routes.ToArray() : new RouteConfiguration[0];
                string[] routeNames = routes.Select(r => r != null ? r.Title : "<None>").ToArray();

                int currentIndex = -1;
                var currentRoute = routeProp.objectReferenceValue as RouteConfiguration;
                if (currentRoute != null)
                    currentIndex = System.Array.IndexOf(routes, currentRoute);
                else if (routes.Length > 0)
                    currentIndex = 0;

                var routeRect = new Rect(position.x, currentY, position.width, EditorGUIUtility.singleLineHeight);
                int selected = EditorGUI.Popup(routeRect, "Route", currentIndex, routeNames);

                if (selected >= 0 && selected < routes.Length && routes[selected] != currentRoute)
                {
                    routeProp.objectReferenceValue = routes[selected];
                    sequenceSO.ApplyModifiedProperties();
                    EditorUtility.SetDirty(sequence);
                }
                currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            var elementsProp = sequenceSO.FindProperty("_sequenceElements");
            if (elementsProp != null)
            {
                DrawSequenceElementsSection(position, sequenceSO, elementsProp, ref currentY, sequence);
            }

            if (sequenceSO.hasModifiedProperties)
            {
                sequenceSO.ApplyModifiedProperties();
                EditorUtility.SetDirty(sequence);
            }

            EditorGUI.EndProperty();
        }

        private void DrawSequenceElementsSection(Rect position, SerializedObject sequenceSO, SerializedProperty elementsProp, ref float currentY, SequenceConfiguration sequence)
        {
            var headerRect = new Rect(position.x, currentY, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(headerRect, "Sequence Elements", EditorStyles.boldLabel);
            currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            for (int i = 0; i < elementsProp.arraySize; i++)
            {
                var elementProp = elementsProp.GetArrayElementAtIndex(i);
                var element = elementProp.objectReferenceValue as SequenceElementConfiguration;

                string elementLabel = element != null ?
                    $"{ObjectNames.NicifyVariableName(element.GetType().Name)} {i + 1}" :
                    $"Element {i + 1} (Missing)";

                var elementHeight = ElementDrawerHelper.GetSequenceElementHeight(elementProp, new GUIContent(elementLabel), element);
                var elementRect = new Rect(position.x, currentY, position.width - 25, elementHeight);
                var deleteRect = new Rect(position.x + position.width - 20, currentY, 20, EditorGUIUtility.singleLineHeight);

                ElementDrawerHelper.DrawSequenceElement(elementRect, elementProp, new GUIContent(elementLabel), element);

                if (GUI.Button(deleteRect, "-"))
                {
                    if (element != null)
                    {
                        var elementName = element.name;
                        bool isFactoryElement = elementName.Contains("_") && elementName.Length >= 21;
                        if (isFactoryElement)
                        {
                            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(element));
                        }
                    }
                    elementsProp.DeleteArrayElementAtIndex(i);
                    sequenceSO.ApplyModifiedProperties();
                    EditorUtility.SetDirty(sequence);
                    break;
                }

                currentY += elementHeight + EditorGUIUtility.standardVerticalSpacing;
            }

            if (elementsProp.arraySize == 0)
            {
                GUIHelper.DrawWarning("No Sequence Elements assigned", position, ref currentY);
            }

            var addButtonRect = new Rect(position.x, currentY, position.width, EditorGUIUtility.singleLineHeight);
            if (GUI.Button(addButtonRect, "Add Sequence Element"))
            {
                ShowAddElementMenu(elementsProp, sequence);
            }
            currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }

        private void ShowAddElementMenu(SerializedProperty elementsProp, SequenceConfiguration sequence)
        {
            var menu = new GenericMenu();

            var sequenceElementTypes = System.AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(asm => asm.GetTypes())
                .Where(t => t.IsSubclassOf(typeof(SequenceElementConfiguration)) && !t.IsAbstract)
                .ToArray();

            foreach (var elementType in sequenceElementTypes)
            {
                var typeName = ObjectNames.NicifyVariableName(elementType.Name);
                menu.AddItem(new GUIContent(typeName), false, () =>
                {
                    var element = LevelAssetFactory.CreateSequenceElement(elementType, sequence);
                    elementsProp.arraySize++;
                    var newElementProp = elementsProp.GetArrayElementAtIndex(elementsProp.arraySize - 1);
                    newElementProp.objectReferenceValue = element;

                    elementsProp.serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(sequence);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                });
            }

            menu.ShowAsContext();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.objectReferenceValue == null)
                return 0;

            var sequence = property.objectReferenceValue as SequenceConfiguration;
            if (sequence == null)
                return 0;

            var sequenceSO = new SerializedObject(sequence);
            var elementsProp = sequenceSO.FindProperty("_sequenceElements");

            float height = 0;

            // Route
            height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            // Sequence Elements Header
            height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            if (elementsProp != null)
            {
                if (elementsProp.arraySize == 0)
                {
                    height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Warning
                }
                else
                {
                    for (int i = 0; i < elementsProp.arraySize; i++)
                    {
                        var elementProp = elementsProp.GetArrayElementAtIndex(i);
                        var element = elementProp.objectReferenceValue as SequenceElementConfiguration;
                        string elementLabel = element != null ?
                            $"{ObjectNames.NicifyVariableName(element.GetType().Name)} {i + 1}" :
                            $"Element {i + 1} (Missing)";

                        height += ElementDrawerHelper.GetSequenceElementHeight(elementProp, new GUIContent(elementLabel), element)
                                  + EditorGUIUtility.standardVerticalSpacing;
                    }
                }
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Add-Button
            }

            return height;
        }

        private LevelConfiguration FindParentLevel(SequenceConfiguration sequence)
        {
            var guids = AssetDatabase.FindAssets($"t:{nameof(LevelSystem)}.{nameof(LevelConfiguration)}");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var level = AssetDatabase.LoadAssetAtPath<LevelConfiguration>(path);
                if (level == null) continue;

                foreach (var wave in level.Waves)
                {
                    if (wave == null) continue;

                    foreach (var elements in wave.WaveElements)
                    {
                        if (elements == null) continue;
                        if (elements is SpawnPlanConfiguration spawnPlan)
                        {
                            foreach (var seq in spawnPlan.Sequences)
                            {
                                if (seq == sequence)
                                    return level;
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}