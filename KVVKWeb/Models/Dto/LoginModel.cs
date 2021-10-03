using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KVVKWeb.Models.Dto
{
    public class LoginModel
    {
        public string Userid { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
        [Display(Name ="RememberMe")]
        public bool RememberMe { get; set; }
    }
}
