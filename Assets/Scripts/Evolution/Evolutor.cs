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

namespace Scripts.Evolution
{
    internal class Evolutor : MonoBehaviour
    {
        [SerializeField] private Containers.IntReference _inputCount;
        [SerializeField] private Containers.IntReference _outputCount;
        [SerializeField] private Containers.IntReference _complexityThreshold;
        [SerializeField] private Containers.IntReference _populationSize;
        [SerializeField] private Containers.IntReference _specieCount;
        [SerializeField] private Containers.IntReference _elitismProportion;

        [SerializeField] private Containers.FloatReference _AddConnectionMutationProbability;
        [SerializeField] private Containers.FloatReference _DeleteConnectionMutationProbability;
        [SerializeField] private Containers.FloatReference _AddNodeMutationProbability;
        [SerializeField] private Containers.FloatReference _ConnectionWeightMutationProbability;
        [SerializeField] private Containers.FloatReference _InitialInterconnectionsProportion;

        private NeatEvolutionAlgorithmParameters _eaParams;
        private NeatGenomeParameters _neatGenomeParams;
        private NetworkActivationScheme _activationScheme;

        private NeatEvolutionAlgorithm<NeatGenome> _ea;

        public void Start()
        {
            Debug.Assert(_populationSize.Value > 5);

            _activationScheme = NetworkActivationScheme.CreateCyclicFixedTimestepsScheme(2, true);

            _eaParams = new NeatEvolutionAlgorithmParameters();
            _eaParams.ElitismProportion = _elitismProportion.Value;
            _eaParams.SpecieCount = _specieCount.Value;

            _neatGenomeParams = new NeatGenomeParameters();
            _neatGenomeParams.AddConnectionMutationProbability = _AddConnectionMutationProbability.Value;
            _neatGenomeParams.DeleteConnectionMutationProbability = _DeleteConnectionMutationProbability.Value;
            _neatGenomeParams.AddNodeMutationProbability = _AddNodeMutationProbability.Value;
            _neatGenomeParams.ConnectionWeightMutationProbability = _ConnectionWeightMutationProbability.Value;
            _neatGenomeParams.InitialInterconnectionsProportion = _InitialInterconnectionsProportion.Value;
            _neatGenomeParams.FeedforwardOnly = _activationScheme.AcyclicNetwork;

            _ea = CreateEvolutionAlgorithm(new AgentListEvaluator(CreateDecoder()), _populationSize.Value);
        }

        public NeatGenomeDecoder CreateDecoder()
        {
            return new NeatGenomeDecoder(_activationScheme);
        }

        public NeatEvolutionAlgorithm<NeatGenome> CreateEvolutionAlgorithm(IGenomeListEvaluator<NeatGenome> evaluator, int populationSize)
        {
            var genomeFactory = new NeatGenomeFactory(_inputCount.Value, _outputCount.Value, _neatGenomeParams);
            var genomeList = genomeFactory.CreateGenomeList(populationSize, 0);
            return CreateEvolutionAlgorithm(evaluator, genomeList);
        }

        public NeatEvolutionAlgorithm<NeatGenome> CreateEvolutionAlgorithm(IGenomeListEvaluator<NeatGenome> evaluator, List<NeatGenome> list)
        {
            IDistanceMetric distanceMetric = new ManhattanDistanceMetric(1.0, 0.0, 10.0);
            ISpeciationStrategy<NeatGenome> speciationStrategy = new KMeansClusteringStrategy<NeatGenome>(distanceMetric);
            IComplexityRegulationStrategy complexityRegulationStrategy = new DefaultComplexityRegulationStrategy(ComplexityCeilingType.Absolute, _complexityThreshold.Value);

            NeatEvolutionAlgorithm<NeatGenome> neatEvolutionAlgorithm = new NeatEvolutionAlgorithm<NeatGenome>(_eaParams, speciationStrategy, complexityRegulationStrategy);

            var genomeFactory = new NeatGenomeFactory(_inputCount.Value, _outputCount.Value, _neatGenomeParams);

            neatEvolutionAlgorithm.Initialize(evaluator, genomeFactory, list);

            return neatEvolutionAlgorithm;
        }
    }
}
