using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Core.Tests.TestModels
{
    public class ProductProducer
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address{ get; set; }

    }
}
