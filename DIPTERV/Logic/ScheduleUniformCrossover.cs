using GeneticSharp;

namespace DIPTERV.Logic
{
  public class ScheduleUniformCrossover : CrossoverBase
  {
    public ScheduleUniformCrossover(float mixProbability) : base(2, 2)
    {
      MixProbability = mixProbability;
    }

    public ScheduleUniformCrossover() : this(0.5f)
    {
    }

    public float MixProbability { get; set; }

    protected override IList<IChromosome> PerformCross(IList<IChromosome> parents)
    {
      var firstParent = parents[0];
      var secondParent = parents[1];
      var firstChild = firstParent.CreateNew();
      var secondChild = secondParent.CreateNew();

      for (int i = 0; i < firstParent.Length; i++)
      {
        if (RandomizationProvider.Current.GetDouble() < MixProbability)
        {
          firstChild.ReplaceGene(i, firstParent.GetGene(i));
          secondChild.ReplaceGene(i, secondParent.GetGene(i));
        }
        else
        {
          firstChild.ReplaceGene(i, secondParent.GetGene(i));
          secondChild.ReplaceGene(i, firstParent.GetGene(i));
        }
      }

      return new List<IChromosome> { firstChild, secondChild };
    }
  }
}
