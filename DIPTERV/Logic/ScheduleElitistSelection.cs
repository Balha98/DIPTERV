using GeneticSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIPTERV.Logic
{
    public class ScheduleElitistSelection : SelectionBase
    {

        public ScheduleElitistSelection()
            : this(1)
        {
        }

        readonly int _previousGenerationChromosomesNumber;
        List<IChromosome> _previousGenerationChromosomes;

        public ScheduleElitistSelection(int previousGenerationChromosomesNumber) : base(2)
        {
            _previousGenerationChromosomesNumber = previousGenerationChromosomesNumber;
        }

        protected override IList<IChromosome> PerformSelectChromosomes(int number, Generation generation)
        {
            if (generation.Number == 1)
                _previousGenerationChromosomes = new List<IChromosome>();

            _previousGenerationChromosomes.AddRange(generation.Chromosomes);

            var ordered = _previousGenerationChromosomes.OrderByDescending(c => c.Fitness);
            var result = ordered.Take(number).ToList();

            _previousGenerationChromosomes = result.Take(_previousGenerationChromosomesNumber).ToList();

            return result;
        }
    }
}
