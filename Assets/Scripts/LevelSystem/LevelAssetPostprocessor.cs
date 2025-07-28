using UnityEditor;
using UnityEngine;

namespace LevelSystem
{
    public class LevelAssetPostprocessor : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (string assetPath in importedAssets)
            {
                if (assetPath.EndsWith(".asset"))
                {
                    var level = AssetDatabase.LoadAssetAtPath<Level>(assetPath);
                    if (level != null && level.IsSerializationBlocked)
                    {
                        Debug.LogWarning($"Level '{level.name}' has serialization blocked due to scene context mismatch. " +
                                       $"Please update scene context before making changes.");
                    }
                }
            }
        }
    }
}