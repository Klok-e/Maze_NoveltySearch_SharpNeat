using System;
using System.Collections;
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

        public static float AvgSpeedWithPositions(Deque<Vector2> deque)
        {
            float distance = 0;
            for (int i = 0; i < deque.Length - 1; i++)
            {
                distance += (deque[i] - deque[i + 1]).magnitude;
            }
            if (deque.Length > 0)
            {
                return distance / deque.Length;
            }
            return 0;
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
            private List<T> internalList;
            private int size;

            public int Length { get; private set; }

            public Deque(int size)
            {
                internalList = new List<T>(size);
                this.size = size;
                Length = 0;
            }

            public void Add(T elem)
            {
                internalList.Add(elem);
                if (internalList.Count > size)
                {
                    internalList.RemoveAt(0);
                }
                Length = internalList.Count;
            }

            public T this[int index]
            {
                get
                {
                    return internalList[index];
                }
            }
        }
    }
}
