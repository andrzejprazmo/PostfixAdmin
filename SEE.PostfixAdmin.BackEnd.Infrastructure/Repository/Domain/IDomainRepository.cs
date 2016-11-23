using SEE.PostfixAdmin.BackEnd.Common;
using SEE.PostfixAdmin.BackEnd.Common.Contracts;
using SEE.PostfixAdmin.BackEnd.Common.List;
using SEE.PostfixAdmin.BackEnd.Common.Requests;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEE.PostfixAdmin.BackEnd.Infrastructure.Repository.Domain
{
    public interface IDomainRepository
    {
        DomainLogic CreateDomainLogic();

        DomainLogic GetDomainLogic(int id);

        DataResponse<DomainRequest, DomainContract> FindBy(DomainRequest request);

        Dictionary<int, string> GetDomainsDictionary();

        OperationResult RemoveDomain(int id);
    }
}
