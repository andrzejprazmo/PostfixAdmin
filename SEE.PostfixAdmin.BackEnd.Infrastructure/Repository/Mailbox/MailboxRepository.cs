using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SEE.PostfixAdmin.BackEnd.Common.Contracts;
using SEE.PostfixAdmin.BackEnd.Common.List;
using SEE.PostfixAdmin.BackEnd.Common.Requests;
using SEE.PostfixAdmin.BackEnd.Infrastructure;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Logic;
using Microsoft.EntityFrameworkCore;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Repository.Configuration;
using SEE.PostfixAdmin.BackEnd.Common;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Model;

namespace SEE.PostfixAdmin.BackEnd.Infrastructure.Repository.Mailbox
{
    public class MailboxRepository : IMailboxRepository
    {
        #region members & ctor
        private readonly DataContext _dataContext;
        private readonly IConfigurationRepository _configuration;
        public MailboxRepository(DataContext dataContext, IConfigurationRepository configuration)
        {
            _dataContext = dataContext;
            _configuration = configuration;
        }

        #endregion

        public MailboxLogic CreateMailboxLogic() => MailboxLogic.Create(_dataContext, _configuration);

        public MailboxLogic GetMailboxLogic(int id) => MailboxLogic.Create(_dataContext, _configuration, id);

        public PasswordContract GetAdmin(string userName)
        {
            var mailbox = _dataContext.Mailboxes
                .Where(x => x.UserName + "@" + x.Domain.Name == userName && x.IsAdmin && x.IsActive)
                .Select(x => new PasswordContract
                {
                    UserId = x.Id,
                    UserName = x.UserName,
                    DomainName = x.Domain.Name,
                    Password = x.Password,
                }).SingleOrDefault();
            return mailbox;
        }

        public DataResponse<MailboxRequest, MailboxContract> FindBy(MailboxRequest request)
        {
            var query = _dataContext.Mailboxes.AsNoTracking();

            #region filtering
            if (request.DomainId.HasValue)
            {
                query = query.Where(x => x.DomainId == request.DomainId);
            }
            if (!string.IsNullOrWhiteSpace(request.UserName))
            {
                query = query.Where(x => x.UserName.Contains(request.UserName));
            }
            if (request.IsAdmin)
            {
                query = query.Where(x => x.IsAdmin);
            }
            #endregion

            #region sorting
            switch (request.SortColumn)
            {
                case "Name":
                default:
                    switch (request.SortDirection)
                    {
                        case SortDirection.Descending:
                            query = query.OrderByDescending(x => x.UserName);
                            break;
                        case SortDirection.Ascending:
                        default:
                            query = query.OrderBy(x => x.UserName);
                            break;
                    }
                    break;
            }
            #endregion

            request.TotalRecords = query.Count();
            var list = query.Select(x => new MailboxContract
            {
                Id = x.Id,
                UserName = x.UserName,
                DomainName = x.Domain.Name,
                Quota = x.Quota,
                IsAdmin = x.IsAdmin,
                IsActive = x.IsActive,
                CreateDate = x.CreateDate,
                CreatedBy = x.CreatedBy,
                UpdateDate = x.UpdateDate,
                UpdatedBy = x.UpdatedBy,
                DomainId = x.DomainId,
                AliasesCount = x.Aliases.Count(),
            }).Skip(request.Offset).Take(request.PageSize).ToList();

            return new DataResponse<MailboxRequest, MailboxContract>(request, list);
        }

        public PasswordLogic CreatePasswordLogic() => PasswordLogic.Create(_configuration.PasswordConfiguration);

        public PasswordContract GetPasswordContract(int id)
        {
            return _dataContext.Mailboxes.Where(x => x.Id == id).Select(x => new PasswordContract
            {
                UserId = x.Id,
                UserName = x.UserName,
                DomainName = x.Domain.Name,
                Password = x.Password,
            }).Single();
        }

        public OperationResult RemoveMailbox(int id)
        {
            var aliases = _dataContext.Aliases.Where(x => x.MailboxId == id).ToList();
            foreach (var aliasEntity in aliases)
            {
                _dataContext.Aliases.Remove(aliasEntity);
            }
            var mailboxEntity = _dataContext.Mailboxes.Single(x => x.Id == id);
            _dataContext.Mailboxes.Remove(mailboxEntity);
            _dataContext.SaveChanges();
            return new OperationResult();
        }

        public StartupLogic CreateStartupLogic() => StartupLogic.Create(_dataContext);

        public bool AccountsExist() => _dataContext.Mailboxes.Any();

        public StatsContract GetStats()
        {
            return new StatsContract
            {
                NumberOfDomains = _dataContext.Domains.Count(),
                NumberOfMailboxes = _dataContext.Mailboxes.Count(),
            };
        }
    }
}
