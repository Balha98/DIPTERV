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

        public async Task<SubjectDivision[]> GetAllSubjectDivisionsAsync()
        {
            using var context = _factory.CreateDbContext();
            return await context.SubjectDivisions.Include(sd => sd.SchoolClass).Include(sd => sd.Teacher).ToArrayAsync();
        }

        public async Task<SubjectDivision> GetSubjectDivisionByIDAsync(int id)
        {
            using var context = _factory.CreateDbContext();
            return await context.SubjectDivisions.Include(sd => sd.Teacher).Include(sd => sd.Teacher.FreeBlocks).Include(sd => sd.SchoolClass).Where(sd => sd.ID == id).FirstOrDefaultAsync();
        }

        public async Task<SubjectDivision[]> GetSubjectDivisionbyIDsAsync(int[] ids)
        {
            using var context = _factory.CreateDbContext();
            return await context.SubjectDivisions.Include(sd => sd.Teacher).Where(sd => ids.Contains(sd.ID)).ToArrayAsync();
        }

        public async Task InsertAllSubjectDivisionAsync(SubjectDivision[] subjectDivisions)
        {
            using var context = _factory.CreateDbContext();

            context.SubjectDivisions.AddRange(subjectDivisions);

            await context.SaveChangesAsync();
        }
    }
}

