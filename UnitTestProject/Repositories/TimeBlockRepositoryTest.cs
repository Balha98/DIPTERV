using DIPTERV.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject.Repositories
{
  public class TimeBlockRepositoryTest
  {
    //private ApplicationDbContext mockApplicationDbContext;
    //private IDbContextFactory<ApplicationDbContext> _factory = new TestDbContextFactory();
    private MockRepository mockRepository;

    public TimeBlockRepositoryTest()
    {
      mockRepository = new MockRepository(MockBehavior.Strict);
    }


    [Fact]
    public async Task Test_GoodGetMultiple()
    {
      // Arrange
      var timeBlockRepository = this.CreateTimeBlockRepository();

      TimeBlock tb0 = new TimeBlock(Day.Monday,0);
      TimeBlock tb1 = new TimeBlock(Day.Tuesday, 1);
      var tbs = new List<TimeBlock>() { tb0, tb1 };
      // Act
      await timeBlockRepository.InsertAllTimeBlocksAsync(tbs.ToArray());

      // Assert

      Assert.Equal<TimeBlock>(tb0, timeBlockRepository.GetAllTimeBlocksAsync().Result[0]);
      TimeBlock.nextId = 0;
    }



    private TimeBlockRepository CreateTimeBlockRepository()
    {
      return new TimeBlockRepository(
          new TestDbContextFactory());
    }

  }
}
