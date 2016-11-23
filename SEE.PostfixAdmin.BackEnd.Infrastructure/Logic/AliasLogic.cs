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
    public class AliasLogic : AliasContract
    {
        #region members & ctor
        private readonly DataContext _dataContext;

        protected AliasLogic() { }

        private AliasLogic(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public static AliasLogic Create(DataContext dataContext) => new AliasLogic(dataContext);

        public static AliasLogic Create(DataContext dataContext, int id)
        {
            var aliasLogic = new AliasLogic(dataContext);
            aliasLogic.Refresh(id);
            return aliasLogic;
        }

        #endregion

        public virtual void Copy(AliasContract contract) => AutoMapper<AliasContract, AliasContract>.Map(contract, this);

        public virtual OperationResult Validate(AliasContract contract)
        {
            return AutoValidator<AliasContract>.Validate(contract, (c, errors) =>
            {
                if (_dataContext.Aliases.Any(x => x.Name == c.Name + "@" + c.DomainName && x.Id != c.Id))
                {
                    errors.Add(new ValidationResult("Alias with that name already exists.", new string[] { "Name" }));
                }
            });
        }

        public virtual void Create(string createdBy)
        {
            var entity = new AliasEntity
            {
                Name = $"{Name}@{DomainName}",
                MailboxId = MailboxId,
                CreatedBy = createdBy,
                CreateDate = DateTime.Now,
            };
            _dataContext.Aliases.Add(entity);
            _dataContext.SaveChanges();
            Refresh(entity.Id);
        }

        public virtual void Update(string updatedBy)
        {
            var entity = _dataContext.Aliases.Single(x => x.Id == Id);
            entity.Name = $"{Name}@{DomainName}";
            entity.UpdatedBy = updatedBy;
            entity.UpdateDate = DateTime.Now;
            _dataContext.SaveChanges();
            Refresh(entity.Id);
        }

        public virtual void Refresh(int aliasId)
        {
            var contract = _dataContext.Aliases.Where(x => x.Id == aliasId).Select(x => new AliasContract
            {
                Id = x.Id,
                Name = x.Name,
                MailboxId = x.MailboxId,
                UserName = x.Mailbox.UserName,
                DomainName = x.Mailbox.Domain.Name,
                CreateDate = x.CreateDate,
                CreatedBy = x.CreatedBy,
                UpdateDate = x.UpdateDate,
                UpdatedBy = x.UpdatedBy,
            }).FirstOrDefault();
            Copy(contract);
        }
    }
}
