using System.Linq;
using DataAccessLayer.Core.EntityFramework.Repositories;
using DataAccessLayer.Core.Interfaces.Repositories;
using DataAccessLayer.Core.Tests.TestModels;
using Xunit;

namespace DataAccessLayer.Core.Tests.Tests
{
    public class RepositoryReadTests
    {
        private readonly IRepository<Product> productRepo;
        private readonly IRepository<Order> orderRepo;
        private readonly IRepository<Client> clientRepo;
        private readonly IRepository<Address> addresses;
        public RepositoryReadTests()
        {
            var context = new TestDbContext();
            productRepo = new EntityFrameworkRepository<Product>(context);
            orderRepo = new EntityFrameworkRepository<Order>(context);
            clientRepo = new EntityFrameworkRepository<Client>(context);
            addresses = new EntityFrameworkRepository<Address>(context);
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
            Assert.True(client != null && client.Orders != null);
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
            Assert.True(products.FirstOrDefault().Order != null);
        }

        [Fact]
        public void GetProductsWithProducerAndAddress()
        {
            var product = productRepo.Get(1, enableTracking: false, tablePredicate: new System.Linq.Expressions.Expression<System.Func<Product, object>>[] { p => p.Order, p => p.Producer.Address});
            Assert.True(product.Producer.Address != null);
        }

    }
}
