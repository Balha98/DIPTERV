using DIPTERV.Context;
using DIPTERV.Data;
using Microsoft.EntityFrameworkCore;
namespace DIPTERV.Repositories
{
    public class CourseRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _factory;

        public CourseRepository(IDbContextFactory<ApplicationDbContext> factory)
        {
            _factory = factory;
        }

        public async Task<Course[]> GetAllCoursesAsync()
        {
            using var context = _factory.CreateDbContext();
            return await context.Courses.ToArrayAsync();
        }

        public async Task InsertAllCoursesAsync(Course[] courses)
        {
            using var context = _factory.CreateDbContext();

            context.Courses.AddRange(courses);

            await context.SaveChangesAsync();
        }
    }
}
