using AnyCompany.Core.Command.Domain;
using AnyCompany.Core.Command.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace AnyCompany.Core.Tests.Command.Services
{
    public class OrderServicesTests
    {
        private OrderService OrderService;
        private DbContextOptions<AnyCompanyDbContext> options;
        private int _defaultCustomerId = 1;

        public OrderServicesTests()
        {
            // Set up the in Memory settings
            options = new DbContextOptionsBuilder<AnyCompanyDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Using a Guid recreates the Db every test
                .Options;

            OrderService = new OrderService();
        }

        [Fact]
        public void GivenNoOrder_WhenIPlaceOrder_ThenOrderFails()
        {
            CreateDefaultCustomer();

            using (new AnyCompanyDbContext(options))
            {
                var result = OrderService.PlaceOrder(null, _defaultCustomerId);

                Assert.False(result);
            }

            ValidateNoOrderCreated();
        }

        [Fact]
        public void GivenNoCustomer_WhenIPlaceOrder_ThenOrderFails()
        {
            using (new AnyCompanyDbContext(options))
            {
                var result = OrderService.PlaceOrder(null, _defaultCustomerId);

                Assert.False(result);
            }

            ValidateNoOrderCreated();
        }

        [Fact]
        public void GivenValidOrderAndCustomer_WhenIPlaceOrder_ThenAnOrderIsCreated()
        {
            CreateDefaultCustomer();
            var order = CreateOrder();

            using (new AnyCompanyDbContext(options))
            {
                var result = OrderService.PlaceOrder(order, _defaultCustomerId);

                Assert.True(result);
            }

            ValidateOrder(order);
        }

        [Fact]
        public void GivenUkCustomer_WhenIPlaceOrder_ThenVatIsSet()
        {
            CreateDefaultCustomer();
            var order = CreateOrder(vat:99);

            using (new AnyCompanyDbContext(options))
            {
                var result = OrderService.PlaceOrder(order, _defaultCustomerId);

                Assert.True(result);
            }

            var expectedOrder = CreateOrder();
            ValidateOrder(expectedOrder);

        }

        [Fact]
        public void GivenNonUkCustomer_WhenIPlaceOrder_ThenVatIsZero()
        {
            CreateDefaultCustomer("US");
            var order = CreateOrder();

            using (new AnyCompanyDbContext(options))
            {
                var result = OrderService.PlaceOrder(order, _defaultCustomerId);

                Assert.True(result);
            }

            var expectedOrder = CreateOrder(vat:0);
            ValidateOrder(expectedOrder);

        }

        [Fact]
        public void GivenOrderWithNoAmount_WhenIPlaceOrder_ThenOrderFails()
        {
            CreateDefaultCustomer();
            var order = CreateOrder(amount:0);

            using (new AnyCompanyDbContext(options))
            {
                var result = OrderService.PlaceOrder(order, _defaultCustomerId);

                Assert.False(result);
            }

            ValidateNoOrderCreated();
        }

        private Order CreateOrder(int amount = 15, double vat = 0.2)
        {
            return new Order()
            {
                OrderId = 555,
                Amount = amount,
                VAT = vat
            };
        }

        /// <summary>
        /// Adds a customer to the test database
        /// </summary>
        private void CreateDefaultCustomer(string country = "UK")
        {
            using (var context = new AnyCompanyDbContext(options))
            {
                var defaultCustomer = new Customer()
                {
                    CustomerId = _defaultCustomerId,
                    Country = country,
                    DateOfBirth = new DateTime(1980, 04, 07),
                    Name = "Default Customer"
                };
                context.Add(defaultCustomer);
                context.SaveChanges();

            }
        }

        private void ValidateOrder(Order expected, int expectedCustomerId = 1)
        {
            using (var context = new AnyCompanyDbContext(options))
            {
                var actual = context.Orders
                    .FirstOrDefault();

                Assert.NotNull(actual);
                Assert.Equal(expected.OrderId, actual.OrderId);
                Assert.Equal(expected.VAT, actual.VAT);
                Assert.Equal(expected.Amount, actual.Amount);

                Assert.Equal(expectedCustomerId, actual.CustomerId);
            }
        }

        private void ValidateNoOrderCreated()
        {
            using (var context = new AnyCompanyDbContext(options))
            {
                var actual = context.Orders.FirstOrDefault();
                Assert.Null(actual);
            }
        }

    }
}
