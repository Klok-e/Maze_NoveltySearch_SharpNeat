using SharpNeat.Core;
using SharpNeat.Genomes.Neat;
using SharpNeat.Phenomes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Evolution
{
    internal class AgentListEvaluator : IGenomeListEvaluator<NeatGenome>
    {
        private IGenomeDecoder<NeatGenome, IBlackBox> _decoder;

        public AgentListEvaluator(IGenomeDecoder<NeatGenome, IBlackBox> decoder)
        {
            _decoder = decoder;
        }

        public ulong EvaluationCount { get { throw new NotImplementedException(); } }

        public bool StopConditionSatisfied { get { throw new NotImplementedException(); } }

        public void Evaluate(IList<NeatGenome> genomeList)
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
