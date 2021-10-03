using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KVVKWeb.Models
{
    public class URLs
    {
        public Add Add { get; set; }
        public Get Get { get; set; }
        public Edit Edit { get; set; }
        public Delete Delete { get; set; }

    }
    public class Add
    {
        public string AddKeyInfo { get; set; }
        public string AddCustomerLicences { get; set; }
        public string AddCustomerClientInfo { get; set; }
        public string AddAccessManagement { get; set; }
    }
    public class Get
    {
        public string GetCustomerLicences { get; set; }
        public string GetLicencePackages { get; set; }
        public string GetAccessManagement { get; set; }
        public string GetAccessManagementID { get; set; }
        public string GetClientInfo { get; set; }
        public string GetKeyInfo { get; set; }
        public string GetMenu { get; set; }
        public string GetCurrentLicences { get; set; }

    }
    public class Edit
    {
        public string EditAccessManagement { get; set; }
    }
    public class Delete
    { }
}
