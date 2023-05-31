using DIPTERV.Data;
using DIPTERV.Repositories;

namespace DIPTERV.Services
{
    public class SubjectDivisionService
    {
        private readonly SubjectDivisionRepository _repo;

        public SubjectDivisionService(SubjectDivisionRepository repository)
        {
            _repo = repository;
        }

        public async Task<SubjectDivision> GetSubjectDivisionByIdAsync(int id)
        {
            return await _repo.GetSubjectDivisionByIDAsync(id);
        }

        public async Task<SubjectDivision[]> GetAllSubjectDivisionsAsync()
        {
            return await _repo.GetAllSubjectDivisionsAsync();
        }

    }
}
