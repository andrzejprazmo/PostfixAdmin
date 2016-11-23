using SEE.PostfixAdmin.BackEnd.Common;
using SEE.PostfixAdmin.BackEnd.Common.Contracts;
using SEE.PostfixAdmin.BackEnd.Infrastructure;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SEE.PostfixAdmin.BackEnd.Infrastructure.Logic
{
    public class DomainLogic : DomainContract
    {
        #region members & ctor
        private readonly DataContext _dataContext;

        protected DomainLogic() { }

        private DomainLogic(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public static DomainLogic Create(DataContext dataContext) => new DomainLogic(dataContext);

        public static DomainLogic Create(DataContext dataContext, int id)
        {
            var domainLogic = new DomainLogic(dataContext);
            domainLogic.Refresh(id);
            return domainLogic;
        }

        #endregion

        public virtual void Copy(DomainContract contract) => AutoMapper<DomainContract, DomainContract>.Map(contract, this);

        public virtual OperationResult Validate(DomainContract contract)
        {
            return AutoValidator<DomainContract>.Validate(contract, (c, errors) =>
            {
                if (_dataContext.Domains.Any(x => x.Name == c.Name && x.Id != c.Id))
                {
                    errors.Add(new ValidationResult("Domain with that name already exists.", new string[] { "Name" }));
                }
            });
        }

        public virtual void Create(string createdBy)
        {
            var entity = new DomainEntity
            {
                Name = Name,
                CreatedBy = createdBy,
                CreateDate = DateTime.Now,
            };
            _dataContext.Domains.Add(entity);
            _dataContext.SaveChanges();
            Refresh(entity.Id);
        }

        public virtual void Update(string updatedBy)
        {
            var entity = _dataContext.Domains.Single(x => x.Id == Id);
            entity.Name = Name;
            entity.UpdatedBy = updatedBy;
            entity.UpdateDate = DateTime.Now;
            _dataContext.SaveChanges();
            Refresh(entity.Id);
        }

        public virtual void Refresh(int domainId)
        {
            var contract = _dataContext.Domains.Where(x => x.Id == domainId).Select(x => new DomainContract
            {
                Id = x.Id,
                Name = x.Name,
                CreateDate = x.CreateDate,
                CreatedBy = x.CreatedBy,
                UpdateDate = x.UpdateDate,
                UpdatedBy = x.UpdatedBy,
                MailboxCount = _dataContext.Mailboxes.Where(y => y.DomainId == x.Id).Count(),
            }).Single();
            Copy(contract);
        }
    }
}
