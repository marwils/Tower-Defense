using System.Collections;

using UnityEngine;

namespace Helper
{
    public class CoroutineRunner : MonoBehaviour
    {
        private static CoroutineRunner Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public static Coroutine Start(IEnumerator routine)
        {
            return Instance.StartCoroutine(routine);
        }

        public static void Stop(Coroutine routine)
        {
            Instance.StopCoroutine(routine);
        }

        public static void StopAll()
        {
            Instance.StopAllCoroutines();
        }

        private void OnDestroy()
        {
            StopAll();
        }
    }
}