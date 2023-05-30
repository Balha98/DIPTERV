using DIPTERV.Data;
using DIPTERV.Repositories;

namespace DIPTERV.Services
{
    public class FreeBlockService
    {
        private readonly TimeBlockRepository _timeBlockRepository;
        private readonly TeacherRepository _teacherRepository;

        public FreeBlockService(TimeBlockRepository timeBlockRepository, TeacherRepository teacherRepository)
        {
            _timeBlockRepository = timeBlockRepository;
            _teacherRepository = teacherRepository;
        }

        public async Task<int[]> GetFreeTimeBlockIDsByTeacherIDAsync(int teacherId)
        {
            var teacher = await _teacherRepository.GetTeacherByIdAsync(teacherId);
            return teacher.FreeBlocks.Select(tb => tb.ID).ToArray();
        }
        public async Task<TimeBlock[]> GetTimeBlocksAsync()
        {
            return await _timeBlockRepository.GetAllTimeBlocksAsync();
        }
    }
}
