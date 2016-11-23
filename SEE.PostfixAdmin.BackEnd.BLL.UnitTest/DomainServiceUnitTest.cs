using Moq;
using NUnit.Framework;
using SEE.PostfixAdmin.BackEnd.BLL.UnitTest.Fixtures;
using SEE.PostfixAdmin.BackEnd.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEE.PostfixAdmin.BackEnd.BLL.UnitTest.Domain
{
    public class DomainServiceUnitTest
    {
        [Test]
        public void DomainServiceCreateDomain_InputNotValid_ResultFail()
        {
            // Arrange
            var fixture = new DomainServiceFixture();
            var createdBy = "andrzej.prazmo";
            var domainContract = new DomainContract { };
            fixture.DomainLogicMock.Setup(x => x.Copy(domainContract));
            fixture.DomainLogicMock.Setup(x => x.Validate(domainContract)).Returns(new Common.OperationResult("ERROR"));
            fixture.DomainLogicMock.Setup(x => x.Create(createdBy));

            fixture.DomainRepositoryMock.Setup(x => x.CreateDomainLogic()).Returns(fixture.DomainLogicMock.Object);
            // Act
            var sut = fixture.CreateSut();
            var result = sut.CreateDomain(domainContract, createdBy);

            // Assert
            Assert.False(result.Succeeded);
            fixture.DomainLogicMock.Verify(x => x.Validate(domainContract), Times.Once());
            fixture.DomainLogicMock.Verify(x => x.Copy(domainContract), Times.Never());
            fixture.DomainLogicMock.Verify(x => x.Create(createdBy), Times.Never());
        }

        [Test]
        public void DomainServiceCreateDomain_InputValid_ResultOK()
        {
            // Arrange
            var fixture = new DomainServiceFixture();
            var createdBy = "andrzej.prazmo";
            var domainContract = new DomainContract { };
            fixture.DomainLogicMock.Setup(x => x.Copy(domainContract));
            fixture.DomainLogicMock.Setup(x => x.Validate(domainContract)).Returns(new Common.OperationResult());
            fixture.DomainLogicMock.Setup(x => x.Create(createdBy));

            fixture.DomainRepositoryMock.Setup(x => x.CreateDomainLogic()).Returns(fixture.DomainLogicMock.Object);
            // Act
            var sut = fixture.CreateSut();
            var result = sut.CreateDomain(domainContract, createdBy);

            // Assert
            Assert.True(result.Succeeded);
            fixture.DomainLogicMock.Verify(x => x.Validate(domainContract), Times.Once());
            fixture.DomainLogicMock.Verify(x => x.Copy(domainContract), Times.Once());
            fixture.DomainLogicMock.Verify(x => x.Create(createdBy), Times.Once());
        }

        [Test]
        public void DomainServiceUpdateDomain_InputNotValid_ResultFail()
        {
            // Arrange
            var fixture = new DomainServiceFixture();
            var updatedBy = "andrzej.prazmo";
            var domainContract = new DomainContract { };
            fixture.DomainLogicMock.Setup(x => x.Copy(domainContract));
            fixture.DomainLogicMock.Setup(x => x.Validate(domainContract)).Returns(new Common.OperationResult("ERROR"));
            fixture.DomainLogicMock.Setup(x => x.Update(updatedBy));

            fixture.DomainRepositoryMock.Setup(x => x.GetDomainLogic(It.IsAny<int>())).Returns(fixture.DomainLogicMock.Object);
            // Act
            var sut = fixture.CreateSut();
            var result = sut.UpdateDomain(domainContract, updatedBy);

            // Assert
            Assert.False(result.Succeeded);
            fixture.DomainLogicMock.Verify(x => x.Validate(domainContract), Times.Once());
            fixture.DomainLogicMock.Verify(x => x.Copy(domainContract), Times.Never());
            fixture.DomainLogicMock.Verify(x => x.Update(updatedBy), Times.Never());
        }

        [Test]
        public void DomainServiceUpdateDomain_InputValid_ResultOK()
        {
            // Arrange
            var fixture = new DomainServiceFixture();
            var updatedBy = "andrzej.prazmo";
            var domainContract = new DomainContract { };
            fixture.DomainLogicMock.Setup(x => x.Copy(domainContract));
            fixture.DomainLogicMock.Setup(x => x.Validate(domainContract)).Returns(new Common.OperationResult());
            fixture.DomainLogicMock.Setup(x => x.Update(updatedBy));

            fixture.DomainRepositoryMock.Setup(x => x.GetDomainLogic(It.IsAny<int>())).Returns(fixture.DomainLogicMock.Object);
            // Act
            var sut = fixture.CreateSut();
            var result = sut.UpdateDomain(domainContract, updatedBy);

            // Assert
            Assert.True(result.Succeeded);
            fixture.DomainLogicMock.Verify(x => x.Validate(domainContract), Times.Once());
            fixture.DomainLogicMock.Verify(x => x.Copy(domainContract), Times.Once());
            fixture.DomainLogicMock.Verify(x => x.Update(updatedBy), Times.Once());
        }
    }
}
