using Moq;
using NUnit.Framework;
using SEE.PostfixAdmin.BackEnd.BLL.UnitTest.Fixtures;
using SEE.PostfixAdmin.BackEnd.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEE.PostfixAdmin.BackEnd.BLL.UnitTest
{
    public class AliasServiceUnitTest
    {
        [Test]
        public void AliasServiceCreateAlias_InputNotValid_ResultFail()
        {
            // Arrange
            var fixture = new AliasServiceFixture();
            var createdBy = "andrzej.prazmo";
            var aliasContract = new AliasContract { };
            fixture.AliasLogicMock.Setup(x => x.Copy(aliasContract));
            fixture.AliasLogicMock.Setup(x => x.Validate(aliasContract)).Returns(new Common.OperationResult("ERROR"));
            fixture.AliasLogicMock.Setup(x => x.Create(createdBy));

            fixture.AliasRepositoryMock.Setup(x => x.CreateAliasLogic()).Returns(fixture.AliasLogicMock.Object);
            // Act
            var sut = fixture.CreateSut();
            var result = sut.CreateAlias(aliasContract, createdBy);

            // Assert
            Assert.False(result.Succeeded);
            fixture.AliasLogicMock.Verify(x => x.Validate(aliasContract), Times.Once());
            fixture.AliasLogicMock.Verify(x => x.Copy(aliasContract), Times.Never());
            fixture.AliasLogicMock.Verify(x => x.Create(createdBy), Times.Never());
        }

        [Test]
        public void AliasServiceCreateAlias_InputValid_ResultOK()
        {
            // Arrange
            var fixture = new AliasServiceFixture();
            var createdBy = "andrzej.prazmo";
            var aliasContract = new AliasContract { };
            fixture.AliasLogicMock.Setup(x => x.Copy(aliasContract));
            fixture.AliasLogicMock.Setup(x => x.Validate(aliasContract)).Returns(new Common.OperationResult());
            fixture.AliasLogicMock.Setup(x => x.Create(createdBy));

            fixture.AliasRepositoryMock.Setup(x => x.CreateAliasLogic()).Returns(fixture.AliasLogicMock.Object);
            // Act
            var sut = fixture.CreateSut();
            var result = sut.CreateAlias(aliasContract, createdBy);

            // Assert
            Assert.True(result.Succeeded);
            fixture.AliasLogicMock.Verify(x => x.Validate(aliasContract), Times.Once());
            fixture.AliasLogicMock.Verify(x => x.Copy(aliasContract), Times.Once());
            fixture.AliasLogicMock.Verify(x => x.Create(createdBy), Times.Once());
        }

        [Test]
        public void AliasServiceUpdateAlias_InputNotValid_ResultFail()
        {
            // Arrange
            var fixture = new AliasServiceFixture();
            var updatedBy = "andrzej.prazmo";
            var aliasContract = new AliasContract { };
            fixture.AliasLogicMock.Setup(x => x.Copy(aliasContract));
            fixture.AliasLogicMock.Setup(x => x.Validate(aliasContract)).Returns(new Common.OperationResult("ERROR"));
            fixture.AliasLogicMock.Setup(x => x.Update(updatedBy));

            fixture.AliasRepositoryMock.Setup(x => x.GetAliasLogic(It.IsAny<int>())).Returns(fixture.AliasLogicMock.Object);
            // Act
            var sut = fixture.CreateSut();
            var result = sut.UpdateAlias(aliasContract, updatedBy);

            // Assert
            Assert.False(result.Succeeded);
            fixture.AliasLogicMock.Verify(x => x.Validate(aliasContract), Times.Once());
            fixture.AliasLogicMock.Verify(x => x.Copy(aliasContract), Times.Never());
            fixture.AliasLogicMock.Verify(x => x.Update(updatedBy), Times.Never());
        }

        [Test]
        public void AliasServiceUpdateAlias_InputValid_ResultOK()
        {
            // Arrange
            var fixture = new AliasServiceFixture();
            var updatedBy = "andrzej.prazmo";
            var aliasContract = new AliasContract { };
            fixture.AliasLogicMock.Setup(x => x.Copy(aliasContract));
            fixture.AliasLogicMock.Setup(x => x.Validate(aliasContract)).Returns(new Common.OperationResult());
            fixture.AliasLogicMock.Setup(x => x.Update(updatedBy));

            fixture.AliasRepositoryMock.Setup(x => x.GetAliasLogic(It.IsAny<int>())).Returns(fixture.AliasLogicMock.Object);
            // Act
            var sut = fixture.CreateSut();
            var result = sut.UpdateAlias(aliasContract, updatedBy);

            // Assert
            Assert.True(result.Succeeded);
            fixture.AliasLogicMock.Verify(x => x.Validate(aliasContract), Times.Once());
            fixture.AliasLogicMock.Verify(x => x.Copy(aliasContract), Times.Once());
            fixture.AliasLogicMock.Verify(x => x.Update(updatedBy), Times.Once());
        }
    }
}
