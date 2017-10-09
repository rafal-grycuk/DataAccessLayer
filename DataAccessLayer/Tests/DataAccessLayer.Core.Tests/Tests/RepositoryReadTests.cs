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
        public void GetClientByIdTest()
        {
            var client= clientRepo.Get(1, false, c => c.Orders.Select(x => x.Products.Select(pr => pr.Producer)));
            Assert.True(client != null);
        }

        [Fact]
        public void GeClienttByPredicateTest()
        {

            var client = clientRepo.Get(x => x.Id == 1, false,
                c => c.Orders.Select(x => x.Products.Select(pr => pr.Producer)));
            Assert.True(client != null);
        }

        [Fact]
        public void GetClientRangeTest()
        {

            var clients = clientRepo.GetRange(enableTracking: false, tablePredicate: c => c.Orders.Select(x => x.Products.Select(pr => pr.Producer)));

            var clients2 = clientRepo.GetRange(enableTracking: false, tablePredicate: c => c.Orders.Select(x => x.Products));
            Assert.True(clients != null && clients.Any() && clients.FirstOrDefault().Orders != null);
            Assert.True(clients2 != null && clients2.Any() && clients2.FirstOrDefault().Orders != null);
        }

        [Fact]
        public void GetProductRangeTest()
        {
            var products = productRepo.GetRange(enableTracking: false, tablePredicate: p => p.Order);
          
        }

    }
}
