using AnyCompany.Core.Query.Domain;
using AnyCompany.Core.Query.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace AnyCompany.Core.Tests.Query.Repositories
{
    public class CustomerQueryRepositoryTests
    {
        private CustomerQueryRepository customerQueryRepository;
        private DbContextOptions<AnyCompanyDbContext> options;

        public CustomerQueryRepositoryTests()
        {
            // Set up the in Memory settings
            options = new DbContextOptionsBuilder<AnyCompanyDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Using a Guid recreates the Db every test
                .Options;

            customerQueryRepository = new CustomerQueryRepository();
        }
        
        [Fact]
        public void GivenNoCustomers_WhenILoadAllCustomers_ThenEmptyListIsReturned()
        {
            using (new AnyCompanyDbContext(options))
            {
                var result = customerQueryRepository.LoadAll();
                Assert.NotNull(result);
                Assert.Empty(result);
            }
        }

        [Fact]
        public void GivenCustomersWithNoOrders_WhenILoadAllCustomers_ThenCustomerIsLoadedWithEmptyOrderList()
        {
            CreateCustomers(3, 0);

            using (new AnyCompanyDbContext(options))
            {
                var result = customerQueryRepository.LoadAll();

                Assert.NotNull(result);
                Assert.Equal(3, result.Count());
            }
        }

        [Fact]
        public void GivenMultipleCustomersAndOrders_WhenILoadAllCustomers_ThenCustomersAndOrdersAreLoaded()
        {
            CreateCustomers(3, 5);

            using (new AnyCompanyDbContext(options))
            {
                var result = customerQueryRepository.LoadAll();

                Assert.NotNull(result);
                Assert.Equal(3, result.Count());
                foreach (var customer in result)
                {
                    Assert.Equal(5, customer.Orders.Count());
                }
            }
        }

        private OrderDTO CreateOrder()
        {
            return new OrderDTO();
        }

        private CustomerDTO CreateCustomer(AnyCompanyDbContext context, int numberOfOrders)
        {
            var customer = new CustomerDTO();

            for(int i = 0; i < numberOfOrders; i++)
            {
                var order = CreateOrder();
                order.Customer = customer;
                context.Add(order);
            }
            return customer;
        }

        /// <summary>
        /// Note: I had hoped to create customers using the Customer Command object instead of the DTO here, 
        /// Which would test that they actually point at the same tables. 
        /// Turns out the InMemory Db does not support the [Table] attribute. 
        /// This would be something to look at in the future.
        /// </summary>
        /// <param name="numberOfCustomers"></param>
        /// <param name="numberOfOrders"></param>
        private void CreateCustomers(int numberOfCustomers, int numberOfOrders)
        {
            using (var context = new AnyCompanyDbContext(options))
            {
                for (int i = 0; i < numberOfCustomers; i++)
                {
                    var customer = CreateCustomer(context, numberOfOrders);
                    context.Add(customer);
                }

                context.SaveChanges();
            }
        }
    }
}
