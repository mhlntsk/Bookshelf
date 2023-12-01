using Bookshelf_FL.ViewModels;
using Bookshelf_TL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Bookshelf_FL.Controllers
{
    public class HomeController : Controller
    {
        UserManager<User> _userManager;
        public HomeController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                string id = _userManager.GetUserId(User);

                return RedirectToAction("GetUser", "User", new { userId = id });
            }

            return View();
        }
    }
}
