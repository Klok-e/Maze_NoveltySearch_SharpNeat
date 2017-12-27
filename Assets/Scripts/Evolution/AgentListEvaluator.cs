using Scripts.ScriptableObjects.Containers;
using Scripts.Help;
using SharpNeat.Core;
using SharpNeat.Genomes.Neat;
using SharpNeat.Phenomes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts.Evolution
{
    internal class AgentListEvaluator : IGenomeListEvaluator<NeatGenome>
    {
        private IGenomeDecoder<NeatGenome, IBlackBox> _decoder;
        private GameObject _gameObject;
        private GameObject _agentPrefab;
        private IntContainer _tickPerEvalCount;

        private Novelty_archive _novelty_Archive;
        private const float _noveltyThreshold = 1;

        private float[] _fitnesses;

        #region Cached

        private Agent.Abstract.AAgentController[] _agents;

        #endregion Cached

        public AgentListEvaluator(IGenomeDecoder<NeatGenome, IBlackBox> decoder, GameObject gameObject, GameObject agentPrefab, IntContainer tickPerEvalCount)
        {
            _gameObject = gameObject;
            _decoder = decoder;
            _agentPrefab = agentPrefab;
            _tickPerEvalCount = tickPerEvalCount;
            _evaluationCount = 0;

            _novelty_Archive = new Novelty_archive();
            _novelty_Archive.Add(Vec3ToVec2(gameObject.transform.position));

            UnityThread.InitUnityThread();
        }

        #region Interface

        public ulong EvaluationCount { get { return _evaluationCount; } }
        private ulong _evaluationCount;

        public bool StopConditionSatisfied { get { return false; } }

        public void Evaluate(IList<NeatGenome> genomeList)
        {
            var boxes = new IBlackBox[genomeList.Count];
            for (int i = 0; i < genomeList.Count; i++)
            {
                boxes[i] = _decoder.Decode(genomeList[i]);
            }
            _fitnesses = null;

            if (_evaluationCount > 0)
            {
                UnityThread.ExecuteCoroutine(EvaluateInMainThread(boxes));
                Debug.Log($"Evaluation requested in {Thread.CurrentThread.ManagedThreadId} thread");

                while (_fitnesses == null)
                {
                    Thread.Sleep(10);
                }
                Debug.Assert(genomeList.Count == _fitnesses.Length);
                for (int i = 0; i < _fitnesses.Length; i++)
                {
                    genomeList[i].EvaluationInfo.SetFitness(_fitnesses[i]);
                }
                Debug.Log("Evaluation finished");
            }
            else
            {
                Debug.Log("Evaluator initialized");
                foreach (var item in genomeList)
                {
                    item.EvaluationInfo.SetFitness(0);
                }
            }
            _evaluationCount += 1;
        }

        public void Reset()
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception e) { Debug.Log(e); }
        }

        #endregion Interface

        private IEnumerator EvaluateInMainThread(IBlackBox[] boxes)
        {
            Debug.Log($"Evaluating in {Thread.CurrentThread.ManagedThreadId} thread");
            if (_agents == null)
            {
                _agents = new Agent.Abstract.AAgentController[boxes.Length];
                for (int i = 0; i < boxes.Length; i++)
                {
                    var obj = Coroutiner.Instantiate(_agentPrefab, _gameObject.transform);
                    _agents[i] = obj.GetComponent<Agent.Abstract.AAgentController>();
                }
            }

            for (int i = 0; i < boxes.Length; i++)
            {
                var ag = (Agent.NetworkController)_agents[i];
                ag.Net = boxes[i];
                ag.Reset(_gameObject.transform.position);
            }

            yield return null;
            for (int i = 0; i < _tickPerEvalCount.Value; i++)
            {
                foreach (var item in _agents)
                {
                    item.Tick();
                }
                yield return new WaitForFixedUpdate();
            }

            _fitnesses = CalculateFitness(_agents);
        }

        private float[] CalculateFitness(Agent.Abstract.AAgentController[] agents)
        {
            var fitnesses = new float[agents.Length];

            var popVectors = new List<Vector2>(agents.Length);
            foreach (var item in agents)
            {
                popVectors.Add(Vec3ToVec2(item.transform.position));
            }

            for (int i = 0; i < agents.Length; i++)
            {
                var vecPos = Vec3ToVec2(agents[i].transform.position);

                var compToArch = _novelty_Archive.CompareToArchive(vecPos, 5);
                if (compToArch > _noveltyThreshold)
                {
                    _novelty_Archive.Add(vecPos);
                }

                var compToPop = Novelty_archive.CompareToPopulation(popVectors, vecPos, 5);
                fitnesses[i] = compToArch + compToPop;
            }
            return fitnesses;
        }

        private static Vector2 Vec3ToVec2(Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.z);
        }
    }
}
