using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SEE.PostfixAdmin.BackEnd.Common.Contracts
{
    public class PasswordContract
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string DomainName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Retype password")]
        public string Confirm { get; set; }
    }
}
