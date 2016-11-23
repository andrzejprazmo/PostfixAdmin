using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SEE.PostfixAdmin.BackEnd.BLL.Mailbox;
using SEE.PostfixAdmin.BackEnd.Common.Requests;
using SEE.PostfixAdmin.BackEnd.BLL.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using SEE.PostfixAdmin.BackEnd.Common.Contracts;

namespace SEE.PostfixAdmin.FrontEnd.Controllers
{
    [Authorize]
    public class MailboxController : Controller
    {
        #region members & ctor
        private readonly IMailboxService _mailboxService;
        private readonly IDomainService _domainService;
        public MailboxController(IMailboxService mailboxService, IDomainService domainService)
        {
            _mailboxService = mailboxService;
            _domainService = domainService;
        }

        #endregion

        #region list & details
        public IActionResult Index(MailboxRequest request)
        {
            var result = _mailboxService.FindMailboxesBy(request);
            return View(result);
        }
        public IActionResult Details(int id)
        {
            var result = _mailboxService.GetMailboxDetails(id);
            return View(result.Value);
        }

        #endregion

        #region create
        public IActionResult Create(int? DomainId)
        {
            ViewBag.DomainsDictionary = new SelectList(_domainService.GetDomainsDictionary(), "Key", "Value");
            return View(new RegisterContract
            {
                DomainId = DomainId ?? 0,
                IsActive = true,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RegisterContract mailbox)
        {
            ModelState.Clear();
            var result = _mailboxService.CreateMailbox(mailbox, User.Identity.Name);
            if (result.Succeeded)
            {
                return RedirectToAction("Details", new { id = result.Value });
            }
            ViewBag.DomainsDictionary = new SelectList(_domainService.GetDomainsDictionary(), "Key", "Value");
            result.GetAllErrors().ForEach(x => ModelState.AddModelError(x.PropertyName, x.ErrorMessage));
            return View(mailbox);
        }

        #endregion

        #region edit
        public IActionResult Edit(int id)
        {
            ViewBag.DomainsDictionary = new SelectList(_domainService.GetDomainsDictionary(), "Key", "Value");
            return View(_mailboxService.GetMailboxById(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MailboxContract contract)
        {
            ModelState.Clear();
            var result = _mailboxService.UpdateMailbox(contract, User.Identity.Name);
            if (result.Succeeded)
            {
                return RedirectToAction("Details", new { id = result.Value });
            }
            ViewBag.DomainsDictionary = new SelectList(_domainService.GetDomainsDictionary(), "Key", "Value");
            result.GetAllErrors().ForEach(x => ModelState.AddModelError(x.PropertyName, x.ErrorMessage));
            return View(contract);
        }
        #endregion

        #region remove
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int id)
        {
            var result = _mailboxService.RemoveMailbox(id);
            if (!result.Succeeded)
            {
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region password
        public IActionResult Password(int id)
        {
            return View(_mailboxService.GetPasswordContract(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Password(PasswordContract contract)
        {
            ModelState.Clear();
            var result = _mailboxService.ChangePassword(contract, User.Identity.Name);
            if (result.Succeeded)
            {
                return RedirectToAction("PasswordChanged", new { id = result.Value });
            }
            result.GetAllErrors().ForEach(x => ModelState.AddModelError(x.PropertyName, x.ErrorMessage));
            return View(contract);
        }

        public IActionResult PasswordChanged(int id)
        {
            return View(_mailboxService.GetPasswordContract(id));
        }

        #endregion
    }
}
