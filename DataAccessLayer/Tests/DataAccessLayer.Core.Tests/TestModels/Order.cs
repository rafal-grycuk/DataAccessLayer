using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Core.Tests.TestModels
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public int ClientId { get; set; }

        [ForeignKey("ClientId")]
        public  Client Client { get; set; }
        
        public  ICollection<Product> Products { get; set; }

        public DateTime PlaceOrderTimeStamp { get; set; }
    }
}
