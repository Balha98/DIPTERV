using DIPTERV.Data;
using DIPTERV.Repositories;

namespace DIPTERV.Services
{
    public class FreeBlockService
    {
        private readonly TimeBlockRepository _timeBlockRepo;
        private readonly TeacherRepository _teacherRepo;

        public FreeBlockService(TimeBlockRepository timeBlockRepository, TeacherRepository teacherRepository)
        {
            _timeBlockRepo = timeBlockRepository;
            _teacherRepo = teacherRepository;
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
