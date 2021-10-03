using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KVVKWeb.Models.Dto
{
    public class DownloadReturnModel
    {
        public string Email { get; set; }
        public string ID { get; set; }
        public int CustomerId { get; set; }
        public int TotalPackagesCount { get; set; }
        public int  RemainingPackagesCount { get; set; }
        public string AccessToken { get; set; }

    }
}
