using AnyCompany.Core.Command.Domain;

namespace AnyCompany.Core.Command.Repositories
{
    internal class OrderRepository
    {
        private static string ConnectionString = @"Data Source=(local);Database=Orders;User Id=admin;Password=password;";

        public void Save(Order order)
        {
            AnyCompanyDbContext context = AnyCompanyDbContext.Instance;

            context.Add(order);
            context.SaveChanges();
        }
    }
}
