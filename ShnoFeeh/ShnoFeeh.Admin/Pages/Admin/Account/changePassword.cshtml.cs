using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.BusinessService.Common.Constant;
using ShnoFeeh.BusinessService.Common.Manager;
using ShnoFeeh.BusinessService.Interfaces;

namespace ShnoFeeh.Admin.Pages.Account
{

    [Authorize(Roles = "Admin,Company")]
    public class changePasswordModel : PageModel
    {
        public readonly ISessionManager _sessionManager;
        private readonly IAccountService _accountService;

        #region PageProperties
        [ViewData]
        public string SuccessMessage { get; set; }
        [ViewData]
        public string ErrorMessage { get; set; }

        [BindProperty]
        public UserPasswordDto userPasswordDto { get; set; }

        #endregion


        public changePasswordModel(ISessionManager sessionManager, IAccountService accountService)
        {
            _sessionManager = sessionManager;
            _accountService = accountService;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostChangePasswordAsync()
        {
            if (ModelState.IsValid)
            {
                userPasswordDto.UserId = _sessionManager.GetInt(SessionItems.Id) ?? 0;
                if (userPasswordDto.NewPassword == userPasswordDto.ConfirmPassword)
                {
                    var response = await _accountService.ChangePasswordAsync(userPasswordDto);
                    if (response.HasSucceeded)
                    {
                        if (response.Message.Contains("success"))
                        {
                            SuccessMessage = "Password updated successfully !";
                        }
                        else if (response.Message.Contains("match"))
                        {
                            ErrorMessage = "Current password is not correct.";
                        }
                        else
                        {
                            ErrorMessage = "Some Error Occured : " + response.Message;
                        }
                    }
                    else
                    {
                        ErrorMessage = "Some Error Occured : " + response.Message;
                    }
                }
                else
                {
                    ErrorMessage = "New and Confirm password should be same.";
                }
                return Page();
            }
            else
            {
                ErrorMessage = "Cannot process your request. Please try again.";
                return Page();
            }
        }
    }
}
