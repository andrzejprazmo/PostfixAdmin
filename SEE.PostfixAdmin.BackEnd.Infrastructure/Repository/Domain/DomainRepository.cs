using SEE.PostfixAdmin.BackEnd.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SEE.PostfixAdmin.BackEnd.Common.List;
using SEE.PostfixAdmin.BackEnd.Common.Contracts;
using Microsoft.EntityFrameworkCore;
using SEE.PostfixAdmin.BackEnd.Common.Requests;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Logic;
using SEE.PostfixAdmin.BackEnd.Common;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Model;

namespace SEE.PostfixAdmin.BackEnd.Infrastructure.Repository.Domain
{
    public class DomainRepository : IDomainRepository
    {
        #region members & ctor
        private readonly DataContext _dataContext;
        public DomainRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        #endregion

        public DomainLogic CreateDomainLogic() => DomainLogic.Create(_dataContext);

        public DomainLogic GetDomainLogic(int id) => DomainLogic.Create(_dataContext, id);

        public DataResponse<DomainRequest, DomainContract> FindBy(DomainRequest request)
        {
            var query = _dataContext.Domains.AsNoTracking();

            #region filtering
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                query = query.Where(x => x.Name.Contains(request.Name));
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
                            query = query.OrderByDescending(x => x.Name);
                            break;
                        case SortDirection.Ascending:
                        default:
                            query = query.OrderBy(x => x.Name);
                            break;
                    }
                    break;
            }
            #endregion

            request.TotalRecords = query.Count();
            var list = query.Select(x => new DomainContract
            {
                Id = x.Id,
                Name = x.Name,
                MailboxCount = x.Mailboxes.Count(),
                CreateDate = x.CreateDate,
                CreatedBy = x.CreatedBy,
                UpdateDate = x.UpdateDate,
                UpdatedBy = x.UpdatedBy
            }).Skip(request.Offset).Take(request.PageSize).ToList();

            return new DataResponse<DomainRequest, DomainContract>(request, list);
        }

        public Dictionary<int, string> GetDomainsDictionary()
        {
            return _dataContext.Domains.OrderBy(x => x.Name).ToDictionary(x => x.Id, y => y.Name);
        }

        public OperationResult RemoveDomain(int id)
        {
            var domainEntity = _dataContext.Domains.Single(x => x.Id == id);
            var mailboxes = _dataContext.Mailboxes.Where(x => x.DomainId == id).ToList();
            foreach (var mailboxEntity in mailboxes)
            {
                var aliases = _dataContext.Aliases.Where(x => x.MailboxId == mailboxEntity.Id).ToList();
                foreach (var aliasEntity in aliases)
                {
                    _dataContext.Aliases.Remove(aliasEntity);
                }
                _dataContext.Mailboxes.Remove(mailboxEntity);
            }
            _dataContext.Domains.Remove(domainEntity);
            _dataContext.SaveChanges();
            return new OperationResult();
        }
    }
}
