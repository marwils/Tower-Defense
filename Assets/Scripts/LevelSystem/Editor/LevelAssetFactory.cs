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

        public static WaveElement CreateWaveElement(Type elementType, UnityEngine.Object context)
        {
            var element = ScriptableObject.CreateInstance(elementType) as WaveElement;
            element.name = $"{elementType.Name}_{DateTime.Now:yyyyMMdd_HHmmss}";
            SaveAsset(element, context, null);
            return element;
        }

        public static SequenceElement CreateSequenceElement(Type elementType, UnityEngine.Object context)
        {
            var element = ScriptableObject.CreateInstance(elementType) as SequenceElement;
            element.name = $"{elementType.Name}_{DateTime.Now:yyyyMMdd_HHmmss}";
            SaveAsset(element, context, "Sequences");
            return element;
        }

        public static Route CreateRoute(UnityEngine.Object context)
        {
            var route = ScriptableObject.CreateInstance<Route>();
            route.name = $"Route_{DateTime.Now:yyyyMMdd_HHmmss}";
            SaveAsset(route, context, "Routes");
            return route;
        }

        public static SpawnPlan CreateSpawnPlanSequence(UnityEngine.Object context)
        {
            var spawnPlan = ScriptableObject.CreateInstance<SpawnPlan>();
            spawnPlan.name = $"SpawnPlan_{DateTime.Now:yyyyMMdd_HHmmss}";
            SaveAsset(spawnPlan, context, "SpawnPlans");
            return spawnPlan;
        }

        public static SpawnPlanSequence CreateSpawnPlanSequence(SpawnPlan spawnPlan)
        {
            var sequence = ScriptableObject.CreateInstance<SpawnPlanSequence>();
            sequence.name = $"SpawnPlanSequence_{DateTime.Now:yyyyMMdd_HHmmss}";

            var assetPath = AssetDatabase.GetAssetPath(spawnPlan);
            AssetDatabase.AddObjectToAsset(sequence, assetPath);
            AssetDatabase.SaveAssets();

            return sequence;
        }

        public static SequenceElement CreateSequenceElement(Type elementType, object parent)
        {
            var element = ScriptableObject.CreateInstance(elementType) as SequenceElement;
            element.name = $"{elementType.Name}_{DateTime.Now:yyyyMMdd_HHmmss}";

            string assetPath;
            if (parent is SpawnPlanSequence sequence)
            {
                assetPath = AssetDatabase.GetAssetPath(sequence);
            }
            else
            {
                throw new ArgumentException("Parent must be SpawnPlanSequence or WaveRoute");
            }

            AssetDatabase.AddObjectToAsset(element, assetPath);
            AssetDatabase.SaveAssets();

            return element;
        }

        private static void SaveAsset(ScriptableObject asset, UnityEngine.Object context, string folderName)
        {
            string contextPath = AssetDatabase.GetAssetPath(context);
            string contextDir = Path.GetDirectoryName(contextPath);
            string contextName = Path.GetFileNameWithoutExtension(contextPath);
            string levelFolder = Path.Combine(contextDir, contextName);
            if (!AssetDatabase.IsValidFolder(levelFolder))
            {
                string guid = AssetDatabase.CreateFolder(contextDir, contextName);
                levelFolder = AssetDatabase.GUIDToAssetPath(guid);
            }
            string assetPathFolder;
            if (folderName == null)
            {
                assetPathFolder = levelFolder;
            }
            else
            {
                string typeFolder = Path.Combine(levelFolder, folderName);
                if (!AssetDatabase.IsValidFolder(typeFolder))
                {
                    string guid = AssetDatabase.CreateFolder(levelFolder, folderName);
                    typeFolder = AssetDatabase.GUIDToAssetPath(guid);
                }
                assetPathFolder = typeFolder;
            }
            string assetPath = Path.Combine(assetPathFolder, $"{asset.name}.asset");
            assetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);

            AssetDatabase.CreateAsset(asset, assetPath);
            AssetDatabase.SaveAssets();
        }
    }
}