using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SEE.PostfixAdmin.BackEnd.Common.Contracts
{
    public class MailboxContract
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Domain name")]
        public int? DomainId { get; set; }

        [Required]
        [MaxLength(128)]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string DomainName { get; set; }

        [Required]
        public int? Quota { get; set; }

        public DateTime? CreateDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string UpdatedBy { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsActive { get; set; }

        public int AliasesCount { get; set; }

        public List<AliasContract> Aliases { get; set; }
    }
}
