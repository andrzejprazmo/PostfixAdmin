using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEE.PostfixAdmin.BackEnd.Common.Contracts
{
    public class StartupContract
    {
        [Required]
        [MaxLength(128)]
        [Display(Name = "Domain name")]
        [RegularExpression(@"(?=^.{4,253}$)(^((?!-)[a-zA-Z0-9-]{1,63}(?<!-)\.)+[a-zA-Z]{2,63}$)", ErrorMessage = "Domain name is not valid.")]
        public string DomainName { get; set; }

        [Required]
        [MaxLength(128)]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        public int? Quota { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Retype password")]
        public string Confirm { get; set; }
    }
}
