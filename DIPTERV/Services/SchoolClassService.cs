using DIPTERV.Data;
using DIPTERV.Repositories;

namespace DIPTERV.Services
{
    public class SchoolClassService
    {
        private readonly SchoolClassRepository _repo;

        public SchoolClassService(SchoolClassRepository repository)
        {
            _repo = repository;
        }

        public async Task<SchoolClass[]> GetAllSchoolClassesAsync()
        {
            return await _repo.GetAllSchoolClassesAsync();
        }

    }
}
