using SEE.PostfixAdmin.BackEnd.Common;
using SEE.PostfixAdmin.BackEnd.Common.Contracts;
using SEE.PostfixAdmin.BackEnd.Common.List;
using SEE.PostfixAdmin.BackEnd.Common.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEE.PostfixAdmin.BackEnd.BLL.Domain
{
    public interface IDomainService
    {
        DataResponse<DomainRequest, DomainContract> FindDomainsBy(DomainRequest request);

        OperationResult<int> CreateDomain(DomainContract contract, string createdBy);

        OperationResult<DomainContract> GetDomainById(int id);

        OperationResult<int> UpdateDomain(DomainContract contract, string updatedBy);

        Dictionary<int, string> GetDomainsDictionary();

        OperationResult RemoveDomain(int id);

    }
}
