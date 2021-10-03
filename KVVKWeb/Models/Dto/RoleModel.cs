using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KVVKWeb.Models.Dto
{
    public class RoleModel
    {
        public string UserID { get; set; }
        public string RoleName { get; set; }
        public List<LoginModel> Customers { get; set; }
        public List<string> RoleNames { get; set; }
    }
}

