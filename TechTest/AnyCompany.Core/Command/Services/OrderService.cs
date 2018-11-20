using AnyCompany.Core.Command.Domain;
using AnyCompany.Core.Command.Repositories;

namespace AnyCompany.Core.Command.Services
{
    public class OrderService
    {
        private readonly OrderRepository orderRepository = new OrderRepository();

        public bool PlaceOrder(Order order, int customerId)
        {
            Customer customer = CustomerRepository.Load(customerId);

            if (order == null || customer == null || order.Amount == 0)
                return false;

            // Completly replacing the customer prevents updates to the customer during PlaceOrder.
            order.Customer = customer;

            if (customer.Country == "UK")
                order.VAT = 0.2d;
            else
                order.VAT = 0;

            orderRepository.Save(order);

            return true;
        }
    }
}
