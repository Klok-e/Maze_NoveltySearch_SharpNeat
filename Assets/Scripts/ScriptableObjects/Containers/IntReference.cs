using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.ScriptableObjects.Containers
{
    [Serializable]
    public class IntReference
    {
        [SerializeField] private bool useConstant = true;
        [SerializeField] private int constantValue;
        [SerializeField] private IntContainer referencedValue;

        public int Value
        {
            get
            {
                return useConstant ? constantValue : referencedValue.value;
            }
        }
    }

    [CreateAssetMenu]
    internal class IntContainer : ScriptableObject
    {
        public int value;
    }
}
