using SEE.PostfixAdmin.BackEnd.Common;
using SEE.PostfixAdmin.BackEnd.Common.Contracts;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SEE.PostfixAdmin.BackEnd.Infrastructure.Logic
{
    public class StartupLogic : StartupContract
    {
        #region members & ctor
        private readonly DataContext _dataContext;

        protected StartupLogic()
        {

        }

        private StartupLogic(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public static StartupLogic Create(DataContext dataContext) => new StartupLogic(dataContext);

        #endregion

        public virtual OperationResult Validate(StartupContract contract)
        {
            return AutoValidator<StartupContract>.Validate(contract, (c, errors) =>
            {
                if (_dataContext.Mailboxes.Any())
                {
                    errors.Add(new ValidationResult("Cannot create first account. Some account exists.", new string[] { "EXCEPTION" }));
                }
            });
        }

        public virtual void Copy(StartupContract contract) => AutoMapper<StartupContract, StartupContract>.Map(contract, this);

        public virtual void Create(string encryptedPassword)
        {
            var domainEntity = new DomainEntity
            {
                Name = DomainName,
                CreateDate = DateTime.Now,
                CreatedBy = $"{UserName}@{DomainName}",
            };
            var mailboxEntity = new MailboxEntity
            {
                UserName = UserName,
                Quota = Quota,
                IsAdmin = true,
                IsActive = true,
                Password = encryptedPassword,
                CreateDate = DateTime.Now,
                CreatedBy = $"{UserName}@{DomainName}",
                Domain = domainEntity,
            };
            _dataContext.Mailboxes.Add(mailboxEntity);
            _dataContext.SaveChanges();
        }
    }
}
