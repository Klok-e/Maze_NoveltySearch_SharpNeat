using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Agent
{
    [RequireComponent(typeof(Rigidbody))]
    public class MoveController : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        public void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Move(Vector3 vector)
        {
            _rigidbody.AddRelativeForce(vector, ForceMode.Impulse);
        }

        public void Rotate(Vector3 vector)
        {
            _rigidbody.AddRelativeTorque(new Vector3(0, vector.y, 0), ForceMode.Impulse);
        }
    }
}
