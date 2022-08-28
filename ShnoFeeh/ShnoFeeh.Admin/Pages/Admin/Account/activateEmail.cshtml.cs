using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShnoFeeh.BusinessService.Interfaces;
using ShnoFeeh.BusinessService.Common.Manager;
using System.Buffers.Text;

namespace ShnoFeehAdmin.Pages.Account
{
    public class activateEmailModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly ISessionManager _sessionManager;
        
        public activateEmailModel(ISessionManager sessionManager, IAccountService accountService)
        {
            _sessionManager = sessionManager;
            _accountService = accountService;            
        }

        public string Email { get; set; }
        public async Task<IActionResult> OnGet(string code)
        {
            if (string.IsNullOrEmpty(code))
                return RedirectToPage("/Admin/Account/login");

            
            var response = await _accountService.VerifyEmailToken(code);

            if (response == null)
                TempData["Exist"] = "Cannot verify email at the moment, please contact us";
            else if (response.HasSucceeded)
                TempData["success"] = "Email verified, please login with your new email and password";
            else if (!response.HasSucceeded)
                TempData["Exist"] = "Cannot verify email at the moment, please contact us";

            return RedirectToPage("/Admin/Account/login");


        }
    }
}