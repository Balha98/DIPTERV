using DIPTERV.Data;
using DIPTERV.Pages;
using DIPTERV.Repositories;
using System.Linq;

namespace DIPTERV.Services
{
    public class TimetableService
    {
        private readonly TeacherRepository _teacherRepo;
        private readonly TimeBlockRepository _timeBlockRepo;
        private readonly CourseRepository _courseRepo;
        private readonly SubjectDivisionRepository _subjectDivisionRepo;


        public TimetableService(CourseRepository courseRepository, TimeBlockRepository timeBlockRepository, TeacherRepository teacherRepository, SubjectDivisionRepository subjectDivisionRepository)
        {
            _courseRepo = courseRepository;
            _timeBlockRepo = timeBlockRepository;
            _teacherRepo = teacherRepository;
            _subjectDivisionRepo = subjectDivisionRepository;
        }
        public async Task<Course[]> GetCoursesByTeacherIDAsync(int teacherId)
        {
            var courses = await _courseRepo.GetAllCoursesAsync();
            var subjectDivisionIds = courses.Select(c => c.SubjectDivisinId).ToArray();
            var subjectDivisions = await _subjectDivisionRepo.GetSubjectDivisionbyIDsAsync(subjectDivisionIds);
            var correctSubjectDivisions = subjectDivisions.Where(sd => sd.TeacherId == teacherId).Select(sd => sd.ID).ToArray();
            return courses.Where(c => correctSubjectDivisions.Contains(c.SubjectDivisinId)).ToArray();
        }

        public async Task<Course[]> GetCoursesBySchoolClassIDAsync(int schoolClassId)
        {
            var courses = await _courseRepo.GetAllCoursesAsync();
            return courses.Where(c => c.SubjectDivision.SchoolClassId == schoolClassId).ToArray();
        }

        public async Task<Course[]> GetCoursesByRoomIDAsync(int roomId)
        {
            var courses = await _courseRepo.GetAllCoursesAsync();
            return courses.Where(c => c.RoomId == roomId).ToArray();
        }


        public async Task<int[]> GetFreeTimeBlockIDsByTeacherIDAsync(int teacherId)
        {
            var teacher = await _teacherRepo.GetTeacherByIdAsync(teacherId);
            return teacher.FreeBlocks.Select(tb => tb.ID).ToArray();
        }
        public async Task<TimeBlock[]> GetTimeBlocksAsync()
        {
            return await _timeBlockRepo.GetAllTimeBlocksAsync();
        }
    }
}
