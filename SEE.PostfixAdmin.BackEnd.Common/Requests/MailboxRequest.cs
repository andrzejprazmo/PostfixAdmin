using SEE.PostfixAdmin.BackEnd.Common.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEE.PostfixAdmin.BackEnd.Common.Requests
{
    public class MailboxRequest : DataRequest
    {
        public int? DomainId { get; set; }
        public string UserName { get; set; }
        public bool IsAdmin { get; set; }

    }
}
