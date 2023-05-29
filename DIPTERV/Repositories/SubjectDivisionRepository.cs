using DIPTERV.Context;
using DIPTERV.Data;
using Microsoft.EntityFrameworkCore;

namespace DIPTERV.Repositories
{
    public class SubjectDivisionRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _factory;
        public SubjectDivisionRepository(IDbContextFactory<ApplicationDbContext> factory)
        {
            _factory = factory;
        }

        public async Task<SubjectDivision[]> GetAllSubjectDivisionAsync()
        {
            using var context = _factory.CreateDbContext();
            return await context.SubjectDivisions.ToArrayAsync();
        }

        public async Task InsertAllSubjectDivisionAsync(SubjectDivision[] subjectDivisions)
        {
            using var context = _factory.CreateDbContext();

            context.SubjectDivisions.AddRange(subjectDivisions);

            await context.SaveChangesAsync();
        }
    }
}

