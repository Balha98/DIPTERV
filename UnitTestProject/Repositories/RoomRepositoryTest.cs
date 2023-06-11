using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject.Repositories
{
  public class RoomRepositoryTest
  {
    //private ApplicationDbContext mockApplicationDbContext;
    //private IDbContextFactory<ApplicationDbContext> _factory = new TestDbContextFactory();
    private MockRepository mockRepository;

    public RoomRepositoryTest()
    {
      mockRepository = new MockRepository(MockBehavior.Strict);
    }

    [Fact]
    public async Task Test_WrongID()
    {
      // Arrange
      var roomRepository = this.CreateRoomRepository();

      // Act

      // Assert

      // non-existent id
      Assert.Null(roomRepository.GetRoomByIdAsync(-1).AsyncState);
      mockRepository.VerifyAll();
    }

    [Fact]
    public async Task Test_GoodGetMultiple()
    {
      // Arrange
      var roomRepository = this.CreateRoomRepository();

      Room room0 = new Room("Szoba1");
      Room room1 = new Room("Szoba2","matek" );
      Room room2 = new Room("Szoba3", "magyar");
      var rooms = new List<Room>() { room0, room1, room2 };
      int[] testids = new int[] {room0.ID, room1.ID, room2.ID };
      // Act
      await roomRepository.InsertAllRoomsAsync(rooms.ToArray());

      // Assert

      Assert.Equal<Room>(room0,roomRepository.GetRoomByIdAsync(0).Result);
      Assert.Equal<Room>(room1, roomRepository.GetAllRoomsAsync().Result[1]);
      Assert.Equal<Room>(room2, roomRepository.GetRoombyIDsAsync(testids).Result[2]);
      Room.nextId = 0;
    }



    private RoomRepository CreateRoomRepository()
    {
      return new RoomRepository(
          new TestDbContextFactory());
    }

  }
}
