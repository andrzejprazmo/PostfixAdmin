using SEE.PostfixAdmin.BackEnd.Common;
using SEE.PostfixAdmin.BackEnd.Common.Contracts;
using SEE.PostfixAdmin.BackEnd.Infrastructure;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Model;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Repository.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace SEE.PostfixAdmin.BackEnd.Infrastructure.Logic
{
    public class MailboxLogic : MailboxContract
    {
        #region members & ctor
        private readonly DataContext _dataContext;
        private readonly IConfigurationRepository _configuration;

        protected MailboxLogic()
        {

        }

        private MailboxLogic(DataContext dataContext, IConfigurationRepository configuration)
        {
            _dataContext = dataContext;
            _configuration = configuration;
        }

        public static MailboxLogic Create(DataContext dataContext, IConfigurationRepository configuration) => new MailboxLogic(dataContext, configuration);

        public static MailboxLogic Create(DataContext dataContext, IConfigurationRepository configuration, int id)
        {
            var mailboxLogic = new MailboxLogic(dataContext, configuration);
            mailboxLogic.Refresh(id);
            return mailboxLogic;
        }

        #endregion

        public virtual void Copy(RegisterContract contract) => AutoMapper<RegisterContract, MailboxContract>.Map(contract, this);

        public virtual void Copy(MailboxContract contract) => AutoMapper<MailboxContract, MailboxContract>.Map(contract, this);

        public virtual OperationResult Validate(RegisterContract contract)
        {
            return Validate(new MailboxContract
            {
                DomainId = contract.DomainId,
                UserName = contract.UserName,
                IsAdmin = contract.IsAdmin,
                Quota = contract.Quota,
                IsActive = contract.IsActive,
            });
        }
        public virtual OperationResult Validate(MailboxContract contract)
        {
            return AutoValidator<MailboxContract>.Validate(contract, (c, errors) =>
            {
                if (_dataContext.Mailboxes.Any(x => x.Id != c.Id && x.DomainId == c.DomainId && x.UserName == c.UserName))
                {
                    errors.Add(new ValidationResult("User with that email address already exists.", new string[] { "Name" }));
                }
            });
        }

        public virtual void Create(string createdBy, string encryptedPassword)
        {
            var mailbox = new MailboxEntity
            {
                DomainId = DomainId.Value,
                UserName = UserName,
                Quota = Quota,
                Password = encryptedPassword,
                CreatedBy = createdBy,
                CreateDate = DateTime.Now,
                IsAdmin = IsAdmin,
                IsActive = IsActive,
            };
            _dataContext.Mailboxes.Add(mailbox);
            _dataContext.SaveChanges();
            Refresh(mailbox.Id);
        }

        public virtual void Update(string updatedBy)
        {
            var mailbox = _dataContext.Mailboxes.Single(x => x.Id == Id);
            mailbox.DomainId = DomainId.Value;
            mailbox.UserName = UserName;
            mailbox.Quota = Quota;
            mailbox.IsAdmin = IsAdmin;
            mailbox.IsActive = IsActive;
            mailbox.UpdateDate = DateTime.Now;
            mailbox.UpdatedBy = updatedBy;
            _dataContext.SaveChanges();
            Refresh(Id);
        }

        public virtual void SetPassword(int userId, string encryptedPassword, string updatedBy)
        {
            var mailbox = _dataContext.Mailboxes.Single(x => x.Id == Id);
            mailbox.Password = encryptedPassword;
            mailbox.UpdateDate = DateTime.Now;
            mailbox.UpdatedBy = updatedBy;
            _dataContext.SaveChanges();

        }

        protected virtual void Refresh(int mailboxId)
        {
            var contract = _dataContext.Mailboxes.Where(x => x.Id == mailboxId).Select(x => new MailboxContract
            {
                Id = x.Id,
                DomainId = x.DomainId,
                DomainName = x.Domain.Name,
                UserName = x.UserName,
                Quota = x.Quota,
                CreateDate = x.CreateDate,
                CreatedBy = x.CreatedBy,
                UpdateDate = x.UpdateDate,
                UpdatedBy = x.UpdatedBy,
                IsAdmin = x.IsAdmin,
                IsActive = x.IsActive,
            }).Single();
            Copy(contract);
        }
    }
}
