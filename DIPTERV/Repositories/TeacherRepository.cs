using DIPTERV.Context;
using DIPTERV.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace DIPTERV.Repositories
{
    public class TeacherRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _factory;
        public TeacherRepository(IDbContextFactory<ApplicationDbContext> factory) 
        { 
            _factory = factory;
        }

        public async Task<Teacher> GetTeacherByIdAsync(int id)
        {
            using var context = _factory.CreateDbContext();
            return await context.Teachers.FirstOrDefaultAsync(t => t.ID == id);
        }

        public async Task<Teacher[]> GetAllTeacherAsync()
        {
            using var context = _factory.CreateDbContext();
            return await context.Teachers.ToArrayAsync();
        }
    }
}
