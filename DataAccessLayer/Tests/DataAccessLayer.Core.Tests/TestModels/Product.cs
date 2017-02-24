using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Core.Tests.TestModels
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public string Type { get; set; }

        public int ProducerId { get; set; }

        [ForeignKey("ProducerId")]
        public ProductProducer Producer { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        public int OrderId { get; set; }

        public decimal Price { get; set; }
    }
}
