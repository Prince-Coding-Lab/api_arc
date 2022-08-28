using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.BusinessService.Common.Manager;
using ShnoFeeh.BusinessService.Dto;
using ShnoFeeh.BusinessService.Interfaces;

namespace ShnoFeeh.Admin.Pages.Account
{
    public class forgot_passwordModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly ISessionManager _sessionManager;

        #region PageProperties
        [ViewData]
        public string ErrorMessage { get; set; }

        [ViewData]
        public string SuccessMessage { get; set; }

        [BindProperty]
        public UserEmailDto UserEmailDto { get; set; }

        #endregion

        public forgot_passwordModel(ISessionManager sessionManager, IAccountService accountService)
        {
            _sessionManager = sessionManager;
            _accountService = accountService;
        }

        public void OnGet()
        {
           
        }
        public async Task<IActionResult> OnPostForgetPasswordAsync()
        {
            if (ModelState.IsValid)
            {
                var response = await _accountService.ForgotPasswordAsync(UserEmailDto);
                if (response != null)
                {
                    if (response.HasSucceeded == true)
                    {
                        SuccessMessage = response.Message;
                        return Page();
                    }
                    else
                    {
                        ErrorMessage = "Some error occured : " + (response.Message.Contains("not exists") ? "Email not found." : response.Message);
                        return Page();
                    }
                }
                else
                {
                    ErrorMessage = "Wrong Email id";
                    return Page();
                }
            }
            else
            {
                ErrorMessage = "Cannot process your request. Please try again.";
                return Page();
            }
        }
    }
}
