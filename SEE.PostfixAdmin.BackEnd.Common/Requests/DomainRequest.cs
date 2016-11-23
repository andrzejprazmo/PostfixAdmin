using SEE.PostfixAdmin.BackEnd.Common.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEE.PostfixAdmin.BackEnd.Common.Requests
{
    public class DomainRequest : DataRequest
    {
        public string Name { get; set; }
    }
}
