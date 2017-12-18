using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scripts.Agent
{
    public class PlayerInputController : MonoBehaviour
    {
        [SerializeField] private Containers.FloatReference sensitivity;

        private void Update()
        {
            var vertical = Input.GetAxis("Vertical");
            var mouseX = Input.GetAxis("Mouse X");

            var rot = new Vector3(0, 0, 0);
            if (Mathf.Abs(mouseX) > 0)
            {
                Debug.Assert(sensitivity.Value > 0, "Sensitivity can't be zero");
                rot.y = mouseX * sensitivity.Value * Time.deltaTime;
            }
            vertical *= 10 * Time.deltaTime;

            SendMessage(Messages.move, new Vector3(0, 0, vertical));
            SendMessage(Messages.rotate, rot);
        }
    }
}
