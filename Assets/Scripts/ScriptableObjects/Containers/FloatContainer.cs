using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.ScriptableObjects.Containers
{
    [CreateAssetMenu]
    internal class FloatContainer : ScriptableObject
    {
        public float _value;
        public float Value { get { return _value; } }
    }
}
