using SEE.PostfixAdmin.BackEnd.Common;
using SEE.PostfixAdmin.BackEnd.Common.Contracts;
using SEE.PostfixAdmin.BackEnd.Common.List;
using SEE.PostfixAdmin.BackEnd.Common.Requests;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEE.PostfixAdmin.BackEnd.BLL.Mailbox
{
    public interface IMailboxService
    {
        MailboxContract GetMailboxById(int id);

        OperationResult<int> CreateMailbox(RegisterContract contract, string createdBy);

        DataResponse<MailboxRequest, MailboxContract> FindMailboxesBy(MailboxRequest request);

        OperationResult RemoveMailbox(int id);

        OperationResult<MailboxContract> GetMailboxDetails(int id);

        OperationResult<int> UpdateMailbox(MailboxContract contract, string updatedBy);

        PasswordContract GetPasswordContract(int id);

        OperationResult<int> ChangePassword(PasswordContract contract, string updatedBy);

        StatsContract GetStats();

    }
}
