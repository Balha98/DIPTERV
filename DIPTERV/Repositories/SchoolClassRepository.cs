using DIPTERV.Context;
using DIPTERV.Data;
using Microsoft.EntityFrameworkCore;
namespace DIPTERV.Repositories
{
    public class SchoolClassRepository
    {

        private readonly IDbContextFactory<ApplicationDbContext> _factory;
        public SchoolClassRepository(IDbContextFactory<ApplicationDbContext> factory)
        {
            _factory = factory;
        }

        public async Task<SchoolClass[]> GetAllSchoolClassesAsync()
        {
            using var context = _factory.CreateDbContext();
            return await context.SchoolClasses.Include(sc => sc.HeadMaster).OrderBy(sc => sc.Name).ToArrayAsync();
        }

        public async Task InsertAllSchoolClassesAsync(SchoolClass[] schoolClasses)
        {
            using var context = _factory.CreateDbContext();

            context.SchoolClasses.AddRange(schoolClasses);

            await context.SaveChangesAsync();
        }
    }
}
