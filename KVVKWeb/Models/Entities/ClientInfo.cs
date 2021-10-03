using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KVVKWeb.Models.Entities
{
    public class ClientInfo
    {
        public int ID { get; set; }
        public bool Status { get; set; }
        public string CustomerID { get; set; }
        public int CustomerLicenceID { get; set; }
        public Guid ClientGuid { get; set; }
        public string BasePath { get; set; }

    }
}
