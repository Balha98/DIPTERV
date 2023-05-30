using DIPTERV.Data;
using DIPTERV.Pages;
using GeneticSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIPTERV.Logic
{
    public class ScheduleMutation : MutationBase
    {
        private List<SubjectDivision> _subjectDivisions;
        private List<TimeBlock> _timeBlocks;
        private List<Teacher> _teachers;

        public ScheduleMutation(List<SubjectDivision> subjectDivisions, List<TimeBlock> timeBlocks, List<Teacher> teachers)
        {
            _subjectDivisions = subjectDivisions;
            _timeBlocks = timeBlocks;
            _teachers = teachers;
        }

        protected override void PerformMutate(IChromosome chromosome, float probability)
        {
            ExceptionHelper.ThrowIfNull("chromosome", chromosome);


            //random Timeblock-ban minden SchoolClassból csak egy legyen
            //SchoolClassMoreThanOnePlaceAtTheSameTime csökkentésére
            if (RandomizationProvider.Current.GetDouble() <= probability)
            {
                var RandomTimeBlock = _timeBlocks[RandomizationProvider.Current.GetInt(0, _timeBlocks.Count)];
                var courses = new List<Course>();

                for (int i = 0; i < _subjectDivisions.Count; i++)
                    courses.Add((Course)chromosome.GetGene(i).Value);
                var counter = new Dictionary<SchoolClass, int>();
                foreach (var c in courses.Where(x => x.TimeBlock.Equals(RandomTimeBlock)))
                {
                    if (counter.ContainsKey(c.SubjectDivision.SchoolClass))
                        //TODO olyat kapjon, ahol még nincs az adott osztálynak foglalt timeblockja
                        c.TimeBlock = _timeBlocks[RandomizationProvider.Current.GetInt(0, _timeBlocks.Count)];
                    else
                        counter[c.SubjectDivision.SchoolClass] = 1;
                }
                //chromosome.ReplaceGene(geneIndex, chromosome.GenerateGene(geneIndex));
            }

            // MoreThanOneTimeBlockinaRoomAtTheSameTime csökkentésére
            // random Timeblock-ban minden Room-ból csak egy legyen
            if (RandomizationProvider.Current.GetDouble() <= probability)
            {
                var RandomTimeBlock = _timeBlocks[RandomizationProvider.Current.GetInt(0, _timeBlocks.Count)];
                var courses = new List<Course>();

                for (int i = 0; i < _subjectDivisions.Count; i++)
                    courses.Add((Course)chromosome.GetGene(i).Value);
                var counter = new Dictionary<Room, int>();
                foreach (var c in courses.Where(x => x.TimeBlock.Equals(RandomTimeBlock)))
                {
                    if (counter.ContainsKey(c.Room))
                        //TODO olyat kapjon, ahol még nincs az adott teremnek foglalt timeblockja
                        c.TimeBlock = _timeBlocks[RandomizationProvider.Current.GetInt(0, _timeBlocks.Count)];
                    else
                        counter[c.Room] = 1;
                }
            }

            // TeacherMoreThanOnePlaceAtTheSameTime csökkentésére
            // random Timeblock-ban minden Teacher-ből csak egy legyen
            if (RandomizationProvider.Current.GetDouble() <= probability *2)
            {
                var RandomTimeBlock = _timeBlocks[RandomizationProvider.Current.GetInt(0, _timeBlocks.Count)];
                var courses = new List<Course>();

                for (int i = 0; i < _subjectDivisions.Count; i++)
                    courses.Add((Course)chromosome.GetGene(i).Value);
                var counter = new Dictionary<Teacher, int>();
                foreach (var c in courses.Where(x => x.TimeBlock.Equals(RandomTimeBlock)))
                {
                    if (counter.ContainsKey(c.SubjectDivision.Teacher))
                        //TODO olyat kapjon, ahol még nincs az adott tanárnak foglalt timeblockja
                        c.TimeBlock = _timeBlocks[RandomizationProvider.Current.GetInt(0, _timeBlocks.Count)];
                    else
                        counter[c.SubjectDivision.Teacher] = 1;
                }
            }

            // TeacherNotAvailable csökkentésére
            // random Tanáron végigmenni, ha nem ér rá, új időblokk
            
            if (RandomizationProvider.Current.GetDouble() <= probability)
            {
                var RandomTeacher = _teachers[RandomizationProvider.Current.GetInt(0, _teachers.Count)];
                var courses = new List<Course>();

                for (int i = 0; i < _subjectDivisions.Count; i++)
                    courses.Add((Course)chromosome.GetGene(i).Value);
                foreach (var c in courses.Where(x => x.SubjectDivision.Teacher.Equals(RandomTeacher)))
                {
                    if (!c.SubjectDivision.Teacher.FreeBlocks.Any(tb => tb.Equals(c.TimeBlock)))
                        c.TimeBlock = c.SubjectDivision.Teacher.FreeBlocks.ToList()[RandomizationProvider.Current.GetInt(0, c.SubjectDivision.Teacher.FreeBlocks.Count)];
                }
            }
            
        }
    }
}
