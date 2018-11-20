using AnyCompany.Core.Command.Domain;
using Microsoft.EntityFrameworkCore;


namespace AnyCompany.Core
{
    public class AnyCompanyDbContext : DbContext
    {
        public static AnyCompanyDbContext Instance { get; private set; }

        static AnyCompanyDbContext()
        {
            Instance = new AnyCompanyDbContext();
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Order> Orders { get; set; }

    }
}
