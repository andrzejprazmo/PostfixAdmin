using SEE.PostfixAdmin.BackEnd.Common.Configuration;
using SEE.PostfixAdmin.BackEnd.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEE.PostfixAdmin.BackEnd.Infrastructure.Repository.Configuration
{
    public interface IConfigurationRepository
    {

        PasswordConfiguration PasswordConfiguration { get; }


    }
}
