using System.Linq;

using UnityEditor;

using UnityEngine;

namespace MarwilsTD.LevelSystem
{
    public static class RouteEditorHelper
    {
        public static void DrawRouteFields(SerializedObject serializedObject, Rect? containerRect = null)
        {
            var titleProp = serializedObject.FindProperty("_title");
            var spawnPointIdProp = serializedObject.FindProperty("_spawnPointId");
            var targetPointIdProp = serializedObject.FindProperty("_targetPointId");

            if (containerRect.HasValue)
            {
                var rect = containerRect.Value;
                var currentY = rect.y;
                var lineHeight = EditorGUIUtility.singleLineHeight;
                var spacing = EditorGUIUtility.standardVerticalSpacing;
                if (titleProp != null)
                {
                    var titleRect = new Rect(rect.x, currentY, rect.width, lineHeight);
                    EditorGUI.PropertyField(titleRect, titleProp, new GUIContent("Title"));
                    currentY += lineHeight + spacing;
                }
                if (spawnPointIdProp != null)
                {
                    var spawnRect = new Rect(rect.x, currentY, rect.width, lineHeight);
                    DrawSpawnPointDropdown(spawnRect, spawnPointIdProp);
                    currentY += lineHeight + spacing;
                }
                if (targetPointIdProp != null)
                {
                    var targetRect = new Rect(rect.x, currentY, rect.width, lineHeight);
                    DrawTargetPointDropdown(targetRect, targetPointIdProp);
                }
            }
            else
            {
                if (titleProp != null)
                {
                    EditorGUILayout.PropertyField(titleProp, new GUIContent("Title"));
                }

                if (spawnPointIdProp != null)
                {
                    DrawSpawnPointDropdownLayout(spawnPointIdProp);
                }

                if (targetPointIdProp != null)
                {
                    DrawTargetPointDropdownLayout(targetPointIdProp);
                }
            }
        }

        public static void DrawSpawnPointDropdown(Rect position, SerializedProperty spawnPointIdProp)
        {
            var spawnPoints = Object.FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);
            var spawnPointNames = spawnPoints.Where(sp => !string.IsNullOrEmpty(sp.gameObject.name))
                                             .Select(sp => sp.gameObject.name)
                                             .Distinct()
                                             .OrderBy(name => name)
                                             .ToArray();

            var currentId = spawnPointIdProp.stringValue;
            var currentIndex = System.Array.IndexOf(spawnPointNames, currentId);

            var displayOptions = new string[spawnPointNames.Length + 1];
            displayOptions[0] = "None";
            System.Array.Copy(spawnPointNames, 0, displayOptions, 1, spawnPointNames.Length);

            var displayIndex = currentIndex >= 0 ? currentIndex + 1 : 0;

            EditorGUI.BeginChangeCheck();
            displayIndex = EditorGUI.Popup(position, "Spawn Point", displayIndex, displayOptions);
            if (EditorGUI.EndChangeCheck())
            {
                if (displayIndex == 0)
                {
                    spawnPointIdProp.stringValue = "";
                }
                else if (displayIndex > 0 && displayIndex <= spawnPointNames.Length)
                {
                    spawnPointIdProp.stringValue = spawnPointNames[displayIndex - 1];
                }
            }
        }

        public static void DrawSpawnPointDropdownLayout(SerializedProperty spawnPointIdProp)
        {
            var spawnPoints = Object.FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);
            var spawnPointNames = spawnPoints.Where(sp => !string.IsNullOrEmpty(sp.gameObject.name))
                                             .Select(sp => sp.gameObject.name)
                                             .Distinct()
                                             .OrderBy(name => name)
                                             .ToArray();

            var currentId = spawnPointIdProp.stringValue;
            var currentIndex = System.Array.IndexOf(spawnPointNames, currentId);

            var displayOptions = new string[spawnPointNames.Length + 1];
            displayOptions[0] = "None";
            System.Array.Copy(spawnPointNames, 0, displayOptions, 1, spawnPointNames.Length);

            var displayIndex = currentIndex >= 0 ? currentIndex + 1 : 0;

            EditorGUI.BeginChangeCheck();
            displayIndex = EditorGUILayout.Popup("Spawn Point", displayIndex, displayOptions);
            if (EditorGUI.EndChangeCheck())
            {
                if (displayIndex == 0)
                {
                    spawnPointIdProp.stringValue = "";
                }
                else if (displayIndex > 0 && displayIndex <= spawnPointNames.Length)
                {
                    spawnPointIdProp.stringValue = spawnPointNames[displayIndex - 1];
                }
            }
            if (!string.IsNullOrEmpty(currentId) && currentIndex < 0)
            {
                EditorGUILayout.HelpBox($"Spawn Point '{currentId}' not found in scene!", MessageType.Warning);
            }
        }

        public static void DrawTargetPointDropdown(Rect position, SerializedProperty targetPointIdProp)
        {
            var targetPoints = Object.FindObjectsByType<TargetPoint>(FindObjectsSortMode.None);
            var targetPointNames = targetPoints.Where(tp => !string.IsNullOrEmpty(tp.gameObject.name))
                                               .Select(tp => tp.gameObject.name)
                                               .Distinct()
                                               .OrderBy(name => name)
                                               .ToArray();

            var currentId = targetPointIdProp.stringValue;
            var currentIndex = System.Array.IndexOf(targetPointNames, currentId);

            var displayOptions = new string[targetPointNames.Length + 1];
            displayOptions[0] = "None";
            System.Array.Copy(targetPointNames, 0, displayOptions, 1, targetPointNames.Length);

            var displayIndex = currentIndex >= 0 ? currentIndex + 1 : 0;

            EditorGUI.BeginChangeCheck();
            displayIndex = EditorGUI.Popup(position, "Target Point", displayIndex, displayOptions);
            if (EditorGUI.EndChangeCheck())
            {
                if (displayIndex == 0)
                {
                    targetPointIdProp.stringValue = "";
                }
                else if (displayIndex > 0 && displayIndex <= targetPointNames.Length)
                {
                    targetPointIdProp.stringValue = targetPointNames[displayIndex - 1];
                }
            }
        }

        public static void DrawTargetPointDropdownLayout(SerializedProperty targetPointIdProp)
        {
            var targetPoints = Object.FindObjectsByType<TargetPoint>(FindObjectsSortMode.None);
            var targetPointNames = targetPoints.Where(tp => !string.IsNullOrEmpty(tp.gameObject.name))
                                               .Select(tp => tp.gameObject.name)
                                               .Distinct()
                                               .OrderBy(name => name)
                                               .ToArray();

            var currentId = targetPointIdProp.stringValue;
            var currentIndex = System.Array.IndexOf(targetPointNames, currentId);

            var displayOptions = new string[targetPointNames.Length + 1];
            displayOptions[0] = "None";
            System.Array.Copy(targetPointNames, 0, displayOptions, 1, targetPointNames.Length);

            var displayIndex = currentIndex >= 0 ? currentIndex + 1 : 0;

            EditorGUI.BeginChangeCheck();
            displayIndex = EditorGUILayout.Popup("Target Point", displayIndex, displayOptions);
            if (EditorGUI.EndChangeCheck())
            {
                if (displayIndex == 0)
                {
                    targetPointIdProp.stringValue = "";
                }
                else if (displayIndex > 0 && displayIndex <= targetPointNames.Length)
                {
                    targetPointIdProp.stringValue = targetPointNames[displayIndex - 1];
                }
            }
            if (!string.IsNullOrEmpty(currentId) && currentIndex < 0)
            {
                EditorGUILayout.HelpBox($"Target Point '{currentId}' not found in scene!", MessageType.Warning);
            }
        }
    }
}