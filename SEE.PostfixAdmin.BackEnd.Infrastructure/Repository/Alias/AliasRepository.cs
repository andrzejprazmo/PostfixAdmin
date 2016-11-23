using Microsoft.EntityFrameworkCore;
using SEE.PostfixAdmin.BackEnd.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SEE.PostfixAdmin.BackEnd.Common;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Logic;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Model;

namespace SEE.PostfixAdmin.BackEnd.Infrastructure.Repository.Alias
{
    public class AliasRepository : IAliasRepository
    {
        private readonly DataContext _dataContext;
        public AliasRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public AliasLogic CreateAliasLogic() => AliasLogic.Create(_dataContext);

        public List<AliasContract> FindByMailbox(int mailboxId)
        {
            return _dataContext.Aliases.AsNoTracking().Where(x => x.MailboxId == mailboxId).OrderBy(x => x.Name).Select(x => new AliasContract
            {
                Id = x.Id,
                MailboxId = x.MailboxId,
                UserName = x.Mailbox.UserName,
                DomainName = x.Mailbox.Domain.Name,
                Name = x.Name,
                CreatedBy = x.CreatedBy,
                CreateDate = x.CreateDate,
                UpdateDate = x.UpdateDate,
                UpdatedBy = x.UpdatedBy
            }).ToList();
        }

        public AliasLogic GetAliasLogic(int id) => AliasLogic.Create(_dataContext, id);

        public OperationResult<int> RemoveAlias(int id)
        {
            var aliasEntity = _dataContext.Aliases.Single(x => x.Id == id);
            _dataContext.Aliases.Remove(aliasEntity);
            _dataContext.SaveChanges();
            return new OperationResult<int>(aliasEntity.MailboxId);
        }
    }
}
