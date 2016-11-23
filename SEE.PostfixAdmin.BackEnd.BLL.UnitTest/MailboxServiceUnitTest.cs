using Moq;
using NUnit.Framework;
using SEE.PostfixAdmin.BackEnd.BLL.Mailbox;
using SEE.PostfixAdmin.BackEnd.BLL.UnitTest.Fixtures;
using SEE.PostfixAdmin.BackEnd.Common.Contracts;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Logic;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Repository.Mailbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEE.PostfixAdmin.BackEnd.BLL.UnitTest.Domain
{
    public class MailboxServiceUnitTest
    {
        [Test]
        public void MailboxServiceCreateMailbox_InputNotValid_ResultFail()
        {
            // Arrange
            var fixture = new MailboxServiceFixture();
            var createdBy = "andrzej.prazmo";
            var passwordContract = new PasswordContract { };
            var mailboxContract = new RegisterContract { };
            fixture.MailboxLogicMock.Setup(x => x.Validate(mailboxContract)).Returns(new Common.OperationResult("ERROR"));
            fixture.MailboxLogicMock.Setup(x => x.Copy(mailboxContract));
            fixture.MailboxLogicMock.Setup(x => x.Create(createdBy, It.IsAny<string>()));
            fixture.PasswordLogicMock.Setup(x => x.Validate(passwordContract)).Returns(new Common.OperationResult("ERROR"));
            fixture.MailboxRepositoryMock.Setup(x => x.CreateMailboxLogic()).Returns(fixture.MailboxLogicMock.Object);
            fixture.MailboxRepositoryMock.Setup(x => x.CreatePasswordLogic()).Returns(fixture.PasswordLogicMock.Object);

            // Act
            var sut = fixture.CreateSut();
            var result = sut.CreateMailbox(mailboxContract, createdBy);

            // Assert
            Assert.False(result.Succeeded);
            fixture.MailboxLogicMock.Verify(x => x.Validate(mailboxContract), Times.Once());
            fixture.PasswordLogicMock.Verify(x => x.Validate(It.IsAny<PasswordContract>()), Times.Once());
            fixture.MailboxLogicMock.Verify(x => x.Copy(mailboxContract), Times.Never());
            fixture.MailboxLogicMock.Verify(x => x.Create(createdBy, It.IsAny<string>()), Times.Never());
        }

        [Test]
        public void MailboxServiceCreateMailbox_InputValid_ResultOK()
        {
            // Arrange
            var fixture = new MailboxServiceFixture();
            var createdBy = "andrzej.prazmo";
            fixture.MailboxLogicMock.Setup(x => x.Copy(It.IsAny<RegisterContract>()));
            fixture.MailboxLogicMock.Setup(x => x.Validate(It.IsAny<RegisterContract>())).Returns(new Common.OperationResult());
            fixture.PasswordLogicMock.Setup(x => x.Validate(It.IsAny<PasswordContract>())).Returns(new Common.OperationResult());
            fixture.MailboxLogicMock.Setup(x => x.Create(createdBy, It.IsAny<string>()));

            fixture.MailboxRepositoryMock.Setup(x => x.CreateMailboxLogic()).Returns(fixture.MailboxLogicMock.Object);
            fixture.MailboxRepositoryMock.Setup(x => x.CreatePasswordLogic()).Returns(fixture.PasswordLogicMock.Object);
            // Act
            var sut = fixture.CreateSut();
            var result = sut.CreateMailbox(new RegisterContract(), createdBy);

            // Assert
            fixture.PasswordLogicMock.Verify(x => x.Validate(It.IsAny<PasswordContract>()), Times.Once());
            fixture.MailboxLogicMock.Verify(x => x.Validate(It.IsAny<RegisterContract>()), Times.Once());
            fixture.MailboxLogicMock.Verify(x => x.Copy(It.IsAny<RegisterContract>()), Times.Once());
            fixture.MailboxLogicMock.Verify(x => x.Create(createdBy, It.IsAny<string>()), Times.Once());
            Assert.True(result.Succeeded);
        }
    }
}
