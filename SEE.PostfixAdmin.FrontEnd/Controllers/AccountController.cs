using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SEE.PostfixAdmin.BackEnd.Common.Contracts;
using SEE.PostfixAdmin.BackEnd.Common.Const;
using SEE.PostfixAdmin.BackEnd.BLL.Identity;

namespace SEE.PostfixAdmin.FrontEnd.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        #region members & ctor
        private readonly IIdentityService _identityService;
        public AccountController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        #endregion

        [AllowAnonymous]
        public IActionResult Forbidden() => View();

        #region login & logout
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginContract model, string ReturnUrl)
        {
            ModelState.Clear();
            var result = _identityService.Validate(model.UserName, model.Password);
            if (result.Succeeded)
            {
                HttpContext.Authentication.SignInAsync(AppSettings.IdentityInstanceCookieName, result.Value);
                return RedirectToLocal(ReturnUrl);
            }
            result.GetAllErrors().ForEach(x => ModelState.AddModelError(x.PropertyName, x.ErrorMessage));
            return View(model);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        public IActionResult Logoff()
        {
            HttpContext.Authentication.SignOutAsync(AppSettings.IdentityInstanceCookieName);
            return RedirectToAction(nameof(AccountController.Login), "Account");
        }

        #endregion

        #region register
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (_identityService.AccountsExist())
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(StartupContract contract)
        {
            ModelState.Clear();
            var result = _identityService.RegisterMailbox(contract);
            if (result.Succeeded)
            {
                return RedirectToAction("Registered");
            }
            result.GetAllErrors().ForEach(x => ModelState.AddModelError(x.PropertyName, x.ErrorMessage));
            return View(contract);
        }

        [AllowAnonymous]
        public IActionResult Registered() => View();

        #endregion
    }
}
