using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KVVKWeb.Models.Entities
{
    public class Packages
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ClientCount { get; set; }
        public decimal Price { get; set; }
    }
}
