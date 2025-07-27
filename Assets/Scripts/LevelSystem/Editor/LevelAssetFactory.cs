using System;
using System.IO;

using UnityEditor;

using UnityEngine;

namespace LevelSystem
{
    public static class LevelAssetFactory
    {
        public static Wave CreateWave(UnityEngine.Object context)
        {
            var wave = ScriptableObject.CreateInstance<Wave>();
            wave.name = $"Wave_{DateTime.Now:yyyyMMdd_HHmmss}";
            SaveAsset(wave, context, "Waves");
            return wave;
        }

        public static AbstractWaveElement CreateWaveElement(Type elementType, UnityEngine.Object context)
        {
            var element = ScriptableObject.CreateInstance(elementType) as AbstractWaveElement;
            element.name = $"{elementType.Name}_{DateTime.Now:yyyyMMdd_HHmmss}";
            SaveAsset(element, context, "Elements");
            return element;
        }

        public static AbstractSequenceElement CreateSequenceElement(Type elementType, UnityEngine.Object context)
        {
            var element = ScriptableObject.CreateInstance(elementType) as AbstractSequenceElement;
            element.name = $"{elementType.Name}_{DateTime.Now:yyyyMMdd_HHmmss}";
            SaveAsset(element, context, "Sequences");
            return element;
        }

        public static WaveRoute CreateWaveRoute(UnityEngine.Object context)
        {
            var route = ScriptableObject.CreateInstance<WaveRoute>();
            route.name = $"WaveRoute_{DateTime.Now:yyyyMMdd_HHmmss}";
            SaveAsset(route, context, "Routes");
            return route;
        }

        private static void SaveAsset(ScriptableObject asset, UnityEngine.Object context, string folderName)
        {
            // Get context path
            string contextPath = AssetDatabase.GetAssetPath(context);
            string contextDir = Path.GetDirectoryName(contextPath);
            string contextName = Path.GetFileNameWithoutExtension(contextPath);

            // Create level-specific folder
            string levelFolder = Path.Combine(contextDir, contextName);
            if (!AssetDatabase.IsValidFolder(levelFolder))
            {
                string guid = AssetDatabase.CreateFolder(contextDir, contextName);
                levelFolder = AssetDatabase.GUIDToAssetPath(guid);
            }

            // Create type-specific subfolder
            string typeFolder = Path.Combine(levelFolder, folderName);
            if (!AssetDatabase.IsValidFolder(typeFolder))
            {
                string guid = AssetDatabase.CreateFolder(levelFolder, folderName);
                typeFolder = AssetDatabase.GUIDToAssetPath(guid);
            }

            // Save asset
            string assetPath = Path.Combine(typeFolder, $"{asset.name}.asset");
            assetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);

            AssetDatabase.CreateAsset(asset, assetPath);
            AssetDatabase.SaveAssets();
        }
    }
}