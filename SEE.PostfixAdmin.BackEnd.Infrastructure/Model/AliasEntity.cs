using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SEE.PostfixAdmin.BackEnd.Infrastructure.Model
{
    [Table("aliases", Schema = "vmail")]
    internal class AliasEntity
    {
        [Key]
        [Column("als_id")]
        public int Id { get; set; }

        [Column("mbx_id")]
        public int MailboxId { get; set; }

        [Column("als_name")]
        public string Name { get; set; }

        [Column("als_created_by")]
        public string CreatedBy { get; set; }

        [Column("als_create_time")]
        public DateTime? CreateDate { get; set; }

        [Column("als_updated_by")]
        public string UpdatedBy { get; set; }

        [Column("als_update_time")]
        public DateTime? UpdateDate { get; set; }

        [ForeignKey("MailboxId")]
        public virtual MailboxEntity Mailbox { get; set; }
    }
}
