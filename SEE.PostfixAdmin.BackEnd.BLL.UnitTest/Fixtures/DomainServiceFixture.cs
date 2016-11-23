using Microsoft.Extensions.Logging;
using Moq;
using SEE.PostfixAdmin.BackEnd.BLL.Domain;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Logic;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Repository.Domain;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Repository.Mailbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEE.PostfixAdmin.BackEnd.BLL.UnitTest.Fixtures
{
    public class DomainServiceFixture
    {
        public Mock<IDomainRepository> DomainRepositoryMock = new Mock<IDomainRepository>();
        public Mock<IMailboxRepository> MailboxRepositoryMock = new Mock<IMailboxRepository>();
        public Mock<ILogger<DomainService>> LoggerMock = new Mock<ILogger<DomainService>>();
        public Mock<DomainLogic> DomainLogicMock = new Mock<DomainLogic>();

        public IDomainService CreateSut()
        {
            return new DomainService(DomainRepositoryMock.Object, MailboxRepositoryMock.Object, LoggerMock.Object);
        }
    }
}
