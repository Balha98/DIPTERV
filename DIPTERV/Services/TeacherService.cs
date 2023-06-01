using DIPTERV.Data;
using DIPTERV.Repositories;

namespace DIPTERV.Services
{
    public class TeacherService
    {
        private readonly TeacherRepository _repo;

        public TeacherService(TeacherRepository repository)
        {
            _repo = repository;
        }

        public async Task<Teacher> GetTeacherByIdAsync(int id)
        {
            return await _repo.GetTeacherByIdAsync(id);
        }

        public async Task<Teacher[]> GetAllTeachersAsync()
        {
            return await _repo.GetAllTeacherAsync();
        }
        public async Task UpdateTeacherAsync(Teacher teacher)
        {
            await _repo.UpdateTeacherAsync(teacher);
        }
    }
}
