using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject.Repositories
{
    public class TeacherRepositoryTest
    {
        //private ApplicationDbContext mockApplicationDbContext;
        //private IDbContextFactory<ApplicationDbContext> _factory = new TestDbContextFactory();
        private MockRepository mockRepository;

        public TeacherRepositoryTest() 
        {
            mockRepository = new MockRepository(MockBehavior.Strict);
        }

        [Fact]
        public async Task Test_WrongID()
        {
            // Arrange
            var teacherRepository = this.CreateTeacherRepository();

            // Act

            // Assert

            // non-existent id
            Assert.Null(teacherRepository.GetTeacherByIdAsync(-1).AsyncState);
            mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Test_GoodGetMultiple()
        {
            // Arrange
            var teacherRepository = this.CreateTeacherRepository();

            Teacher teacher0 = new Teacher("Teszt Elek", new List<TimeBlock>() { new TimeBlock(Day.Monday, 1) });
            Teacher teacher1 = new Teacher("Teszt Elek2", new List<TimeBlock>() { new TimeBlock(Day.Monday, 2) });
            Teacher teacher2 = new Teacher("Teszt Elek3", new List<TimeBlock>() { new TimeBlock(Day.Monday, 3) });
            var teachers = new List<Teacher>() { teacher0, teacher1 };
            // Act
            await teacherRepository.InsertAllTeacherAsync(teachers.ToArray());
            await teacherRepository.InsertTeacherAsync(teacher2);

            // Assert

            // non-existent id
            teacher0. Equals(teacherRepository.GetAllTeacherAsync().Result[0]);
            Assert.Equal<Teacher>(teacher0, teacherRepository.GetAllTeacherAsync().Result[0]);
            Assert.Equal<Teacher>(teacher1, teacherRepository.GetAllTeacherAsync().Result[1]);
            Assert.Equal<Teacher>(teacher2, teacherRepository.GetTeacherByIdAsync(2).Result);
            Teacher.nextId = 0;
            TimeBlock.nextId = 0;
        }



        private TeacherRepository CreateTeacherRepository()
        {
            return new TeacherRepository(
                new TestDbContextFactory());
        }

    }
}
