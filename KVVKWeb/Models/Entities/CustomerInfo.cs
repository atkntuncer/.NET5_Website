using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;


namespace KVVKWeb.Models.Entities
{
    public class CustomerInfo:IdentityUser
    {
        [DataMember]
        [Column("CustomerID"), Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerID { get; set; }
        public bool Status { get; set; }
        public byte CustomerType { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string NationalID { get; set; }
        public DateTime DateofBirth { get; set; }
        public string MobilePhone { get; set; }
        public string Adress { get; set; }
        public string CompanyName { get; set; }
    }
}
