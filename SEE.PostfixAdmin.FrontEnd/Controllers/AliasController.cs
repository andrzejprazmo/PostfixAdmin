using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SEE.PostfixAdmin.BackEnd.BLL.Alias;
using SEE.PostfixAdmin.BackEnd.Common.Contracts;
using SEE.PostfixAdmin.BackEnd.BLL.Mailbox;
using Microsoft.AspNetCore.Authorization;

namespace SEE.PostfixAdmin.FrontEnd.Controllers
{
    [Authorize]
    public class AliasController : Controller
    {
        #region members & ctor
        private readonly IAliasService _aliasService;
        private readonly IMailboxService _mailboxService;
        public AliasController(IAliasService aliasService, IMailboxService mailboxService)
        {
            _aliasService = aliasService;
            _mailboxService = mailboxService;
        }

        #endregion

        #region create
        public IActionResult Create(int MailboxId)
        {
            var mailboxContract = _mailboxService.GetMailboxById(MailboxId);
            return View(new AliasContract
            {
                MailboxId = MailboxId,
                UserName = mailboxContract.UserName,
                DomainName = mailboxContract.DomainName,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AliasContract contract)
        {
            ModelState.Clear();
            var result = _aliasService.CreateAlias(contract, User.Identity.Name);
            if (result.Succeeded)
            {
                return RedirectToAction("Details", "Mailbox", new { id = contract.MailboxId });
            }
            result.GetAllErrors().ForEach(x => ModelState.AddModelError(x.PropertyName, x.ErrorMessage));
            return View(contract);
        }
        #endregion

        #region edit
        public IActionResult Edit(int id)
        {
            return View(_aliasService.GetAlias(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AliasContract contract)
        {
            ModelState.Clear();
            var result = _aliasService.UpdateAlias(contract, User.Identity.Name);
            if (result.Succeeded)
            {
                return RedirectToAction("Details", "Mailbox", new { id = contract.MailboxId });
            }
            result.GetAllErrors().ForEach(x => ModelState.AddModelError(x.PropertyName, x.ErrorMessage));
            return View(contract);
        }
        #endregion

        #region remove

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int id)
        {
            var result = _aliasService.RemoveAlias(id);
            if (!result.Succeeded)
            {
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Details", "Mailbox", new { id = result.Value });
        }
        #endregion
    }
}
