using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Agent
{
    [RequireComponent(typeof(Rigidbody))]
    public class MoveController : Abstract.AMoveController
    {
        private Rigidbody _rigidbody;

        public void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public override void Move(Vector3 vector)
        {
            _rigidbody.AddRelativeForce(new Vector3(0, 0, vector.z), ForceMode.Impulse);
        }

        public override void Rotate(Vector3 vector)
        {
            _rigidbody.AddRelativeTorque(new Vector3(0, vector.y, 0), ForceMode.Impulse);
        }
    }
}
