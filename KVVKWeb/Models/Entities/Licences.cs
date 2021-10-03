using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KVVKWeb.Models.Entities
{
    public class Licences
    {
        public int ID { get; set; }
        public bool Status { get; set; }
        public string CustomerID { get; set; }
        public int LicencePackageID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
