using UnityEditor;
using UnityEngine;

namespace MarwilsTD.LevelSystem
{
    public static class GUIHelper
    {
        public static void DrawWarning(string text, Rect position, ref float currentY)
        {
            var noElementsRect = new Rect(position.x, currentY, position.width, EditorGUIUtility.singleLineHeight);
            var oldColor = GUI.color;
            GUI.color = new Color(1f, 1f, 0.8f);
            GUI.Box(noElementsRect, "", EditorStyles.helpBox);
            GUI.color = oldColor;

            var labelStyle = new GUIStyle(EditorStyles.label);
            labelStyle.fontStyle = FontStyle.Italic;
            EditorGUI.LabelField(noElementsRect, text, labelStyle);
            currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }
    }
}
