using SEE.PostfixAdmin.BackEnd.Infrastructure.Logic;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Repository.Mailbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SEE.PostfixAdmin.BackEnd.Common;
using SEE.PostfixAdmin.BackEnd.Common.Contracts;
using SEE.PostfixAdmin.BackEnd.Common.List;
using SEE.PostfixAdmin.BackEnd.Common.Requests;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Repository.Alias;
using Microsoft.Extensions.Logging;

namespace SEE.PostfixAdmin.BackEnd.BLL.Mailbox
{
    public class MailboxService : IMailboxService
    {
        #region members & ctor
        private readonly IMailboxRepository _mailboxRepository;
        private readonly IAliasRepository _aliasRepository;
        private readonly ILogger _logger;

        public MailboxService(IMailboxRepository mailboxRepository, IAliasRepository aliasRepository, ILogger logger)
        {
            _mailboxRepository = mailboxRepository;
            _aliasRepository = aliasRepository;
            _logger = logger;
        }

        #endregion

        public OperationResult<int> CreateMailbox(RegisterContract contract, string createdBy)
        {
            try
            {
                MailboxLogic mailboxLogic = _mailboxRepository.CreateMailboxLogic();
                PasswordLogic passwordLogic = _mailboxRepository.CreatePasswordLogic();

                var passwordResult = passwordLogic.Validate(new PasswordContract { Password = contract.Password, Confirm = contract.Confirm });
                var validationResult = mailboxLogic.Validate(contract);
                if (passwordResult.Succeeded && validationResult.Succeeded)
                {
                    mailboxLogic.Copy(contract);
                    mailboxLogic.Create(createdBy, passwordLogic.Hash(contract.Confirm));
                    return new OperationResult<int>(mailboxLogic.Id);
                }
                validationResult.Errors.AddRange(passwordResult.Errors);
                return new OperationResult<int>(validationResult.Errors);
            }
            catch (Exception e)
            {
                _logger.LogError("IMailboxService => CreateMailbox", e);
                return new OperationResult<int>(e);
            }
        }

        public DataResponse<MailboxRequest, MailboxContract> FindMailboxesBy(MailboxRequest request) => _mailboxRepository.FindBy(request);

        public OperationResult RemoveMailbox(int id)
        {
            try
            {
                return _mailboxRepository.RemoveMailbox(id);

            }
            catch (Exception e)
            {
                _logger.LogError("IMailboxService => RemoveMailbox", e);
                return new OperationResult(e);
            }
        }

        public MailboxContract GetMailboxById(int id) => _mailboxRepository.GetMailboxLogic(id);

        public OperationResult<MailboxContract> GetMailboxDetails(int id)
        {
            try
            {
                MailboxLogic mailboxLogic = _mailboxRepository.GetMailboxLogic(id);
                mailboxLogic.Aliases = _aliasRepository.FindByMailbox(id);
                return new OperationResult<MailboxContract>(mailboxLogic);
            }
            catch (Exception e)
            {
                _logger.LogError("IMailboxService => GetMailboxDetails", e);
                return new OperationResult<MailboxContract>(e);
            }
        }

        public OperationResult<int> UpdateMailbox(MailboxContract contract, string updatedBy)
        {
            try
            {
                var mailboxLogic = _mailboxRepository.GetMailboxLogic(contract.Id);
                var validationResult = mailboxLogic.Validate(contract);
                if (validationResult.Succeeded)
                {
                    mailboxLogic.Copy(contract);
                    mailboxLogic.Update(updatedBy);
                    return new OperationResult<int>(mailboxLogic.Id);
                }
                return new OperationResult<int>(validationResult.Errors);
            }
            catch (Exception e)
            {
                _logger.LogError("IMailboxService => UpdateMailbox", e);
                return new OperationResult<int>(e);
            }
        }

        public PasswordContract GetPasswordContract(int id) => _mailboxRepository.GetPasswordContract(id);

        public OperationResult<int> ChangePassword(PasswordContract contract, string updatedBy)
        {
            try
            {
                var passwordLogic = _mailboxRepository.CreatePasswordLogic();
                var validationResult = passwordLogic.Validate(contract);
                if (validationResult.Succeeded)
                {
                    var mailboxLogic = _mailboxRepository.GetMailboxLogic(contract.UserId);
                    mailboxLogic.SetPassword(contract.UserId, passwordLogic.Hash(contract.Confirm), updatedBy);
                    return new OperationResult<int>(contract.UserId);
                }
                return new OperationResult<int>(validationResult.Errors);
            }
            catch (Exception e)
            {
                _logger.LogError("IMailboxService => ChangePassword", e);
                return new OperationResult<int>(e);
            }
        }

        public StatsContract GetStats() => _mailboxRepository.GetStats();


    }
}
