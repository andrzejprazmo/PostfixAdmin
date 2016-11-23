using SEE.PostfixAdmin.BackEnd.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SEE.PostfixAdmin.BackEnd.Common.Configuration;

namespace SEE.PostfixAdmin.BackEnd.Infrastructure.Repository.Configuration
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly PasswordConfiguration _passwordConfiguration;
        public ConfigurationRepository(IOptions<PasswordConfiguration> passwordConfiguration)
        {
            _passwordConfiguration = passwordConfiguration.Value;
        }

        public PasswordConfiguration PasswordConfiguration
        {
            get
            {
                return _passwordConfiguration;
            }
        }
    }
}
