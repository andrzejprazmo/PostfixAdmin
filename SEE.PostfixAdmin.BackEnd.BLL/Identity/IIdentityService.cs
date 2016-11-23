using SEE.PostfixAdmin.BackEnd.Common;
using SEE.PostfixAdmin.BackEnd.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SEE.PostfixAdmin.BackEnd.BLL.Identity
{
    public interface IIdentityService
    {
        OperationResult<ClaimsPrincipal> Validate(string userName, string password);

        OperationResult RegisterMailbox(StartupContract contract);

        bool AccountsExist();
    }
}
