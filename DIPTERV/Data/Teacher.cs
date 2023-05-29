using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DIPTERV.Data
{
    public class Teacher : IComparable<Teacher>, IEquatable<Teacher> 
    {
        [Key]
        public int ID { get; set; }
        static int nextId = 0;
        public string Name { get; set; }
        public int CourseNumber { get; set; } = 0;

        public ICollection<TimeBlock> FreeBlocks { get; set; }

        public Teacher(string name, ICollection<TimeBlock> freeBlocks) : this(name)
        {
            FreeBlocks = freeBlocks;
            ID = nextId++;
        }

        /// <summary>
        /// EF constructor
        /// </summary>
        private Teacher(string name)
        {
            Name = name;
        }

        int IComparable<Teacher>.CompareTo(Teacher other)
        {
            return ID - other.ID;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Teacher);
        }

        public bool Equals(Teacher other)
        {
            return other is not null &&
                   ID == other.ID &&
                   Name == other.Name &&
                   CourseNumber == other.CourseNumber &&
                   EqualityComparer<ICollection<TimeBlock>>.Default.Equals(FreeBlocks, other.FreeBlocks);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ID, Name, CourseNumber, FreeBlocks);
        }
    }
}
