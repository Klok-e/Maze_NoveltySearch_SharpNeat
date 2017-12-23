using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scripts.Agent
{
    public class PlayerInputController : Abstract.AAgentController
    {
        public override void Reset(Vector3 defaultPos)
        {
            transform.position = defaultPos;
        }

        protected override void DoAction()
        {
            throw new NotImplementedException();
        }

        private void FixedUpdate()
        {
            ProcessInput();
        }

        private void ProcessInput()
        {
            Debug.Assert(_rotationSpeed.Value > 0, "Sensitivity can't be zero");

            var vertical = Input.GetAxis("Vertical");
            var mouseX = Input.GetAxis("Mouse X");

            var val = (mouseX > 0 ? 1 : -1);
            var rot = Vector3.one * (Math.Abs(mouseX) > 1 ? val : mouseX) * _rotationSpeed.Value * Time.deltaTime;

            var move = Vector3.one * vertical * _movingSpeed.Value * Time.deltaTime;

            var movable = GetComponent<Abstract.AMoveController>();
            movable.Move(move);
            movable.Rotate(rot);
        }
    }
}
