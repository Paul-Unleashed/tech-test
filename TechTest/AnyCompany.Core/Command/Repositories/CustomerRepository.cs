using System.Linq;
using AnyCompany.Core.Command.Domain;

namespace AnyCompany.Core.Command.Repositories
{
    public static class CustomerRepository
    {
        private static string ConnectionString = @"Data Source=(local);Database=Customers;User Id=admin;Password=password;";

        public static Customer Load(int customerId)
        {
            return AnyCompanyDbContext.Instance.Customers.FirstOrDefault(c => c.CustomerId == customerId);
        }
    }
}
