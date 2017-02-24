using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Core.Tests.TestModels
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string PhoneNumer { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
