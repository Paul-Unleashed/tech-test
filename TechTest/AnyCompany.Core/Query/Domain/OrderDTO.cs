using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AnyCompany.Core.Query.Domain
{
    /// <summary>
    /// Readonly version of the Order domain object, for returning in Read Only Queries.
    /// </summary>
    [Table("Orders")]
    public class OrderDTO
    {
        [Key]
        public int OrderId { get; private set; }

        public double Amount { get; private set; }

        public double VAT { get; private set; }

        public int CustomerId { get; private set; }

        public CustomerDTO Customer { get; set; }

    }
}
