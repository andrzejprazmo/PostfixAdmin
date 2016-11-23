using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SEE.PostfixAdmin.BackEnd.Common.Contracts
{
    public class DomainContract
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [RegularExpression(@"(?=^.{4,253}$)(^((?!-)[a-zA-Z0-9-]{1,63}(?<!-)\.)+[a-zA-Z]{2,63}$)", ErrorMessage = "Domain name is not valid.")]
        public string Name { get; set; }

        public int MailboxCount { get; set; }

        public DateTime? CreateDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string UpdatedBy { get; set; }

        public List<MailboxContract> Mailboxes { get; set; } = new List<MailboxContract>();
    }
}
