using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SEE.PostfixAdmin.BackEnd.Common.Contracts
{
    public class RegisterContract
    {
        [Required]
        [Display(Name = "Domain name")]
        public int? DomainId { get; set; }

        [Required]
        [MaxLength(128)]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        public int? Quota { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Retype password")]
        public string Confirm { get; set; }
    }
}
