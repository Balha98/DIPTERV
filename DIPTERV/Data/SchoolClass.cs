using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIPTERV.Data
{
    public class SchoolClass : IComparable<SchoolClass>, IEquatable<SchoolClass>
    {
        [Key]
        public int ID { get; set; }
        static int nextId = 0;
        public string Name { get; set; }

        public int HeadMasterId { get; set; }
        [ForeignKey("HeadMasterId")]
        public Teacher HeadMaster { get; set; }


        public SchoolClass(string name, Teacher headMaster) : this(name)
        {
            HeadMaster = headMaster;
            HeadMasterId = headMaster.ID;
            ID = nextId++;
        }

        /// <summary>
        /// EF constructor
        /// </summary>
        private SchoolClass(string name)
        {
            Name = name;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SchoolClass);
        }

        public bool Equals(SchoolClass other)
        {
            return other is not null &&
                   ID == other.ID &&
                   Name == other.Name &&
                   EqualityComparer<Teacher>.Default.Equals(HeadMaster, other.HeadMaster);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ID, Name, HeadMaster);
        }

        int IComparable<SchoolClass>.CompareTo(SchoolClass other)
        {
            return ID - other.ID;
        }
    }
}
