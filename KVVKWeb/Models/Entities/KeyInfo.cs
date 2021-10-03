using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KVVKWeb.Models.Entities
{
    public class KeyInfo
    {
        public int ID { get; set; }//dummy prop
        public string CustomerID { get; set; }
        [Required]
        public string KeyLabel { get; set; }
        public string KeyType { get; set; }
        public string KeyBit { get; set; }
    }
}
