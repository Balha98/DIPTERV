using DIPTERV.Context;
using DIPTERV.Data;
using Microsoft.EntityFrameworkCore;
namespace DIPTERV.Repositories
{
    public class RoomRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _factory;

        public RoomRepository(IDbContextFactory<ApplicationDbContext> factory)
        {
            _factory = factory;
        }

        public async Task<Room[]> GetAllRoomsAsync()
        {
            using var context = _factory.CreateDbContext();
            return await context.Rooms.ToArrayAsync();
        }

        public async Task InsertAllRoomsAsync(Room[] rooms)
        {
            using var context = _factory.CreateDbContext();

            context.Rooms.AddRange(rooms);

            await context.SaveChangesAsync();
        }
    }
}
