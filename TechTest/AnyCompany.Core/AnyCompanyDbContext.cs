using AnyCompany.Core.Command.Domain;
using AnyCompany.Core.Query.Domain;
using Microsoft.EntityFrameworkCore;


namespace AnyCompany.Core
{
    /// <summary>
    /// Project has been switched from raw SQL to using Entity Framework Core. 
    /// 
    /// Primary reason is that EF Core is significantly easier to work with in unit tests (In Memory Provider).
    /// It also significantly reduces code in the repository classes, and removes the need to write SQL to generate the schema.
    /// </summary>
    public class AnyCompanyDbContext : DbContext
    {
        // FIXME - bit of a hack, any final solution should use an IoC container to create/dispose the Context at appropriate times.
        public static AnyCompanyDbContext Instance { get; private set; }


        public AnyCompanyDbContext() :base()
        {
            Instance = this;
        }

        public AnyCompanyDbContext(DbContextOptions options) : base(options)
        {
            Instance = this;
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Order> Orders { get; set; }

        // The DTO object could potentially be moved to a different context to complete the command/query split 
        public DbSet<CustomerDTO> CustomerDTOs { get; set; }

    }
}
