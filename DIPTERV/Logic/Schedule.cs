using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DIPTERV.Data;
using GeneticSharp;
using static OfficeOpenXml.ExcelErrorValue;

namespace DIPTERV.Logic
{
    public class Schedule : ChromosomeBase
    {

        //public List<Course> Courses { get; set; }
        readonly Random rand = new Random();
        private List<Room> _rooms;
        private List<SubjectDivision> _subjectDivisions;
        private List<TimeBlock> _timeBlocks;

        /*
        public Schedule()
            : base(DataHandler.SubjectDivisions.Count)
        {
            //Courses.Clear();
            //DataHandler.SubjectDivisions = DataHandler.SubjectDivisions.OrderBy(_ => rand.Next()).ToList();
            CreateGenes();
        }
        */

        public Schedule(List<Room> rooms, List<SubjectDivision> subjectDivisions, List<TimeBlock> timeBlocks)
            : base(subjectDivisions.Count)
        {
            _rooms = rooms;
            _subjectDivisions = subjectDivisions;
            _timeBlocks = timeBlocks;
            CreateGenes();
        }

        public override Gene GenerateGene(int geneIndex)
        {
            Course course = new Course
            {
                ID = geneIndex,
                SubjectDivision = _subjectDivisions[geneIndex],
                TimeBlock = _timeBlocks[rand.Next(0, _timeBlocks.Count)],
                Room = _rooms[rand.Next(0, _rooms.Count)],
            };
            course.SubjectDivisinId = course.SubjectDivision.ID;
            course.TimeBlockId = course.TimeBlock.ID;
            course.RoomId = course.Room.ID;

            return new Gene(course);
        }

        public override IChromosome CreateNew()
        {
            return new Schedule(_rooms, _subjectDivisions, _timeBlocks);
        }

        public override IChromosome Clone()
        {
            return base.Clone();
        }
    }
}
