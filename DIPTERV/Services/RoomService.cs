using DIPTERV.Data;
using DIPTERV.Repositories;

namespace DIPTERV.Services
{
    public class RoomService
    {
        private readonly RoomRepository _repo;

        public RoomService(RoomRepository repository)
        {
            _repo = repository;
        }

        public async Task<Room> GetRoomByIdAsync(int id)
        {
            return await _repo.GetRoomByIdAsync(id);
        }

        public async Task<Room[]> GetAllRoomsAsync()
        {
            return await _repo.GetAllRoomsAsync();
        }

    }
}
