using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KVVKWeb.Models.Dto
{
    public class ChangeInfoModel
    {
        [Key]
        public int id { get; set; }
        public string UserID { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string NationalID { get; set; }
        [Required]
        [Display(Name = "Date of Birth")]
        public DateTime DateofBirth { get; set; }
        [Required]
        public string Phone { get; set; }
        public string MobilePhone { get; set; }
        [Required]
        public string Adress { get; set; }
        [Required]
        public string CompanyName { get; set; }
    }
}
