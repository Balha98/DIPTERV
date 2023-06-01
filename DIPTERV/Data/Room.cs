using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DIPTERV.Data
{
    public class Room : IEquatable<Room>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        public static int nextId = 0;
        public string Name { get; set; }

        public string? Subject { get; set; }

        public Room(string name)
        {
            Name = name;
            ID = nextId++;
        }

        public Room(string name, string subject)
        {
            Name = name;
            Subject = subject;
            ID = nextId++;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Room);
        }

        public bool Equals(Room other)
        {
            return other is not null &&
                   ID == other.ID &&
                   Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ID, Name);
        }
    }
}
