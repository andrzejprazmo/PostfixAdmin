using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SEE.PostfixAdmin.BackEnd.Infrastructure.Model
{
    [Table("mailboxes", Schema = "vmail")]
    internal class MailboxEntity
    {
        [Key]
        [Column("mbx_id")]
        public int Id { get; set; }

        [Column("dmn_id")]
        public int DomainId { get; set; }

        [Column("mbx_username")]
        public string UserName { get; set; }

        [Column("mbx_password")]
        public string Password { get; set; }

        [Column("mbx_quota")]
        public int? Quota { get; set; }

        [Column("mbx_is_admin")]
        public bool IsAdmin { get; set; }

        [Column("mbx_created_by")]
        public string CreatedBy { get; set; }

        [Column("mbx_create_time")]
        public DateTime? CreateDate { get; set; }

        [Column("mbx_updated_by")]
        public string UpdatedBy { get; set; }

        [Column("mbx_update_time")]
        public DateTime? UpdateDate { get; set; }

        [Column("mbx_is_active")]
        public bool IsActive { get; set; }

        [ForeignKey("DomainId")]
        public virtual DomainEntity Domain { get; set; }

        public ICollection<AliasEntity> Aliases { get; set; }
    }
}
