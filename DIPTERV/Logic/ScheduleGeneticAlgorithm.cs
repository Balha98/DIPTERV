using DIPTERV.Data;
using GeneticSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIPTERV.Logic
{
  public class ScheduleGeneticAlgorithm
  {

    private List<Room> _rooms { get; set; }
    private List<SubjectDivision> _subjectDivisions { get; set; }
    private List<TimeBlock> _timeBlocks { get; set; }
    private List<Teacher> _teachers { get; set; }

    public ScheduleGeneticAlgorithm(List<Room> rooms, List<SubjectDivision> subjectDivisions, List<TimeBlock> timeBlocks, List<Teacher> teachers)
    {
      _rooms = rooms;
      _subjectDivisions = subjectDivisions;
      _timeBlocks = timeBlocks;
      _teachers = teachers;
    }

    public List<Course> RunGA()
    {
      var selection = new ScheduleElitistSelection();
      var crossover = new ScheduleUniformCrossover();
      var mutation = new ScheduleMutation(_subjectDivisions, _timeBlocks, _teachers);
      var fitness = new ScheduleFitness(_subjectDivisions, _timeBlocks);
      var chromosome = new Schedule(_rooms, _subjectDivisions, _timeBlocks);

      var population = new Population(1000, 2000, chromosome); //max számít, legyen 2xese a minnek


      var ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation);
      //ga.Termination = new FitnessStagnationTermination(100);
      ga.Termination = new OrTermination(new FitnessThresholdTermination(0), new FitnessStagnationTermination(80));
      ga.MutationProbability = 0.7f;
      ga.CrossoverProbability = 0.4f;
      ga.Reinsertion = new ElitistReinsertion();
      ga.GenerationRan += (s, e) => Console.WriteLine($"Generation {ga.GenerationsNumber}. (ga.BestChromosome.Fitness.Value): {((Schedule)ga.BestChromosome).Fitness.Value}, fitness.Evaluate(ga.BestChromosome): {fitness.Evaluate(ga.BestChromosome)}");


      ga.Start();
      Console.WriteLine();
      Console.WriteLine($"Best solution found has fitness: {ga.BestChromosome.Fitness}");
      Console.WriteLine($"Elapsed time: {ga.TimeEvolving}");


      /*
      Console.WriteLine("GA running...");
      int cnt = 0;
      int rounds = 50;
      for (int i = 1; i <= rounds; i++)
      {
          ga.Start();
          Console.WriteLine();
          Console.WriteLine($"{i}. round: Best solution found has fitness: {ga.BestChromosome.Fitness}");
          Console.WriteLine($"Elapsed time: {ga.TimeEvolving}");
          if (ga.BestChromosome.Fitness > -1000000)
          {
              cnt++;
              continue;
              //PrintSchedule(ga.BestChromosome);
              //ga.Fitness.Evaluate(ga.BestChromosome);
              //break;
          }
      }

      Console.WriteLine($"Found perfect solution: {cnt} times out of {rounds}");
       */

      PrintSchedule(ga.BestChromosome);
      PrintScheduleForExcel(ga.BestChromosome);
      fitness.PrintAllEvaluate(ga.BestChromosome);
      Console.WriteLine($"Best solution found has fitness: {ga.BestChromosome.Fitness}");

      //return courses
      List<Course> courses = new List<Course>();
      foreach (var i in ga.BestChromosome.GetGenes())
      {
        var actCourse = (Course)i.Value;
        actCourse.TimeBlockId = actCourse.TimeBlock.ID;
        courses.Add(actCourse);
      }

      return courses;
    }


    public static void PrintSchedule(IChromosome chromosome)
    {
      List<Course> courses = new List<Course>();
      foreach (var i in chromosome.GetGenes())
        courses.Add((Course)i.Value);

      courses = courses.OrderBy(x => x.SubjectDivision.Teacher).ThenBy(x => x.TimeBlock).ToList();

      Console.WriteLine($"{courses[0].SubjectDivision.Teacher.Name}'s courses:");
      Console.WriteLine($"{courses[0].TimeBlock.Day} {courses[0].TimeBlock.LessonNumber} lesson - School Class: {courses[0].SubjectDivision.SchoolClass.Name} Subject: {courses[0].SubjectDivision.Subject} Room: {courses[0].Room.Name}");

      for (int i = 1; i < courses.Count; i++)
      {
        if ((courses[i].SubjectDivision.Teacher != courses[i - 1].SubjectDivision.Teacher))
        {
          Console.WriteLine($"{courses[i].SubjectDivision.Teacher.Name}'s courses:");
        }
        Console.WriteLine($"{courses[i].TimeBlock.Day} {courses[i].TimeBlock.LessonNumber} lesson - School Class: {courses[i].SubjectDivision.SchoolClass.Name} Subject: {courses[i].SubjectDivision.Subject} Room: {courses[i].Room.Name}");
      }
    }

    private static void PrintScheduleForExcel(IChromosome chromosome)
    {
      List<Course> courses = new List<Course>();
      foreach (var i in chromosome.GetGenes())
        courses.Add((Course)i.Value);

      courses = courses.OrderBy(x => x.SubjectDivision.Teacher).ThenBy(x => x.TimeBlock).ToList();

      for (int i = 0; i < courses.Count; i++)
      {
        Console.WriteLine();
        Console.WriteLine($"{courses[i].RoomId} {courses[i].SubjectDivisinId} {courses[i].TimeBlockId} ");
        Console.WriteLine($"{courses[i].Room.ID} {courses[i].SubjectDivision.ID} {courses[i].TimeBlock.ID} ");
      }
    }
  }
}
