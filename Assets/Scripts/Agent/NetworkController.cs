using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SharpNeat.Phenomes;
using Scripts.Help;

namespace Scripts.Agent
{
    public class NetworkController : Abstract.AAgentController
    {
        [SerializeField] private ScriptableObjects.Containers.IntContainer _inputCount;
        [SerializeField] private ScriptableObjects.Containers.IntContainer _outputCount;

        [SerializeField] private ScriptableObjects.Containers.IntContainer _amountOfWallSensors;
        [SerializeField] private ScriptableObjects.Containers.IntContainer _sensorsDensity;
        [SerializeField] private ScriptableObjects.Containers.FloatContainer _sensorRange;

        public IBlackBox Net { get; set; }

        #region Cached variables

        private List<int> _angles;
        private List<float> _allData;

        #endregion Cached variables

        protected override void DoAction()
        {
            if (Net != null)
            {
                var state = GetEnvironmentState();
                var prediction = Predict(state);
                ApplyPrediction(prediction);
            }
        }

        public override void Reset(Vector3 defaultPos)
        {
            transform.position = defaultPos;
            if (Net != null)
            {
                Net.ResetState();
            }
        }

        public void Start()
        {
            Debug.Assert(_movingSpeed.Value > 0);
            Debug.Assert(_rotationSpeed.Value > 0);

            _angles = CalculateAngles();
            _allData = new List<float>();

            var state = GetEnvironmentState();
            _inputCount.value = state.Length;
        }

        private List<int> CalculateAngles()
        {
            var angles = new List<int>();
            var mul = 0;
            for (int i = 0; i < _amountOfWallSensors.value; i++)
            {
                if (i % 2 == 0)
                {
                    angles.Add(90 - _sensorsDensity.value * mul);
                }
                if (i % 2 == 1)
                {
                    mul += 1;
                    angles.Add(90 + _sensorsDensity.value * mul);
                }
            }
            if (_amountOfWallSensors.value % 2 != 1)
            {
                for (int i = 0; i < angles.Count; i++)
                {
                    angles[i] -= _sensorsDensity.value / 2;//why does it work?
                }
            }
            return angles;
        }

        private void ApplyPrediction(float[] prediction)
        {
            /*
             * [0] - forward speed
             * [1] - rotate by some degree
             */
            var movable = GetComponent<Abstract.AMoveController>();

            var moveBy = Helpers.NormalizeNumber(prediction[0], 0, 1, Helpers.NormalizeRange.minusOne_one);
            movable.Move(Vector3.one * moveBy * _movingSpeed.Value * Time.deltaTime);

            var rotateBy = Helpers.NormalizeNumber(prediction[1], 0, 1, Helpers.NormalizeRange.minusOne_one);
            movable.Rotate(Vector3.one * rotateBy * _rotationSpeed.Value * Time.deltaTime);
        }

        private float[] Predict(float[] state)
        {
            Debug.Assert(state.Length == Net.InputCount);
            for (int i = 0; i < Net.InputCount; i++)
            {
                Net.InputSignalArray[i] = state[i];
            }
            Net.Activate();

            Debug.Assert(Net.OutputCount == _outputCount.value);
            var prediction = new float[Net.OutputCount];
            for (int i = 0; i < Net.OutputCount; i++)
            {
                prediction[i] = (float)Net.OutputSignalArray[i];
            }
            return prediction;
        }

        private float[] GetEnvironmentState()
        {
            _allData.Clear();

            //_angles = CalculateAngles();

            #region Raycasts

            var eyesData = new float[_amountOfWallSensors.value, 1];

            var pos = transform.position;

            var count = 0;
            foreach (var angle in _angles)
            {
                var dir = transform.TransformDirection(new Vector3(Mathf.Cos(Mathf.Deg2Rad * (angle)), 0, Mathf.Sin(Mathf.Deg2Rad * (angle))).normalized);
                RaycastHit hit;

                bool isHit = Physics.Raycast(pos, dir, out hit, _sensorRange.value);

                if (isHit)
                {
                    Debug.DrawLine(pos, hit.point);
                    eyesData[count, 0] = 1 - (hit.distance / _sensorRange.value);
                }
                else
                {
                    Debug.DrawRay(pos, dir);
                }
                count += 1; //every eye must see distance and what is it
            }

            #endregion Raycasts

            foreach (var item in eyesData)
            {
                _allData.Add(item);
            }

            return _allData.ToArray();
        }
    }
}
