using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;

namespace MarwilsTD.LevelSystem
{
    public static class EditorHelper
    {
        /// <summary>
        /// Recursively checks if an object is contained within a hierarchy of ScriptableObjects.
        /// </summary>
        /// <param name="root">The root object to search from</param>
        /// <param name="searchTarget">The object to search for</param>
        /// <param name="maxDepth">Maximum search depth (default: 10)</param>
        /// <returns>True if the object is found</returns>
        public static bool ContainsObject(UnityEngine.Object root, UnityEngine.Object searchTarget, int maxDepth = 10)
        {
            if (root == null || searchTarget == null) return false;
            if (root == searchTarget) return true;
            if (maxDepth <= 0) return false;

            return ContainsObjectRecursive(root, searchTarget, maxDepth, new HashSet<UnityEngine.Object>());
        }

        private static bool ContainsObjectRecursive(UnityEngine.Object current, UnityEngine.Object searchTarget, int remainingDepth, HashSet<UnityEngine.Object> visited)
        {
            if (current == null || remainingDepth <= 0) return false;
            if (current == searchTarget) return true;
            if (!visited.Add(current)) return false;

            var currentType = current.GetType();

            foreach (var field in GetAllFields(currentType))
            {
                try
                {
                    var value = field.GetValue(current);
                    if (value == null) continue;

                    if (value is UnityEngine.Object unityObj)
                    {
                        if (ContainsObjectRecursive(unityObj, searchTarget, remainingDepth - 1, visited))
                            return true;
                    }

                    else if (value is IEnumerable enumerable && !(value is string))
                    {
                        foreach (var item in enumerable)
                        {
                            if (item is UnityEngine.Object unityItem)
                            {
                                if (ContainsObjectRecursive(unityItem, searchTarget, remainingDepth - 1, visited))
                                    return true;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return false;
        }

        private static IEnumerable<FieldInfo> GetAllFields(Type type)
        {
            var fields = new List<FieldInfo>();
            var currentType = type;

            while (currentType != null && currentType != typeof(UnityEngine.Object))
            {
                fields.AddRange(currentType.GetFields(
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.DeclaredOnly
                ).Where(f => !f.IsStatic));

                currentType = currentType.BaseType;
            }

            return fields;
        }

        /// <summary>
        /// Finds all Level assets that contain a specific object.
        /// </summary>
        public static IEnumerable<Level> FindLevelsContaining(UnityEngine.Object obj)
        {
#if UNITY_EDITOR
            var levelGuids = UnityEditor.AssetDatabase.FindAssets("t:Level");
            foreach (var guid in levelGuids)
            {
                var levelPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var level = UnityEditor.AssetDatabase.LoadAssetAtPath<Level>(levelPath);

                if (level != null && ContainsObject(level, obj))
                {
                    yield return level;
                }
            }
#else
            yield break;
#endif
        }

        /// <summary>
        /// Finds all assets of a given type that contain a specific object.
        /// </summary>
        public static IEnumerable<T> FindAssetsContaining<T>(UnityEngine.Object obj) where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            var assetGuids = UnityEditor.AssetDatabase.FindAssets($"t:{typeof(T).Name}");
            foreach (var guid in assetGuids)
            {
                var assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);

                if (asset != null && ContainsObject(asset, obj))
                {
                    yield return asset;
                }
            }
#else
            yield break;
#endif
        }

        /// <summary>
        /// Debug method: Displays the hierarchy of a ScriptableObject.
        /// </summary>
        public static void LogObjectHierarchy(UnityEngine.Object obj, int maxDepth = 3)
        {
            LogObjectHierarchyRecursive(obj, 0, maxDepth, new HashSet<UnityEngine.Object>());
        }

        private static void LogObjectHierarchyRecursive(UnityEngine.Object obj, int currentDepth, int maxDepth, HashSet<UnityEngine.Object> visited)
        {
            if (obj == null || currentDepth > maxDepth || !visited.Add(obj)) return;

            var indent = new string(' ', currentDepth * 2);
            Debug.Log($"{indent}{obj.GetType().Name}: {obj.name}");

            var objType = obj.GetType();
            foreach (var field in GetAllFields(objType))
            {
                try
                {
                    var value = field.GetValue(obj);
                    if (value == null) continue;

                    if (value is UnityEngine.Object unityObj)
                    {
                        LogObjectHierarchyRecursive(unityObj, currentDepth + 1, maxDepth, visited);
                    }
                    else if (value is IEnumerable enumerable && !(value is string))
                    {
                        int index = 0;
                        foreach (var item in enumerable)
                        {
                            if (item is UnityEngine.Object unityItem)
                            {
                                Debug.Log($"{indent}  [{index}]:");
                                LogObjectHierarchyRecursive(unityItem, currentDepth + 2, maxDepth, visited);
                                index++;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }
    }
}