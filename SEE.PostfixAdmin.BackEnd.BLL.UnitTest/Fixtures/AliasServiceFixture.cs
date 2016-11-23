using Microsoft.Extensions.Logging;
using Moq;
using SEE.PostfixAdmin.BackEnd.BLL.Alias;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Logic;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Repository.Alias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEE.PostfixAdmin.BackEnd.BLL.UnitTest.Fixtures
{
    public class AliasServiceFixture
    {
        public Mock<IAliasRepository> AliasRepositoryMock = new Mock<IAliasRepository>();
        public Mock<ILogger<AliasService>> LoggerMock = new Mock<ILogger<AliasService>>();
        public Mock<AliasLogic> AliasLogicMock = new Mock<AliasLogic>();

        public IAliasService CreateSut()
        {
            return new AliasService(AliasRepositoryMock.Object, LoggerMock.Object);
        }
    }
}
