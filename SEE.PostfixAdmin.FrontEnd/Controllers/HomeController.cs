using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SEE.PostfixAdmin.BackEnd.BLL.Domain;
using SEE.PostfixAdmin.BackEnd.Common.Contracts;
using SEE.PostfixAdmin.BackEnd.BLL.Mailbox;

namespace SEE.PostfixAdmin.FrontEnd.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        #region members & ctor
        private readonly IMailboxService _mailboxService;
        public HomeController(IMailboxService mailboxService)
        {
            _mailboxService = mailboxService;
        }

        #endregion

        public IActionResult Index() => View(_mailboxService.GetStats());

        public IActionResult Configuration() => View();

        public IActionResult Error() => View();
    }
}
