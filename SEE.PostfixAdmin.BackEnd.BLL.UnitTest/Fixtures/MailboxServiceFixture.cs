using Microsoft.Extensions.Logging;
using Moq;
using SEE.PostfixAdmin.BackEnd.BLL.Mailbox;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Logic;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Repository.Alias;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Repository.Mailbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEE.PostfixAdmin.BackEnd.BLL.UnitTest.Fixtures
{
    public class MailboxServiceFixture
    {
        public Mock<IMailboxRepository> MailboxRepositoryMock = new Mock<IMailboxRepository>();
        public Mock<IAliasRepository> AliasRepositoryMock = new Mock<IAliasRepository>();
        public Mock<ILogger<MailboxService>> LoggerMock = new Mock<ILogger<MailboxService>>();
        public Mock<MailboxLogic> MailboxLogicMock = new Mock<MailboxLogic>();
        public Mock<PasswordLogic> PasswordLogicMock = new Mock<PasswordLogic>();

        public IMailboxService CreateSut()
        {
            return new MailboxService(MailboxRepositoryMock.Object, AliasRepositoryMock.Object, LoggerMock.Object);
        }
    }
}
