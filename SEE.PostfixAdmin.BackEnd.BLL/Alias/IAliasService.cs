using SEE.PostfixAdmin.BackEnd.Common;
using SEE.PostfixAdmin.BackEnd.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEE.PostfixAdmin.BackEnd.BLL.Alias
{
    public interface IAliasService
    {
        AliasContract GetAlias(int id);
        OperationResult<int> CreateAlias(AliasContract contract, string createdBy);
        OperationResult<int> UpdateAlias(AliasContract contract, string updatedBy);
        OperationResult<int> RemoveAlias(int id);

    }
}
