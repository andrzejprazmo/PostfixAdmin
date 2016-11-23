using SEE.PostfixAdmin.BackEnd.Common;
using SEE.PostfixAdmin.BackEnd.Common.Contracts;
using SEE.PostfixAdmin.BackEnd.Common.List;
using SEE.PostfixAdmin.BackEnd.Common.Requests;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEE.PostfixAdmin.BackEnd.Infrastructure.Repository.Mailbox
{
    public interface IMailboxRepository
    {
        MailboxLogic CreateMailboxLogic();
        PasswordLogic CreatePasswordLogic();
        MailboxLogic GetMailboxLogic(int id);
        PasswordContract GetPasswordContract(int id);
        PasswordContract GetAdmin(string userName);
        DataResponse<MailboxRequest, MailboxContract> FindBy(MailboxRequest request);
        OperationResult RemoveMailbox(int id);
        StartupLogic CreateStartupLogic();
        bool AccountsExist();
        StatsContract GetStats();
    }
}
