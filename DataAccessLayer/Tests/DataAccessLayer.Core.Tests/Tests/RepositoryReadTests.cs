using System.Linq;
using DataAccessLayer.Core.EntityFramework.Repositories;
using DataAccessLayer.Core.Interfaces.Infrastructure;
using DataAccessLayer.Core.Interfaces.Repositories;
using DataAccessLayer.Core.Tests.TestModels;
using Xunit;

namespace DataAccessLayer.Core.Tests.Tests
{
    public class RepositoryReadTests
    {
        private IRepository<Product> productRepo;
        private IRepository<Order> orderRepo;
        private IRepository<Client> clientRepo;

        public RepositoryReadTests()
        {
            var context = new TestDbContext();
            productRepo = new EntityFrameworkRepository<Product>(context);
            orderRepo = new EntityFrameworkRepository<Order>(context);
            clientRepo = new EntityFrameworkRepository<Client>(context);
            Seed.SeedData(context);
        }

        [Fact]
        public void GetByIdTest()
        {
            var client= clientRepo.Get(1, false, c => c.Orders.Select(x => x.Products.Select(pr => pr.Producer)));
            Assert.True(client != null);
        }

        [Fact]
        public void GetByPredicateTest()
        {

            var client = clientRepo.Get(x => x.Id == 1, false,
                c => c.Orders.Select(x => x.Products.Select(pr => pr.Producer)));
            Assert.True(client != null);
        }

        [Fact]
        public void GetRangeTest()
        {

            var clients = clientRepo.GetRange(null, false, null,
                c => c.Orders.Select(x => x.Products.Select(pr => pr.Producer)));
            Assert.True(clients != null && clients.Any() && clients.FirstOrDefault().Orders !=null);

        }
    }
}
