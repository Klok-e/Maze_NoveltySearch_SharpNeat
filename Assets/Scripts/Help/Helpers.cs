using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Help
{
    public static class Helpers
    {
        public static string ArrayToString(double[] arr)
        {
            string str = "";
            for (int i = 0; i < arr.Length; i++)
            {
                str += String.Format("{0,6}", Math.Round(arr[i], 3).ToString());
            }
            return str;
        }

        public static void CopyElements<T, T1>(Dictionary<T, T1> dictionaryFrom, Dictionary<T, T1> dictionaryTo)
        {
            dictionaryFrom.ToList().ForEach(x => dictionaryTo.Add(x.Key, x.Value));
        }

        public static Vector2 RandomVector2()
        {
            return new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
        }

        #region Normalize overloads

        public enum NormalizeRange
        {
            zero_one,
            minusOne_one
        }

        /// <param name="x">to normalize</param>
        /// <param name="minX">min value of x</param>
        /// <param name="maxX">max value of x</param>
        /// <param name="range">if true then normalizes to 0..1, else - -1..1</param>
        /// <returns></returns>
        public static double NormalizeNumber(double x, double minX, double maxX, NormalizeRange range)
        {
            Debug.Assert(minX <= x && x <= maxX);

            double ans = (x - minX) / (maxX - minX);
            if (range == NormalizeRange.minusOne_one)
            {
                ans = -1 + 2 * ans;
                Debug.Assert(-1 <= ans && ans <= 1);
            }
            else if (range == NormalizeRange.zero_one)
            {
                Debug.Assert(0 <= ans && ans <= 1);
            }
            return ans;
        }

        /// <param name="x">to normalize</param>
        /// <param name="minX">min value of x</param>
        /// <param name="maxX">max value of x</param>
        /// <param name="range">if true then normalizes to 0..1, else - -1..1</param>
        /// <returns></returns>
        public static float NormalizeNumber(float x, float minX, float maxX, NormalizeRange range)
        {
            Debug.Assert(minX <= x && x <= maxX);

            float ans = (x - minX) / (maxX - minX);
            if (range == NormalizeRange.minusOne_one)
            {
                ans = -1 + 2 * ans;
                Debug.Assert(-1 <= ans && ans <= 1);
            }
            else if (range == NormalizeRange.zero_one)
            {
                Debug.Assert(0 <= ans && ans <= 1);
            }
            return ans;
        }

        #endregion Normalize overloads

        public class Deque<T>
        {
            private Queue<T> q;

            public int Length { get; }
            public int QueueLength { get { return q.Count; } }

            public Deque(int size)
            {
                Length = size;
                q = new Queue<T>();
            }

            public void Enqueue(T obj)
            {
                q.Enqueue(obj);
                while (q.Count > Length)
                    q.Dequeue();
            }

            public T Dequeue()
            {
                return q.Dequeue();
            }

            public T Peek()
            {
                return q.Peek();
            }
        }
    }
}
