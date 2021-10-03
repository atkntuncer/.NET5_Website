using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace KVVKWeb.Models.Dto
{
    public class PasswordResetModel
    {
        public string Id { get; set; }
        [Display(Name = "New Password")]
        [Required]
        public string NewPassword { get; set; }
        [Display(Name = "Verify Password")]
        [Compare("NewPassword", ErrorMessage = "Password doesn't match")]
        [Required]
        public string RePassword { get; set; }
        public string PasswordCode { get; set; }//forgot password
        public MailAddress Email { get; set; }//forgot password
    }
}
