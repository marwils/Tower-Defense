using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MarwilsTD.Helper
{
    public class CoroutineRunner : MonoBehaviour
    {
        private static CoroutineRunner Instance;
        private static HashSet<Coroutine> _runningCoroutines = new HashSet<Coroutine>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public static Coroutine Start(IEnumerator routine)
        {
            if (Instance == null)
            {
                CreateInstance();
            }

            var coroutine = Instance.StartCoroutine(WrapRoutine(routine));
            _runningCoroutines.Add(coroutine);
            return coroutine;
        }

        public static void Stop(Coroutine routine)
        {
            if (Instance != null && routine != null)
            {
                Instance.StopCoroutine(routine);
                _runningCoroutines.Remove(routine);
            }
        }

        public static void StopAll()
        {
            if (Instance != null)
            {
                Instance.StopAllCoroutines();
                _runningCoroutines.Clear();
            }
        }

        public static bool IsRunning(Coroutine routine)
        {
            return routine != null && _runningCoroutines.Contains(routine);
        }

        public static bool AllCoroutinesFinished(List<Coroutine> coroutines)
        {
            foreach (var coroutine in coroutines)
            {
                if (coroutine != null && IsRunning(coroutine))
                {
                    return false;
                }
            }
            return true;
        }

        private static IEnumerator WrapRoutine(IEnumerator routine)
        {
            yield return routine;

            _runningCoroutines.RemoveWhere(c => c == null);
        }

        private static void CreateInstance()
        {
            var go = new GameObject("CoroutineRunner");
            Instance = go.AddComponent<CoroutineRunner>();
            DontDestroyOnLoad(go);
        }

        private void OnDestroy()
        {
            StopAll();
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}