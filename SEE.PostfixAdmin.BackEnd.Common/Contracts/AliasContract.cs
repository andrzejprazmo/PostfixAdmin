using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SEE.PostfixAdmin.BackEnd.Common.Contracts
{
    public class AliasContract
    {
        public int Id { get; set; }

        [Required]
        public int MailboxId { get; set; }

        public string UserName { get; set; }

        public string DomainName { get; set; }

        [Required]
        public string Name { get; set; }
        public DateTime? CreateDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string UpdatedBy { get; set; }
    }
}
