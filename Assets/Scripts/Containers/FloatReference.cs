using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Containers
{
    [Serializable]
    public class FloatReference
    {
        [SerializeField] private bool useConstant = true;
        [SerializeField] private float constantValue;
        [SerializeField] private FloatContainer referencedValue;

        public float Value
        {
            get { return useConstant ? constantValue : referencedValue.value; }
        }

        [CreateAssetMenu]
        private class FloatContainer : ScriptableObject
        {
            public float value;
        }
    }
}
