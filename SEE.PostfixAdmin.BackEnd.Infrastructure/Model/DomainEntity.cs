using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SEE.PostfixAdmin.BackEnd.Infrastructure.Model
{
    [Table("domains", Schema = "vmail")]
    internal class DomainEntity
    {
        [Key]
        [Column("dmn_id")]
        public int Id { get; set; }

        [Column("dmn_name")]
        public string Name { get; set; }

        [Column("dmn_created_by")]
        public string CreatedBy { get; set; }

        [Column("dmn_create_time")]
        public DateTime? CreateDate { get; set; }

        [Column("dmn_updated_by")]
        public string UpdatedBy { get; set; }

        [Column("dmn_update_time")]
        public DateTime? UpdateDate { get; set; }

        public ICollection<MailboxEntity> Mailboxes { get; set; } = new List<MailboxEntity>();
    }
}
