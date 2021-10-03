using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace KVVKWeb.Models.Dto
{
    public class PasswordModel
    {
        public string Id { get; set; }
        [Display(Name = "Current Password")]
        [Required]
        public string OldPassword { get; set; }
        [Display(Name = "New Password")]
        [Required]
        public string NewPassword { get; set; }
        [Display(Name = "Verify Password")]
        [Compare("NewPassword", ErrorMessage = "Password doesn't match")]
        [Required]
        public string RePassword { get; set; }
      
    }
}
