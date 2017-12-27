using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Evolution
{
    internal class Novelty_archive
    {
        private List<Vector2> _archive;

        public Novelty_archive()
        {
            _archive = new List<Vector2>();
        }

        public void Add(Vector2 toAdd)
        {
            _archive.Add(toAdd);
        }

        public static float CompareToPopulation(List<Vector2> pop, Vector2 toCompare, int k)
        {
            //calculate all distances
            var distances = new List<float>(pop.Count);
            foreach (var item in pop)
            {
                distances.Add(Vector2.Distance(toCompare, item));
            }

            //find k smallest distances
            var deque = new List<float>(k)
            {
                distances[0]
            };
            foreach (var item in distances)
            {
                var temp_mx = deque.Max();
                if (temp_mx > item)
                {
                    deque.Add(item);
                    if (deque.Count > k)
                    {
                        deque.Remove(temp_mx);
                    }
                }
            }

            //calculate mean
            float mean = 0;
            for (int i = 0; i < deque.Count; i++)
            {
                mean += deque[i] / deque.Count;
            }
            return mean;
        }

        public float CompareToArchive(Vector2 toCompare, int k)
        {
            return CompareToPopulation(_archive, toCompare, k);
        }
    }
}
