using DIPTERV.Context;
using DIPTERV.Data;
using Microsoft.EntityFrameworkCore;
namespace DIPTERV.Repositories
{
    public class TimeBlockRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _factory;

        public TimeBlockRepository(IDbContextFactory<ApplicationDbContext> factory)
        {
            _factory = factory;
        }

        public async Task<TimeBlock[]> GetAllTimeBlocksAsync()
        {
            using var context = _factory.CreateDbContext();
            return await context.TimeBlocks.ToArrayAsync();
        }

        public async Task InsertAllTimeBlocksAsync(TimeBlock[] timeBlocks)
        {
            using var context = _factory.CreateDbContext();

            context.TimeBlocks.AddRange(timeBlocks);

            await context.SaveChangesAsync();
        }

    }
}
