using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AnyCompany.Core.Query.Domain
{
    /// <summary>
    /// Read Only version of the Customer Domain Object, for returning in Read Only Queries.
    /// </summary>
    [Table("Customers")]
    public class CustomerDTO
    {
        [Key]
        public int CustomerId { get; private set; }

        public string Country { get; private set; }

        public DateTime DateOfBirth { get; private set; }

        public string Name { get; private set; }

        public IEnumerable<OrderDTO> Orders { get; private set; }
    }
}
