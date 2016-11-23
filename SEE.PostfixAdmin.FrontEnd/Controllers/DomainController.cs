using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SEE.PostfixAdmin.BackEnd.Common.Contracts;
using SEE.PostfixAdmin.BackEnd.Common.Requests;
using SEE.PostfixAdmin.BackEnd.BLL.Domain;

namespace SEE.PostfixAdmin.FrontEnd.Controllers
{
    [Authorize]
    public class DomainController : Controller
    {
        #region members & ctor
        private readonly IDomainService _domainService;
        public DomainController(IDomainService domainService)
        {
            _domainService = domainService;
        }

        #endregion

        #region list & details
        public IActionResult Index(DomainRequest filter)
        {
            var result = _domainService.FindDomainsBy(filter);
            return View(result);
        }

        public IActionResult Details(int id)
        {
            var model = _domainService.GetDomainById(id);
            return View(model.Value);
        }
        #endregion

        #region create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DomainContract model)
        {
            ModelState.Clear();
            var result = _domainService.CreateDomain(model, User.Identity.Name);
            if (result.Succeeded)
            {
                return RedirectToAction("Details", new { id = result.Value });
            }
            result.GetAllErrors().ForEach(x => ModelState.AddModelError(x.PropertyName, x.ErrorMessage));
            return View(model);
        }

        #endregion

        #region edit
        public IActionResult Edit(int id)
        {
            var model = _domainService.GetDomainById(id);
            return View(model.Value);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(DomainContract model)
        {
            ModelState.Clear();
            var result = _domainService.UpdateDomain(model, User.Identity.Name);
            if (result.Succeeded)
            {
                return RedirectToAction("Details", new { id = result.Value });
            }
            result.GetAllErrors().ForEach(x => ModelState.AddModelError(x.PropertyName, x.ErrorMessage));
            return View(model);
        }

        #endregion

        #region remove

        [HttpPost]
        public IActionResult Remove(int id)
        {
            var result = _domainService.RemoveDomain(id);
            if (!result.Succeeded)
            {
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Index");
        }
        #endregion

    }
}
