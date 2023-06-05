using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject.Repositories
{
    public  class TestDbContextFactory : IDbContextFactory<ApplicationDbContext>
    {
        private DbContextOptions<ApplicationDbContext> _options;

        public TestDbContextFactory(string databaseName = "InMemoryTest")
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName)
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
        }

        public ApplicationDbContext CreateDbContext()
        {
            return new ApplicationDbContext(_options);
        }
    }
}
