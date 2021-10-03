using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KVVKWeb.Models.Entities
{
    public class Management
    {
        public int ID { get; set; }
        [NotMapped]
        public string EncryptedID { get; set; }
        public int CustomerClientID { get; set; }
        [Required]
        public string PolicyName { get; set; }
        [Required]
        [Display(Name = "Is File?")]
        public bool FileorDirectory { get; set; }
        [Required]
        public string Path { get; set; }
        [Display(Name = "Include Sub Directory?")]
        public bool IncludeSubDir { get; set; }
        public string UnwantedDir { get; set; }
        public string WantedExtensions { get; set; }
        public string UnwantedExtensions { get; set; }
        [Required]
        public int AuthClass { get; set; }
        [Required]
        public string UserName { get; set; }
        public bool Isnew { get; set; }

        public string UserId { get; set; }
    }
}
