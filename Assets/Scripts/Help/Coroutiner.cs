using System.Collections;
using UnityEngine;

namespace Scripts.Help
{
    public static class Coroutiner
    {
        private static CoroutinerInstance routeneHandler = CreateInstance("Coroutiner/Instantiater");

        public static Coroutine StartCoroutine(IEnumerator iterationResult)
        {
            return routeneHandler.ProcessWork(iterationResult);
        }

        public static GameObject Instantiate(GameObject toInst, Transform parent)
        {
            return routeneHandler.CallInstantiate(toInst, parent);
        }

        #region Private

        private static CoroutinerInstance CreateInstance(string name)
        {
            GameObject routineHandlerGo = new GameObject(name);
            return routineHandlerGo.AddComponent(typeof(CoroutinerInstance)) as CoroutinerInstance;
        }

        private class CoroutinerInstance : MonoBehaviour
        {
            public Coroutine ProcessWork(IEnumerator iterationResult)
            {
                return StartCoroutine(iterationResult);
            }

            public GameObject CallInstantiate(GameObject toInst, Transform parent)
            {
                return Instantiate(toInst, parent);
            }
        }

        #endregion Private
    }
}
