using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ShnoFeeh.BusinessService.Common.Constant;

namespace ShnoFeeh.Admin.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AdminLogout()
        {
            HttpContext.Session.Remove(SessionItems.UserName);
            HttpContext.Session.Remove(SessionItems.Token);
            HttpContext.Session.Remove(SessionItems.Id);
            HttpContext.Session.Remove("userInfo");
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            await HttpContext.SignOutAsync();

            return LocalRedirect("/Admin/Account/login");
        }
    }
}