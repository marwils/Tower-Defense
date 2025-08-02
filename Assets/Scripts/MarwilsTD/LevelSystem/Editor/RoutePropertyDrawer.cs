using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace MarwilsTD.LevelSystem
{
    [CustomPropertyDrawer(typeof(Route))]
    public class RoutePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var route = property.objectReferenceValue as Route;
            if (route == null)
            {
                EditorGUI.PropertyField(position, property, label);
                EditorGUI.EndProperty();
                return;
            }

            DrawRouteContent(position, route);

            EditorGUI.EndProperty();
        }

        public void DrawRouteContent(Rect position, Route route)
        {
            if (route == null) return;

            var currentY = position.y;

            var routeSO = new SerializedObject(route);
            routeSO.Update();

            var titleProp = routeSO.FindProperty("_title");
            if (titleProp != null)
            {
                var titleRect = new Rect(position.x, currentY, position.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(titleRect, titleProp, new GUIContent("Title"));
                currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }

            var spawnPointIdProp = routeSO.FindProperty("_spawnPointId");
            if (spawnPointIdProp != null)
            {
                var spawnPointRect = new Rect(position.x, currentY, position.width, EditorGUIUtility.singleLineHeight);
                DrawSpawnPointDropdown(spawnPointRect, spawnPointIdProp);
                currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }

            var targetPointIdProp = routeSO.FindProperty("_targetPointId");
            if (targetPointIdProp != null)
            {
                var targetPointRect = new Rect(position.x, currentY, position.width, EditorGUIUtility.singleLineHeight);
                DrawTargetPointDropdown(targetPointRect, targetPointIdProp);
                currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }

            DrawValidationStatus(position, route, ref currentY);

            if (routeSO.hasModifiedProperties)
            {
                routeSO.ApplyModifiedProperties();
                EditorUtility.SetDirty(route);
            }
        }

        private void DrawSpawnPointDropdown(Rect position, SerializedProperty spawnPointIdProp)
        {
            var availableIds = GetAvailableSpawnPointNames();
            var currentId = spawnPointIdProp.stringValue;
            var currentIndex = System.Array.IndexOf(availableIds, currentId);

            var displayOptions = new string[availableIds.Length + 1];
            displayOptions[0] = "None";
            System.Array.Copy(availableIds, 0, displayOptions, 1, availableIds.Length);

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
                UpdateLevelSceneContext(spawnPointIdProp);
            }
            if (!string.IsNullOrEmpty(currentId) && currentIndex < 0)
            {
                var warningRect = new Rect(position.x + position.width - 20, position.y, 20, position.height);
                var oldColor = GUI.color;
                GUI.color = Color.yellow;
                EditorGUI.LabelField(warningRect, "⚠", EditorStyles.boldLabel);
                GUI.color = oldColor;
            }
        }

        private void DrawTargetPointDropdown(Rect position, SerializedProperty targetPointIdProp)
        {
            var availableIds = GetAvailableTargetPointNames();
            var currentId = targetPointIdProp.stringValue;
            var currentIndex = System.Array.IndexOf(availableIds, currentId);

            var displayOptions = new string[availableIds.Length + 1];
            displayOptions[0] = "None";
            System.Array.Copy(availableIds, 0, displayOptions, 1, availableIds.Length);

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
                UpdateLevelSceneContext(targetPointIdProp);
            }
            if (!string.IsNullOrEmpty(currentId) && currentIndex < 0)
            {
                var warningRect = new Rect(position.x + position.width - 20, position.y, 20, position.height);
                var oldColor = GUI.color;
                GUI.color = Color.yellow;
                EditorGUI.LabelField(warningRect, "⚠", EditorStyles.boldLabel);
                GUI.color = oldColor;
            }
        }

        private void DrawValidationStatus(Rect position, Route route, ref float currentY)
        {
            bool isRouteValid = route != null;
            bool isSpawnPointValid = !string.IsNullOrEmpty(route?.SpawnPointId);
            bool isTargetPointValid = !string.IsNullOrEmpty(route?.TargetPointId);

            List<string> errors = new();
            if (!isRouteValid) errors.Add("✗ Route is not assigned");
            if (!isSpawnPointValid) errors.Add("✗ Spawn Point is not assigned");
            if (!isTargetPointValid) errors.Add("✗ Target Point is not assigned");

            float height = EditorStyles.boldLabel.CalcHeight(new GUIContent(string.Join("\n", errors)), position.width) + EditorGUIUtility.standardVerticalSpacing;

            var statusRect = new Rect(position.x, currentY, position.width, height);

            if (!isRouteValid || !isSpawnPointValid || !isTargetPointValid)
            {
                var oldColor = GUI.color;
                GUI.color = new Color(0.8f, 0.2f, 0.2f);
                GUI.Box(statusRect, "", EditorStyles.helpBox);
                GUI.color = oldColor;

                EditorGUI.LabelField(statusRect, string.Join("\n", errors), EditorStyles.boldLabel);
            }

            currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
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

        private void UpdateLevelSceneContext(SerializedProperty anyPropertyFromRoute)
        {
            var currentObject = anyPropertyFromRoute.serializedObject.targetObject;

            foreach (var level in EditorHelper.FindLevelsContaining(currentObject))
            {
                level.UpdateSceneContext();
                EditorUtility.SetDirty(level);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var route = property.objectReferenceValue as Route;
            if (route == null) return EditorGUIUtility.singleLineHeight;

            bool isRouteValid = route != null;
            bool isSpawnPointValid = !string.IsNullOrEmpty(route?.SpawnPointId);
            bool isTargetPointValid = !string.IsNullOrEmpty(route?.TargetPointId);

            float height = 0;

            height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Title
            height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Spawn Point Dropdown
            height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Target Point Dropdown

            // Validation Status
            List<string> errors = new();
            if (!isRouteValid) errors.Add("✗ Route is not assigned");
            if (!isSpawnPointValid) errors.Add("✗ Spawn Point is not assigned");
            if (!isTargetPointValid) errors.Add("✗ Target Point is not assigned");

            if (errors.Count > 0)
            {
                height += EditorStyles.boldLabel.CalcHeight(new GUIContent(string.Join("\n", errors)), 0);
            }

            return height;
        }
    }
}