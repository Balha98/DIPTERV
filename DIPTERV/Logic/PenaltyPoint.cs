using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIPTERV.Logic
{
    internal class PenaltyPoint
    {

        public static double MoreThanOneSubjectDivison { get; set; } = 1000000;
        public static double SubjectDivisionMissing { get; set; } = 1000000;
        public static double MoreThanOneTimeBlockinaRoomAtTheSameTime { get; set; } = 100000;
        public static double TeacherMoreThanOnePlaceAtTheSameTime { get; set; } = 100000;
        public static double SchoolClassMoreThanOnePlaceAtTheSameTime { get; set; } = 100000;
        public static double TeacherNotAvailable { get; set; } = 100000;
        public static double TeacherScheduleHoles { get; set; } = 5;
        public static double SubjectsNotDistributed { get; set; } = 1;
        public static double DayStartsWithEasySubjects { get; set; } = 100;

    }
}
