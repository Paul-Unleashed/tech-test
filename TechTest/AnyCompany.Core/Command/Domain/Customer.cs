using System;
using System.ComponentModel.DataAnnotations;

namespace AnyCompany.Core.Command.Domain
{
    public class Customer
    {
        public int CustomerId { get; set; }

        public string Country { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Name { get; set; }
    }
}
