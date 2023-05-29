using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Immutable;

namespace DIPTERV.Data
{
    public class SubjectDivision
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        static int nextId = 0;

        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public string Subject { get; set; }

        public int SchoolClassId { get; set; }
        [ForeignKey("SchoolClassId")]
        public SchoolClass SchoolClass { get; set; }

        public SubjectDivision(Teacher teacher, string subject, SchoolClass schoolclass) : this(subject)
        {
            Teacher = teacher;
            TeacherId = teacher.ID;
            SchoolClass = schoolclass;
            SchoolClassId = schoolclass.ID;
            ID = nextId++;
        }


        /// <summary>
        /// EF constructor
        /// </summary>
        private SubjectDivision(string subject)
        {
            Subject = subject;
        }
    }
}
