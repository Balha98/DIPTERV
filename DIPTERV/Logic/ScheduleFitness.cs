using DIPTERV.Data;
using GeneticSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DIPTERV.Logic
{
    public class ScheduleFitness : IFitness
    {

        private List<Course> courses = new List<Course>();
        private List<SubjectDivision> _subjectDivisions;
        private List<TimeBlock> _timeBlocks;

        private static readonly char[] digits = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        private static readonly string[] easySubjects = new[] { "ének - zene", "vizuális kultúra", "rajz", "testnevelés", "technika", "etika", "osztályfőnöki" };

        public ScheduleFitness(List<SubjectDivision> subjectDivisions, List<TimeBlock> timeBlocks)
        {
            _subjectDivisions = subjectDivisions;
            _timeBlocks = timeBlocks;
        }

        public double Evaluate(IChromosome chromosome)
        {
            courses.Clear();
            foreach (var i in chromosome.GetGenes())
                courses.Add((Course)i.Value);

            return Penalise();
        }

    public void PrintAllEvaluate(IChromosome chromosome)
    {
      courses.Clear();
      foreach (var i in chromosome.GetGenes())
        courses.Add((Course)i.Value);
      //hard constraints
      //Console.WriteLine($"MoreThanOneSubjectDivison(): {MoreThanOneSubjectDivison()}");
      //Console.WriteLine($"SubjectDivisionMissing(): {SubjectDivisionMissing()}");
      Console.WriteLine($"MoreThanOneTimeBlockinaRoomAtTheSameTime(): {MoreThanOneTimeBlockinaRoomAtTheSameTime()}");
      Console.WriteLine($"TeacherMoreThanOnePlaceAtTheSameTime(): {TeacherMoreThanOnePlaceAtTheSameTime()}");
      Console.WriteLine($"SchoolClassMoreThanOnePlaceAtTheSameTime(): {SchoolClassMoreThanOnePlaceAtTheSameTime()}");
      Console.WriteLine($"TeacherNotAvailable(): {TeacherNotAvailable()}");


      //helper
      //result -= NotUniversalTB();

      //soft constraints
      
      Console.WriteLine($"TeacherScheduleHoles(): {TeacherScheduleHoles()}");
      Console.WriteLine($"SubjectsNotDistributed(): {SubjectsNotDistributed()}");
      Console.WriteLine($"DayStartsWithEasySubjects(): {DayStartsWithEasySubjects()}");
        }

        private double Penalise()
        {
            double result = 0;

            //hard constraints
            result -= MoreThanOneSubjectDivison(); // nem kell
            result -= SubjectDivisionMissing(); // nem kell
            result -= MoreThanOneTimeBlockinaRoomAtTheSameTime();
            result -= TeacherMoreThanOnePlaceAtTheSameTime();
            result -= SchoolClassMoreThanOnePlaceAtTheSameTime();
            result -= TeacherNotAvailable();

            //helper
            //result -= NotUniversalTB();

            //soft constraints
            result -= TeacherScheduleHoles();
            result -= SubjectsNotDistributed();
            result -= DayStartsWithEasySubjects();
            return result;
        }


        private double NotUniversalTB()
        {
            Dictionary<TimeBlock, int> LocalTimeBlocks = new Dictionary<TimeBlock, int>();

            foreach (var c in courses)
            {
                if (LocalTimeBlocks.ContainsKey(c.TimeBlock))
                    LocalTimeBlocks[c.TimeBlock] += 1;
                else
                    LocalTimeBlocks[c.TimeBlock] = 1;
            }

            double avg = _subjectDivisions.Count / _timeBlocks.Count;
            double eps = 2.0;

            var faults = LocalTimeBlocks.Where(x => (x.Value > (avg+eps)) || (x.Value < (avg - eps))).Sum(x => Math.Abs(x.Value - avg));
            return faults * 1000000;
        }



        //(Room, TimeBlock).count > 1
        private double MoreThanOneTimeBlockinaRoomAtTheSameTime()
        {
            Dictionary <(TimeBlock tb, Room r), int > roomOccupancy = new Dictionary<(TimeBlock tb, Room r), int >();

            foreach (var c in courses)
            {
                //room used more than once in a time
                if (roomOccupancy.ContainsKey((c.TimeBlock, c.Room)))
                    roomOccupancy[(c.TimeBlock, c.Room)] += 1;
                else
                    roomOccupancy[(c.TimeBlock, c.Room)] = 1;
            }
            var faults = roomOccupancy.Where(x => x.Value > 1).Sum(x => x.Value - 1);
            return faults * PenaltyPoint.MoreThanOneTimeBlockinaRoomAtTheSameTime;
        }

        //SubjectDivison.count > 1
        private double MoreThanOneSubjectDivison()
        {
            Dictionary <int,int> sd = new Dictionary<int,int>();
            foreach (var c in courses)
            {
                if (sd.ContainsKey(c.SubjectDivision.ID))
                    sd[c.SubjectDivision.ID]++;
                else
                    sd[c.SubjectDivision.ID] = 1;
            }

            var faults = sd.Where(x => x.Value > 1).Sum(x => x.Value - 1);
            return faults * PenaltyPoint.MoreThanOneSubjectDivison;
        }

        //SubjectDivison.count = 0
        private double SubjectDivisionMissing()
        {
            var excludedIDs = new HashSet<int>(courses.Select(sd => sd.SubjectDivision.ID));
            var result = _subjectDivisions.Where(sd => !excludedIDs.Contains(sd.ID)).Count();

            return result * PenaltyPoint.SubjectDivisionMissing;
        }

        //(SubjectDivison.Teacher,TimeBlock).count > 1
        private double TeacherMoreThanOnePlaceAtTheSameTime()
        {
            Dictionary<(TimeBlock tb, Teacher t), int> teacherOccupancy = new Dictionary<(TimeBlock tb, Teacher r), int>();

            foreach (var c in courses)
            {
                //room used more than once in a time
                if (teacherOccupancy.ContainsKey((c.TimeBlock, c.SubjectDivision.Teacher)))
                    teacherOccupancy[(c.TimeBlock, c.SubjectDivision.Teacher)] += 1;
                else
                    teacherOccupancy[(c.TimeBlock, c.SubjectDivision.Teacher)] = 1;
            }
            var faults = teacherOccupancy.Where(x => x.Value > 1).Sum(x => x.Value - 1);
            return faults * PenaltyPoint.TeacherMoreThanOnePlaceAtTheSameTime;
        }

        //(SubjectDivision.SchoolClass, TimeBlock).count > 1
        private double SchoolClassMoreThanOnePlaceAtTheSameTime()
        {
            Dictionary<(TimeBlock tb, SchoolClass t), int> schoolClassOccupancy = new Dictionary<(TimeBlock tb, SchoolClass r), int>();

            foreach (var c in courses)
            {
                //room used more than once in a time
                if (schoolClassOccupancy.ContainsKey((c.TimeBlock, c.SubjectDivision.SchoolClass)))
                    schoolClassOccupancy[(c.TimeBlock, c.SubjectDivision.SchoolClass)] += 1;
                else
                    schoolClassOccupancy[(c.TimeBlock, c.SubjectDivision.SchoolClass)] = 1;
            }
            var faults = schoolClassOccupancy.Where(x => x.Value > 1).Sum(x => x.Value - 1);
            return faults * PenaltyPoint.SchoolClassMoreThanOnePlaceAtTheSameTime;
        }

        //TimeBlock not in SubjectDivision.Teacher.FreeBlocks
        private double TeacherNotAvailable()
        {
            var faults = 0;
            foreach (var c in courses)
            {
                if (!c.SubjectDivision.Teacher.FreeBlocks.Any(tb => tb.Equals(c.TimeBlock)))
                    faults += 1;
            }

            return faults * PenaltyPoint.TeacherNotAvailable;
        }


        private double TeacherScheduleHoles()
        {
            //order by teachers and timeblocks
            var orderedCourses= courses.OrderBy(x => x.SubjectDivision.Teacher).ThenBy(x => x.TimeBlock).ToList();

            double sumFaults = 0;
            double actTeacherFaults = 0;

            for (int i = 1; i < orderedCourses.Count; i++)
            {
                //if the teacher is the same as at last course
                if (orderedCourses[i].SubjectDivision.Teacher.Equals(orderedCourses[i - 1].SubjectDivision.Teacher))
                {
                    // if last and current courses are on the same day
                    if (orderedCourses[i].TimeBlock.Day == orderedCourses[i - 1].TimeBlock.Day)
                    {
                        //count distance( x^2 if at least 3) of the last 2 lessonNumbers as faults - 1 is best
                        if ((orderedCourses[i].TimeBlock.LessonNumber - orderedCourses[i - 1].TimeBlock.LessonNumber - 1) >= 3)
                            actTeacherFaults += Math.Pow(orderedCourses[i].TimeBlock.LessonNumber - orderedCourses[i - 1].TimeBlock.LessonNumber - 1, 2);
                        else
                            actTeacherFaults += orderedCourses[i].TimeBlock.LessonNumber - orderedCourses[i - 1].TimeBlock.LessonNumber - 1;
                    }
                }
                else
                {
                    // square the faults per teacher to distribute courses evenly
                    sumFaults += Math.Pow(actTeacherFaults, 2);
                    actTeacherFaults = 0;
                }
            }
            return sumFaults * PenaltyPoint.TeacherScheduleHoles;
        }

        private double SubjectsNotDistributed()
        {
            //order by school classes and timeblocks
            var orderedCourses = courses.OrderBy(x => x.SubjectDivision.SchoolClass).ThenBy(x => x.TimeBlock).ToList();
            Dictionary<string, int> subjectOccupancy = new Dictionary<string, int>();
            //add element at index 0
            subjectOccupancy[orderedCourses[0].SubjectDivision.Subject.TrimEnd(digits)] = 1;

            double sumFaults = 0;
            double actSchoolClassFaults = 0;

            for (int i = 1; i < orderedCourses.Count; i++)
            {
                //if the school class is the same as at last course
                if (orderedCourses[i].SubjectDivision.SchoolClass.Equals(orderedCourses[i - 1].SubjectDivision.SchoolClass))
                {
                    // if last and current courses are on the same day
                    if (orderedCourses[i].TimeBlock.Day == orderedCourses[i - 1].TimeBlock.Day)
                    {
                        // if subject(trimmed number at the end) was already held on that day, increase the counter
                        if (subjectOccupancy.ContainsKey(orderedCourses[i].SubjectDivision.Subject.TrimEnd(digits)))
                            subjectOccupancy[orderedCourses[i].SubjectDivision.Subject.TrimEnd(digits)] += 1;
                        else
                            subjectOccupancy[orderedCourses[i].SubjectDivision.Subject.TrimEnd(digits)] = 1;
                    }
                    // else count the faults of the day. Faults are the excesses of each subjects. After clear the dictionary and add the first course
                    else
                    {
                        //TODO: make it exponential
                        actSchoolClassFaults += Math.Pow(subjectOccupancy.Where(x => x.Value > 1).Sum(x => x.Value - 1),2);
                        subjectOccupancy.Clear();
                        //add the first course on that day to the dictionary
                        subjectOccupancy[orderedCourses[i].SubjectDivision.Subject.TrimEnd(digits)] = 1;
                    }
                }
                else
                {
                    // square the faults per school class to distribute courses evenly
                    sumFaults += Math.Pow(actSchoolClassFaults, 2);
                    actSchoolClassFaults = 0;
                }
                
            }

            return sumFaults * PenaltyPoint.SubjectsNotDistributed;
        }

        private double DayStartsWithEasySubjects()
        {
            var faults = 0;
            foreach ( var c in courses)
            {
                if (easySubjects.Contains(c.SubjectDivision.Subject.TrimEnd(digits)) && c.TimeBlock.LessonNumber <= 2)
                    faults++;
            }
            return faults * PenaltyPoint.DayStartsWithEasySubjects;
        }
    }
}
