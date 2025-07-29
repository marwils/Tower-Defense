using UnityEditor;

using UnityEngine;

namespace LevelSystem
{
    [CustomPropertyDrawer(typeof(WaveRoutes))]
    public class WaveRoutesPropertyDrawer : PropertyDrawer
    {
        private WaveRoutePropertyDrawer _waveRouteDrawer;

        private WaveRoutePropertyDrawer WaveRouteDrawer
        {
            get
            {
                if (_waveRouteDrawer == null)
                    _waveRouteDrawer = new WaveRoutePropertyDrawer();
                return _waveRouteDrawer;
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var waveRoutes = property.objectReferenceValue as WaveRoutes;
            if (waveRoutes == null)
            {
                EditorGUI.PropertyField(position, property, label);
                EditorGUI.EndProperty();
                return;
            }

            var currentY = position.y;
            var headerRect = new Rect(position.x, currentY, position.width, EditorGUIUtility.singleLineHeight);
            var headerStyle = new GUIStyle(EditorStyles.boldLabel);
            headerStyle.fontSize = 14;
            EditorGUI.LabelField(headerRect, label.text, headerStyle);
            currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            var indentedRect = new Rect(position.x, currentY, position.width, position.height - (currentY - position.y));

            var waveRoutesSO = new SerializedObject(waveRoutes);
            waveRoutesSO.Update();

            currentY = indentedRect.y;
            var routesProp = waveRoutesSO.FindProperty("_routes");
            if (routesProp != null)
            {
                for (int i = 0; i < routesProp.arraySize; i++)
                {
                    var routeProp = routesProp.GetArrayElementAtIndex(i);
                    var route = routeProp.objectReferenceValue as WaveRoute;

                    string routeLabel = route != null ? $"Route {i + 1}" : $"Route {i + 1} (Missing)";
                    var routeGuiLabel = new GUIContent(routeLabel);
                    var routeHeight = WaveRouteDrawer.GetPropertyHeight(routeProp, routeGuiLabel);
                    const float margin = 8f;
                    var totalRouteHeight = routeHeight + margin * 2;
                    var routeBoxRect = new Rect(indentedRect.x, currentY, indentedRect.width, totalRouteHeight);
                    GUI.Box(routeBoxRect, "", EditorStyles.helpBox);
                    var routeHeaderRect = new Rect(indentedRect.x + margin, currentY + margin, indentedRect.width - margin * 2 - 25, EditorGUIUtility.singleLineHeight);
                    var deleteRect = new Rect(indentedRect.x + indentedRect.width - 25, currentY + margin, 20, EditorGUIUtility.singleLineHeight);

                    var routeHeaderStyle = new GUIStyle(EditorStyles.boldLabel);
                    routeHeaderStyle.fontSize = 12;
                    EditorGUI.LabelField(routeHeaderRect, routeLabel, routeHeaderStyle);
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
                    var routeContentY = currentY + margin + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    var routeContentHeight = routeHeight - EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing;
                    var routeContentRect = new Rect(indentedRect.x + margin, routeContentY, indentedRect.width - margin * 2, routeContentHeight);
                    DrawWaveRoute(routeContentRect, routeProp, routeGuiLabel);

                    currentY += totalRouteHeight + EditorGUIUtility.standardVerticalSpacing;
                }
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

        private void DrawWaveRoute(Rect position, SerializedProperty routeProp, GUIContent label)
        {
            var route = routeProp.objectReferenceValue as WaveRoute;
            if (route == null) return;

            var wasExpanded = routeProp.isExpanded;
            routeProp.isExpanded = true;

            var waveRouteRect = new Rect(
                position.x,
                position.y - EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing,
                position.width,
                position.height + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing
            );

            WaveRouteDrawer.OnGUI(waveRouteRect, routeProp, label);

            routeProp.isExpanded = wasExpanded;
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

            float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            if (routesProp != null)
            {
                const float margin = 8f;
                for (int i = 0; i < routesProp.arraySize; i++)
                {
                    var routeProp = routesProp.GetArrayElementAtIndex(i);
                    string routeLabel = $"Route {i + 1}";
                    var wasExpanded = routeProp.isExpanded;
                    routeProp.isExpanded = true;

                    var routeHeight = WaveRouteDrawer.GetPropertyHeight(routeProp, new GUIContent(routeLabel));
                    var totalRouteHeight = routeHeight + margin * 2;

                    routeProp.isExpanded = wasExpanded;

                    height += totalRouteHeight + EditorGUIUtility.standardVerticalSpacing;
                }

                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }

            return height;
        }
    }
}