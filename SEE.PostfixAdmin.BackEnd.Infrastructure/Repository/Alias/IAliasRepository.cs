using SEE.PostfixAdmin.BackEnd.Common;
using SEE.PostfixAdmin.BackEnd.Common.Contracts;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEE.PostfixAdmin.BackEnd.Infrastructure.Repository.Alias
{
    public interface IAliasRepository
    {
        List<AliasContract> FindByMailbox(int mailboxId);
        AliasLogic CreateAliasLogic();
        AliasLogic GetAliasLogic(int id);
        OperationResult<int> RemoveAlias(int id);
    }
}
