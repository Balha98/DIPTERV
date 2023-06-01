using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIPTERV.Data
{
    public enum Day
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday
    }


    public class TimeBlock : IEquatable<TimeBlock>, IComparable<TimeBlock>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        public static int nextId = 0;
        public Day Day { get; set; }
        public int LessonNumber { get; set; }

        public TimeBlock(Day day, int lessonNumber)
        {
            Day = day;
            LessonNumber = lessonNumber;
            ID = nextId++;
        }
        int IComparable<TimeBlock>.CompareTo(TimeBlock other)
        {
            int res = Day.CompareTo(other.Day);

            if (res == 0)
                res = LessonNumber.CompareTo(other.LessonNumber);
            return res;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TimeBlock);
        }

        public bool Equals(TimeBlock other)
        {
            return other is not null &&
                    ID == other.ID &&
                  Day == other.Day &&
                   LessonNumber == other.LessonNumber;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ID, Day, LessonNumber);
        }
    }
}
