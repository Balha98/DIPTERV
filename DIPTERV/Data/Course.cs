using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIPTERV.Data
{

    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        public static int nextId = 0;

        public int RoomId { get; set; }
        [NotMapped]
        public Room Room { get; set; }

        public int SubjectDivisinId { get; set; }
        [NotMapped]
        public SubjectDivision SubjectDivision { get; set; }

        public int TimeBlockId { get; set; }
        [NotMapped]
        public TimeBlock TimeBlock { get; set; }

        public Course(Room room, SubjectDivision sd, TimeBlock tb)
        {
            Room = room;
            RoomId = room.ID;
            SubjectDivision = sd;
            SubjectDivisinId = sd.ID;
            TimeBlock = tb;
            TimeBlockId = tb.ID;
            ID = nextId++;
        }
        
        public Course() { }

    }
}
