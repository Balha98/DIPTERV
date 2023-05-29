using DIPTERV.Data;
using DIPTERV.Repositories;

namespace DIPTERV.Services
{
    public class TeacherService
    {
        private readonly TeacherRepository _repository;

        public TeacherService(TeacherRepository repository)
        {
            _repository = repository;
        }

        public async Task<Teacher> GetTeacherByIdAsync(int id)
        {
            return await _repository.GetTeacherByIdAsync(id);
        }
        public async Task<Teacher[]> GetTeachersAsync()
        {
            return await _repository.GetAllTeacherAsync();
        }
    }
}
