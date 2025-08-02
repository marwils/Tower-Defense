using UnityEditor;

using UnityEngine;

namespace MarwilsTD.LevelSystem
{
    [CustomPropertyDrawer(typeof(SpawnPlan))]
    public class SpawnPlanPropertyDrawer : PropertyDrawer
    {
        private SpawnPlanSequencePropertyDrawer _sequenceDrawer = new SpawnPlanSequencePropertyDrawer();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.objectReferenceValue == null)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            var spawnPlan = property.objectReferenceValue as SpawnPlan;
            if (spawnPlan == null)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            var currentY = position.y;
            var headerRect = new Rect(position.x, currentY, position.width, EditorGUIUtility.singleLineHeight);
            var headerStyle = new GUIStyle(EditorStyles.boldLabel);
            headerStyle.fontSize = 14;
            EditorGUI.LabelField(headerRect, label.text, headerStyle);
            currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            DrawSpawnPlanContent(position, spawnPlan, ref currentY);

            EditorGUI.HelpBox(
                new Rect(position.x, currentY, position.width, EditorGUIUtility.singleLineHeight * 2),
                "All sequences in this Spawn Plan start simultaneously. The Spawn Plan is only considered finished when all sequences are complete.",
                MessageType.Info
            );
            currentY += EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing;
        }

        public void DrawSpawnPlanContent(Rect position, SpawnPlan spawnPlan, ref float currentY)
        {
            if (spawnPlan == null) return;

            var spawnPlanSO = new SerializedObject(spawnPlan);
            spawnPlanSO.Update();

            var sequencesProp = spawnPlanSO.FindProperty("_spawnPlanSequences");
            if (sequencesProp != null)
            {
                for (int i = 0; i < sequencesProp.arraySize; i++)
                {
                    var sequenceProp = sequencesProp.GetArrayElementAtIndex(i);
                    var sequence = sequenceProp.objectReferenceValue as SpawnPlanSequence;

                    string sequenceLabel = sequence != null ? $"Sequence {i + 1}" : $"Sequence {i + 1} (Missing)";
                    var sequenceGuiLabel = new GUIContent(sequenceLabel);

                    var sequenceHeight = sequenceProp.isExpanded ? _sequenceDrawer.GetPropertyHeight(sequenceProp, sequenceGuiLabel) : 0;
                    var totalHeight = sequenceHeight + Constants.MarginVertical * 2 + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                    var boxRect = new Rect(position.x, currentY, position.width, totalHeight);
                    GUI.Box(boxRect, "", EditorStyles.helpBox);

                    var foldoutRect = new Rect(position.x + Constants.MarginVertical, currentY + Constants.MarginVertical,
                                             15, EditorGUIUtility.singleLineHeight);
                    sequenceProp.isExpanded = EditorGUI.Foldout(foldoutRect, sequenceProp.isExpanded, GUIContent.none, true);

                    var headerWaveRect = new Rect(position.x + Constants.MarginVertical, currentY + Constants.MarginVertical,
                                                 position.width - 60 - Constants.MarginVertical, EditorGUIUtility.singleLineHeight);
                    var headerStyle = new GUIStyle(EditorStyles.boldLabel);
                    headerStyle.fontSize = 14;
                    EditorGUI.LabelField(headerWaveRect, sequenceLabel, headerStyle);

                    var deleteMinusButtonRect = new Rect(position.x + position.width - 20 - Constants.MarginVertical,
                                                        currentY + Constants.MarginVertical, 20, EditorGUIUtility.singleLineHeight);
                    if (GUI.Button(deleteMinusButtonRect, "-"))
                    {
                        if (sequence != null)
                        {
                            var routeName = sequence.name;
                            bool isFactorySequence = routeName.StartsWith("SpawnPlanSequence_") && routeName.Length >= 35;
                            if (isFactorySequence)
                            {
                                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(sequence));
                            }
                        }
                        sequencesProp.DeleteArrayElementAtIndex(i);
                        spawnPlanSO.ApplyModifiedProperties();
                        EditorUtility.SetDirty(spawnPlan);
                        break;
                    }

                    var addPlusButtonRect = new Rect(position.x + position.width - 45, currentY + Constants.MarginVertical,
                                                    20, EditorGUIUtility.singleLineHeight);
                    if (GUI.Button(addPlusButtonRect, "+"))
                    {
                        CreateNewSequence(sequencesProp, spawnPlan);
                    }

                    if (sequenceProp.isExpanded)
                    {
                        var contentY = currentY + Constants.MarginVertical + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                        var contentHeight = sequenceHeight;
                        var contentRect = new Rect(position.x + Constants.MarginHorizontal, contentY,
                                                  position.width - Constants.MarginHorizontal * 2, contentHeight);

                        _sequenceDrawer.OnGUI(contentRect, sequenceProp, sequenceGuiLabel);
                    }

                    currentY += totalHeight + Constants.MarginVertical;
                }

                var addButtonRect = new Rect(position.x, currentY, position.width, EditorGUIUtility.singleLineHeight);
                if (GUI.Button(addButtonRect, "Add Sequence to Spawn Plan"))
                {
                    CreateNewSequence(sequencesProp, spawnPlan);
                }

                currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }

            if (spawnPlanSO.hasModifiedProperties)
            {
                spawnPlanSO.ApplyModifiedProperties();
                EditorUtility.SetDirty(spawnPlan);
            }
        }

        private void CreateNewSequence(SerializedProperty sequencesProp, SpawnPlan spawnPlan)
        {
            var sequence = LevelAssetFactory.CreateSpawnPlanSequence(spawnPlan);
            sequencesProp.arraySize++;
            var newSequenceProp = sequencesProp.GetArrayElementAtIndex(sequencesProp.arraySize - 1);
            newSequenceProp.objectReferenceValue = sequence;
            sequencesProp.serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(spawnPlan);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.objectReferenceValue == null)
                return EditorGUIUtility.singleLineHeight;

            var spawnPlan = property.objectReferenceValue as SpawnPlan;
            if (spawnPlan == null)
                return EditorGUIUtility.singleLineHeight;

            var spawnPlanSO = new SerializedObject(spawnPlan);
            var sequencesProp = spawnPlanSO.FindProperty("_spawnPlanSequences");

            float height = 0;

            height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Header

            // Sequences
            if (sequencesProp != null)
            {
                for (int i = 0; i < sequencesProp.arraySize; i++)
                {
                    var sequenceProp = sequencesProp.GetArrayElementAtIndex(i);
                    var sequenceHeight = sequenceProp.isExpanded ? _sequenceDrawer.GetPropertyHeight(sequenceProp, new GUIContent($"Sequence {i + 1}")) : 0;
                    // Header + Content + Margins + Spacing
                    height += sequenceHeight + Constants.MarginVertical * 2 + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + Constants.MarginVertical;
                }

                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Add Button
            }

            height += EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing; // Info Box

            return height;
        }
    }
}