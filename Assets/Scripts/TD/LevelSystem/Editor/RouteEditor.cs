using UnityEditor;

using UnityEngine;

namespace LevelSystem
{
    [CustomEditor(typeof(Route))]
    public class RouteEditor : Editor
    {
        private RoutePropertyDrawer _routeDrawer;

        private void OnEnable()
        {
            _routeDrawer = new RoutePropertyDrawer();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space();
            var headerStyle = new GUIStyle(EditorStyles.boldLabel);
            headerStyle.fontSize = 16;
            EditorGUILayout.LabelField("Route Configuration", headerStyle);
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUI.indentLevel++;
            EditorGUILayout.Foldout(true, "Route Properties", true);
            DrawRouteUsingPropertyDrawer();
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();

            if (EditorApplication.isPlaying)
            {
                DrawDebugInfo();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawRouteUsingPropertyDrawer()
        {
            var route = target as Route;
            var wrapper = CreateWrapper(route);
            var wrapperSO = new SerializedObject(wrapper);
            var routeProperty = wrapperSO.FindProperty("route");

            if (routeProperty != null)
            {
                var height = _routeDrawer.GetPropertyHeight(routeProperty, new GUIContent("Route"));
                var rect = GUILayoutUtility.GetRect(0, height, GUILayout.ExpandWidth(true));

                EditorGUI.BeginChangeCheck();
                _routeDrawer.OnGUI(rect, routeProperty, new GUIContent("Route"));

                if (EditorGUI.EndChangeCheck())
                {
                    wrapperSO.ApplyModifiedProperties();
                    EditorUtility.SetDirty(route);
                }
            }

            if (wrapper != null)
            {
                DestroyImmediate(wrapper);
            }
        }

        private RouteWrapper CreateWrapper(Route route)
        {
            var wrapper = CreateInstance<RouteWrapper>();
            wrapper.route = route;
            return wrapper;
        }

        private void DrawDebugInfo()
        {
            var route = target as Route;

            EditorGUILayout.LabelField("Debug Information", EditorStyles.boldLabel);

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Spawn Transform", route?.SpawnTransform?.name ?? "null");
            EditorGUILayout.TextField("Target Transform", route?.TargetTransform?.name ?? "null");
            EditorGUILayout.Toggle("Is Valid", route?.IsValid ?? false);
            EditorGUI.EndDisabledGroup();

            if (GUILayout.Button("Refresh Scene Points"))
            {
                Repaint();
            }

            if (route.IsValid && GUILayout.Button("Select Spawn Point"))
            {
                Selection.activeGameObject = route.SpawnTransform.gameObject;
                EditorGUIUtility.PingObject(route.SpawnTransform.gameObject);
            }

            if (route.IsValid && GUILayout.Button("Select Target Point"))
            {
                Selection.activeGameObject = route.TargetTransform.gameObject;
                EditorGUIUtility.PingObject(route.TargetTransform.gameObject);
            }
        }
    }

    [System.Serializable]
    internal class RouteWrapper : ScriptableObject
    {
        public Route route;
    }
}