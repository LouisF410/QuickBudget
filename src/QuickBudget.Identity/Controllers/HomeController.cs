using Microsoft.AspNetCore.Mvc;
using QuickBudget.Identity.Models;
using QuickBudget.Identity.Services;
using System.Diagnostics;

namespace Identity.API.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRedirectService _redirectSvc;

        public HomeController(IRedirectService redirectSvc)
        {
            _redirectSvc = redirectSvc;
        }

        public IActionResult Index(string returnUrl)
        {
            return View();
        }

        public IActionResult ReturnToOriginalApplication(string returnUrl)
        {
            if (returnUrl != null)
                return Redirect(_redirectSvc.ExtractRedirectUriFromReturnUrl(returnUrl));
            else
                return RedirectToAction("Index", "Home");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
