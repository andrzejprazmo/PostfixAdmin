using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SEE.PostfixAdmin.BackEnd.Common;
using SEE.PostfixAdmin.BackEnd.Common.Contracts;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Logic;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Repository.Alias;
using Microsoft.Extensions.Logging;

namespace SEE.PostfixAdmin.BackEnd.BLL.Alias
{
    public class AliasService : IAliasService
    {
        #region members & ctor
        private readonly IAliasRepository _aliasRepository;
        private readonly ILogger _logger;

        public AliasService(IAliasRepository aliasRepository, ILogger<AliasService> logger)
        {
            _aliasRepository = aliasRepository;
            _logger = logger;
        }

        #endregion

        public OperationResult<int> CreateAlias(AliasContract contract, string createdBy)
        {
            try
            {
                AliasLogic aliasLogic = _aliasRepository.CreateAliasLogic();
                var validationResult = aliasLogic.Validate(contract);
                if (validationResult.Succeeded)
                {
                    aliasLogic.Copy(contract);
                    aliasLogic.Create(createdBy);
                    return new OperationResult<int>(aliasLogic.Id);
                }
                return new OperationResult<int>(validationResult.Errors);
            }
            catch (Exception e)
            {
                _logger.LogError("IAliasService => CreateAlias", e);
                return new OperationResult<int>(e);
            }
        }

        public OperationResult<int> RemoveAlias(int id)
        {
            try
            {
                return _aliasRepository.RemoveAlias(id);
            }
            catch (Exception e)
            {
                _logger.LogError("IAliasService => RemoveAlias", e);
                return new OperationResult<int>(e);
            }
        }

        public OperationResult<int> UpdateAlias(AliasContract contract, string updatedBy)
        {
            try
            {
                AliasLogic aliasLogic = _aliasRepository.GetAliasLogic(contract.Id);
                var validationResult = aliasLogic.Validate(contract);
                if (validationResult.Succeeded)
                {
                    aliasLogic.Copy(contract);
                    aliasLogic.Update(updatedBy);
                    return new OperationResult<int>(aliasLogic.Id);
                }
                return new OperationResult<int>(validationResult.Errors);
            }
            catch (Exception e)
            {
                _logger.LogError("IAliasService => UpdateAlias", e);
                return new OperationResult<int>(e);
            }
        }

        public AliasContract GetAlias(int id) => _aliasRepository.GetAliasLogic(id);
    }
}
