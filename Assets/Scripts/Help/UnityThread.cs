using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Help
{
    public class UnityThread : MonoBehaviour
    {
        private static UnityThread instance = null;

        private static List<Action> actionQueueUpdateFunc = new List<Action>();

        private List<Action> actionCopiedQueueUpdateFunc = new List<Action>();

        private static volatile bool noActionQueueToExecuteUpdateFunc = true;

        public static void InitUnityThread(bool visible = false)
        {
            if (instance != null)
            {
                return;
            }
            if (Application.isPlaying)
            {
                var obj = new GameObject("MainThreadExecuter");
                if (!visible)
                {
                    obj.hideFlags = HideFlags.HideAndDontSave;
                }

                DontDestroyOnLoad(obj);
                instance = obj.AddComponent<UnityThread>();
            }
        }

        public static void ExecuteCoroutine(IEnumerator action)
        {
            if (instance != null)
            {
                ExecuteInUpdate(() => instance.StartCoroutine(action));
            }
        }

        public static void ExecuteInUpdate(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException();
            }

            lock (actionQueueUpdateFunc)
            {
                actionQueueUpdateFunc.Add(action);
                noActionQueueToExecuteUpdateFunc = false;
            }
        }

        public void Update()
        {
            if (noActionQueueToExecuteUpdateFunc)
            {
                return;
            }

            actionCopiedQueueUpdateFunc.Clear();
            lock (actionQueueUpdateFunc)
            {
                actionCopiedQueueUpdateFunc.AddRange(actionQueueUpdateFunc);
                actionQueueUpdateFunc.Clear();
                noActionQueueToExecuteUpdateFunc = true;
            }
            foreach (var item in actionCopiedQueueUpdateFunc)
            {
                item.Invoke();
            }
        }

        private void OnDisable()
        {
            if (instance == this)
            {
                instance = null;
            }
        }
    }
}
