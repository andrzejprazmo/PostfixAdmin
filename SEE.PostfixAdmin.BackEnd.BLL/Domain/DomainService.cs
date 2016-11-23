using Microsoft.EntityFrameworkCore;
using SEE.PostfixAdmin.BackEnd.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SEE.PostfixAdmin.BackEnd.Common;
using System.ComponentModel.DataAnnotations;
using SEE.PostfixAdmin.BackEnd.Common.List;
using SEE.PostfixAdmin.BackEnd.Common.Requests;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Repository.Domain;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Logic;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Repository.Mailbox;
using Microsoft.Extensions.Logging;

namespace SEE.PostfixAdmin.BackEnd.BLL.Domain
{
    public class DomainService : IDomainService
    {
        #region members & ctor
        private readonly IDomainRepository _domainRepository;
        private readonly IMailboxRepository _mailboxRepository;
        private readonly ILogger _logger;
        public DomainService(IDomainRepository domainRepository, IMailboxRepository mailboxRepository, ILogger<DomainService> logger)
        {
            _domainRepository = domainRepository;
            _mailboxRepository = mailboxRepository;
            _logger = logger;
        }

        #endregion

        public OperationResult<int> CreateDomain(DomainContract contract, string createdBy)
        {
            try
            {
                DomainLogic domainLogic = _domainRepository.CreateDomainLogic();
                var validationResult = domainLogic.Validate(contract);
                if (validationResult.Succeeded)
                {
                    domainLogic.Copy(contract);
                    domainLogic.Create(createdBy);
                    return new OperationResult<int>(domainLogic.Id);
                }
                return new OperationResult<int>(validationResult.Errors);
            }
            catch (Exception e)
            {
                _logger.LogError("IDomainService => CreateDomain", e);
                return new OperationResult<int>(e);
            }
        }

        public OperationResult<DomainContract> GetDomainById(int id)
        {
            try
            {
                DomainLogic domainLogic = _domainRepository.GetDomainLogic(id);
                var mailboxList = _mailboxRepository.FindBy(new MailboxRequest { DomainId = id });
                domainLogic.Mailboxes = mailboxList.List;
                return new OperationResult<DomainContract>(domainLogic);
            }
            catch (Exception e)
            {
                _logger.LogError("IDomainService => GetDomainById", e);
                return new OperationResult<DomainContract>(e);
            }
        }

        public DataResponse<DomainRequest, DomainContract> FindDomainsBy(DomainRequest request) => _domainRepository.FindBy(request);

        public Dictionary<int, string> GetDomainsDictionary() => _domainRepository.GetDomainsDictionary();

        public OperationResult<int> UpdateDomain(DomainContract contract, string updatedBy)
        {
            try
            {
                var domainLogic = _domainRepository.GetDomainLogic(contract.Id);
                var validationResult = domainLogic.Validate(contract);
                if (validationResult.Succeeded)
                {
                    domainLogic.Copy(contract);
                    domainLogic.Update(updatedBy);
                    return new OperationResult<int>(domainLogic.Id);
                }
                return new OperationResult<int>(validationResult.Errors);
            }
            catch (Exception e)
            {
                _logger.LogError("IDomainService => UpdateDomain", e);
                return new OperationResult<int>(e);
            }
        }

        public OperationResult RemoveDomain(int id)
        {
            try
            {
                return _domainRepository.RemoveDomain(id);
            }
            catch (Exception e)
            {
                _logger.LogError("IDomainService => RemoveDomain", e);
                return new OperationResult(e);
            }
        }
    }
}
