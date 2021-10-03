
using KVVKWeb.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KVVKWeb.Models.Dto
{
    public class PasswordForgotModel
    {
        [Required]
        [EmailAddress(ErrorMessage ="Invalid email adress")]
        [Remote(action:"IsUserExist","Account")]
        public string Email { get; set; }
    }
}
