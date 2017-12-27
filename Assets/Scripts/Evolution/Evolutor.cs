using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpNeat.EvolutionAlgorithms;
using SharpNeat.Genomes.Neat;
using SharpNeat.Decoders;
using SharpNeat.Decoders.Neat;
using SharpNeat.DistanceMetrics;
using SharpNeat.SpeciationStrategies;
using SharpNeat.EvolutionAlgorithms.ComplexityRegulation;
using SharpNeat.Core;
using SharpNeat.Phenomes;
using UnityEngine;
using System.Threading;
using System.Collections;

namespace Scripts.Evolution
{
    internal class Evolutor : MonoBehaviour
    {
        [SerializeField] private ScriptableObjects.Containers.IntContainer _tickPerEvalCount;

        [SerializeField] private ScriptableObjects.Containers.IntContainer _inputCount;
        [SerializeField] private ScriptableObjects.Containers.IntContainer _outputCount;
        [SerializeField] private int _complexityThreshold;
        [SerializeField] private int _populationSize;
        [SerializeField] private int _specieCount;

        [SerializeField] private float _elitismProportion;
        [SerializeField] private float _AddConnectionMutationProbability;
        [SerializeField] private float _DeleteConnectionMutationProbability;
        [SerializeField] private float _AddNodeMutationProbability;
        [SerializeField] private float _ConnectionWeightMutationProbability;
        [SerializeField] private float _InitialInterconnectionsProportion;

        [SerializeField] private GameObject _agentPref;

        private NeatEvolutionAlgorithmParameters _eaParams;
        private NeatGenomeParameters _neatGenomeParams;
        private NetworkActivationScheme _activationScheme;
        private IGenomeListEvaluator<NeatGenome> _evaluator;

        private NeatEvolutionAlgorithm<NeatGenome> _ea;

        public IEnumerator Start()
        {
            Debug.Assert(_populationSize > 5);
            Debug.Log($"Main thread is {Thread.CurrentThread.ManagedThreadId}");

            _activationScheme = NetworkActivationScheme.CreateCyclicFixedTimestepsScheme(2, true);

            _eaParams = new NeatEvolutionAlgorithmParameters();
            _eaParams.ElitismProportion = _elitismProportion;
            _eaParams.SpecieCount = _specieCount;

            _neatGenomeParams = new NeatGenomeParameters();
            _neatGenomeParams.AddConnectionMutationProbability = _AddConnectionMutationProbability;
            _neatGenomeParams.DeleteConnectionMutationProbability = _DeleteConnectionMutationProbability;
            _neatGenomeParams.AddNodeMutationProbability = _AddNodeMutationProbability;
            _neatGenomeParams.ConnectionWeightMutationProbability = _ConnectionWeightMutationProbability;
            _neatGenomeParams.InitialInterconnectionsProportion = _InitialInterconnectionsProportion;
            _neatGenomeParams.FeedforwardOnly = _activationScheme.AcyclicNetwork;

            Debug.Log("Creating evaluator");
            yield return new WaitForSeconds(0.1f);
            _evaluator = CreateEvaluator();

            Debug.Log("Creating algorithm");
            yield return new WaitForSeconds(0.1f);
            _ea = CreateEvolutionAlgorithm(_evaluator, _populationSize);
            _ea.UpdateEvent += _ea_UpdateEvent;
        }

        private void _ea_UpdateEvent(object sender, EventArgs e)
        {
            Debug.Log(
                $"Generation: {_ea.CurrentGeneration};" +
                $" Max complexity: {_ea.Statistics._maxComplexity};" +
                $" Mean fitness: {_ea.Statistics._meanFitness};" +
                $" Max fitness: {_ea.Statistics._maxFitness}"
                );
        }

        public void StartEvolution()
        {
            _ea.StartContinue();
        }

        public void StopEvolution()
        {
            _ea.Stop();
        }

        private NeatGenomeDecoder CreateDecoder()
        {
            return new NeatGenomeDecoder(_activationScheme);
        }

        private IGenomeListEvaluator<NeatGenome> CreateEvaluator()
        {
            return new AgentListEvaluator(CreateDecoder(), this.gameObject, _agentPref, _tickPerEvalCount);
        }

        private NeatEvolutionAlgorithm<NeatGenome> CreateEvolutionAlgorithm(IGenomeListEvaluator<NeatGenome> evaluator, int populationSize)
        {
            var genomeFactory = new NeatGenomeFactory(_inputCount.Value, _outputCount.Value, _neatGenomeParams);
            var genomeList = genomeFactory.CreateGenomeList(populationSize, 0);
            return CreateEvolutionAlgorithm(evaluator, genomeList);
        }

        private NeatEvolutionAlgorithm<NeatGenome> CreateEvolutionAlgorithm(IGenomeListEvaluator<NeatGenome> evaluator, List<NeatGenome> list)
        {
            IDistanceMetric distanceMetric = new ManhattanDistanceMetric(1.0, 0.0, 10.0);
            ISpeciationStrategy<NeatGenome> speciationStrategy = new KMeansClusteringStrategy<NeatGenome>(distanceMetric);
            IComplexityRegulationStrategy complexityRegulationStrategy = new DefaultComplexityRegulationStrategy(ComplexityCeilingType.Absolute, _complexityThreshold);

            NeatEvolutionAlgorithm<NeatGenome> neatEvolutionAlgorithm = new NeatEvolutionAlgorithm<NeatGenome>(_eaParams, speciationStrategy, complexityRegulationStrategy);

            var genomeFactory = new NeatGenomeFactory(_inputCount.Value, _outputCount.Value, _neatGenomeParams);

            neatEvolutionAlgorithm.Initialize(evaluator, genomeFactory, list);

            return neatEvolutionAlgorithm;
        }
    }
}
