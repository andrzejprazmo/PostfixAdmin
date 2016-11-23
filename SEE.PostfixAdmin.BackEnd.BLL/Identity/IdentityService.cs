using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SEE.PostfixAdmin.BackEnd.Common;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using SEE.PostfixAdmin.BackEnd.Common.Const;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Repository.Mailbox;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Repository.Configuration;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Logic;
using SEE.PostfixAdmin.BackEnd.Common.Contracts;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Repository.Domain;
using Microsoft.Extensions.Logging;

namespace SEE.PostfixAdmin.BackEnd.BLL.Identity
{
    public class IdentityService : IIdentityService
    {
        #region members & ctor
        private readonly IConfigurationRepository _configuration;
        private readonly IDomainRepository _domainRepository;
        private readonly IMailboxRepository _mailboxRepository;
        private readonly ILogger<IdentityService> _logger;
        public IdentityService(IMailboxRepository mailboxRepository, IConfigurationRepository configuration, IDomainRepository domainRepository, ILogger<IdentityService> logger)
        {
            _mailboxRepository = mailboxRepository;
            _configuration = configuration;
            _domainRepository = domainRepository;
            _logger = logger;
        }

        #endregion

        public OperationResult<ClaimsPrincipal> Validate(string userName, string password)
        {
            _logger.LogInformation("Trying to log in.", userName);
            var mailbox = _mailboxRepository.GetAdmin(userName);
            if (mailbox != null)
            {
                PasswordLogic passwordLogic = _mailboxRepository.CreatePasswordLogic();
                string encPassword = passwordLogic.Hash(password);
                if (string.Compare(encPassword, mailbox.Password) == 0)
                {
                    _logger.LogInformation("Log in succeeded.", userName);
                    var identity = CreateClaimsIdentity($"{mailbox.UserName}@{mailbox.DomainName}", mailbox.UserId.ToString());
                    return new OperationResult<ClaimsPrincipal>
                    {
                        Value = new ClaimsPrincipal(identity),
                    };
                }
                _logger.LogInformation("Log in failed because of bad password.", userName);
                return new OperationResult<ClaimsPrincipal>(new ValidationResult("Bad password", new string[] { "Password" }));
            }
            _logger.LogInformation("Log in failed because of bad user name.", userName);
            return new OperationResult<ClaimsPrincipal>(new ValidationResult("There is no user with that name.", new string[] { "UserName" }));
        }

        private ClaimsIdentity CreateClaimsIdentity(string userName, string userId)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", userName), 
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", userId), 
                new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", AppSettings.IdentityInstanceCookieName),
            };
            ClaimsIdentity identity = new ClaimsIdentity(claims, "ApplicationCookie", ClaimTypes.Name, ClaimTypes.Role);
            return identity;
        }

        public bool AccountsExist() => _mailboxRepository.AccountsExist();

        public OperationResult RegisterMailbox(StartupContract contract)
        {
            try
            {
                var startupLogic = _mailboxRepository.CreateStartupLogic();
                var passwordLogic = _mailboxRepository.CreatePasswordLogic();
                var validationResult = startupLogic.Validate(contract);
                var passwordResult = passwordLogic.Validate(new PasswordContract
                {
                    Password = contract.Password,
                    Confirm = contract.Confirm,
                });
                if (validationResult.Succeeded)
                {
                    startupLogic.Copy(contract);
                    startupLogic.Create(passwordLogic.Hash(contract.Confirm));
                    return new OperationResult();
                }
                return new OperationResult(validationResult.Errors);
            }
            catch (Exception e)
            {
                _logger.LogError("IIdentityService => RegisterMailbox", e);
                return new OperationResult(e);
            }
        }
    }
}
