using AnyCompany.Core.Query.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AnyCompany.Core.Query.Repositories
{

    public class CustomerQueryRepository
    {
        /// <summary>
        /// Method to load all customers and their orders. 
        /// Though the spec does not specify if the LoadAll method needs to be read-only or not,
        /// I have assumed read-only because it is very rare (and very unperformant) that you need to edit everything at once.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CustomerDTO> LoadAll()
        {
            return AnyCompanyDbContext.Instance
                .CustomerDTOs
                .Include(c => c.Orders) // make sure orders are loaded.
                .AsNoTracking() // Switch off tracking to save performance.
                .ToList(); 
        }
    }
}
