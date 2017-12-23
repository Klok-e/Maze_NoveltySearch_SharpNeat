using UnityEngine;
using System.Collections;

namespace Scripts.Agent.Abstract
{
    public abstract class AMoveController : MonoBehaviour
    {
        public abstract void Move(Vector3 vector);

        public abstract void Rotate(Vector3 vector);
    }
}
