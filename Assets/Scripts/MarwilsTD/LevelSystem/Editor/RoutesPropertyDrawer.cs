using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

namespace MarwilsTD.LevelSystem
{
    [CustomPropertyDrawer(typeof(List<Route>))]
    public class RoutesPropertyDrawer : PropertyDrawer
    {
        private RoutePropertyDrawer _routeDrawer = new RoutePropertyDrawer();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var currentY = position.y;

            EditorGUI.indentLevel++;

            for (int i = 0; i < property.arraySize; i++)
            {
                var routeProperty = property.GetArrayElementAtIndex(i);
                var routeHeight = _routeDrawer.GetPropertyHeight(routeProperty, new GUIContent($"Route {i + 1}"));

                var route = routeProperty.objectReferenceValue as Route;
                if (route == null)
                {
                    continue;
                }

                var headerTitle = $"Route {i + 1}";
                if (!string.IsNullOrEmpty(route.Title) && route.Title != headerTitle)
                {
                    headerTitle += $" - {route.Title}";
                }

                float totalHeight = GetRouteHeight(routeProperty);

                var boxRect = new Rect(position.x, currentY, position.width, totalHeight);
                GUI.Box(boxRect, "", EditorStyles.helpBox);


                var headerWaveRect = new Rect(position.x, currentY + Constants.MarginVertical, position.width - 20 - Constants.MarginVertical, EditorGUIUtility.singleLineHeight);
                var headerStyle = new GUIStyle(EditorStyles.boldLabel);
                headerStyle.fontSize = 14;
                EditorGUI.LabelField(headerWaveRect, headerTitle, headerStyle);

                var deleteMinusButtonRect = new Rect(position.x + position.width - 20 - Constants.MarginVertical, currentY + Constants.MarginVertical, 20, EditorGUIUtility.singleLineHeight);
                if (GUI.Button(deleteMinusButtonRect, "-"))
                {
                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(route));
                    property.DeleteArrayElementAtIndex(i);
                    property.serializedObject.ApplyModifiedProperties();
                    break;
                }

                var addPlusButtonRect = new Rect(position.x + position.width - 25 * 2, currentY + Constants.MarginVertical, 20, EditorGUIUtility.singleLineHeight);
                DrawAddButton(property, addPlusButtonRect, "+");

                var contentY = currentY + Constants.MarginVertical * 2 + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                var contentHeight = routeHeight - EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing;
                var contentRect = new Rect(position.x + Constants.MarginHorizontal, contentY, position.width - Constants.MarginHorizontal * 2, contentHeight);

                DrawRouteContent(contentRect, routeProperty);

                currentY += totalHeight + Constants.MarginVertical;
            }

            var addButtonRect = new Rect(position.x, currentY, position.width, EditorGUIUtility.singleLineHeight);
            DrawAddButton(property, addButtonRect, "Add Route");

            EditorGUI.indentLevel--;

            EditorGUI.EndProperty();
        }

        private static void DrawAddButton(SerializedProperty property, Rect addRect, string label)
        {
            if (GUI.Button(addRect, label))
            {
                var level = property.serializedObject.targetObject as Level;
                var route = LevelAssetFactory.CreateRoute(level);
                route.Title = $"Route {property.arraySize + 1}";
                property.arraySize++;
                var newRouteProp = property.GetArrayElementAtIndex(property.arraySize - 1);
                newRouteProp.objectReferenceValue = route;

                level.UpdateSceneContext();

                property.serializedObject.ApplyModifiedProperties();
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        private void DrawRouteContent(Rect position, SerializedProperty routeProperty)
        {
            var route = routeProperty.objectReferenceValue as Route;
            if (route == null) return;

            _routeDrawer.DrawRouteContent(position, route);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = 0;

            for (int i = 0; i < property.arraySize; i++)
            {
                var routeProperty = property.GetArrayElementAtIndex(i);
                height += GetRouteHeight(routeProperty) + Constants.MarginVertical;
            }

            height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Add Button

            return height;
        }

        private float GetRouteHeight(SerializedProperty routeProperty)
        {
            var routeHeight = _routeDrawer.GetPropertyHeight(routeProperty, new GUIContent($"Route"));

            float totalHeight =
                    Constants.MarginVertical +
                    EditorGUIUtility.singleLineHeight + // Header
                    EditorGUIUtility.standardVerticalSpacing +
                    Constants.MarginVertical +
                    routeHeight + // Route content
                    Constants.MarginVertical;

            return totalHeight;
        }
    }
}