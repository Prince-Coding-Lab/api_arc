using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShnoFeeh.BusinessService.Dto;
using ShnoFeeh.BusinessService.Interfaces;
using ShnoFeeh.BusinessService.Common.Manager;
using System.Buffers.Text;
using ShnoFeeh.API.Core.Dto;

namespace ShnoFeehAdmin.Pages.Account
{    
    public class resetPasswordModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly ISessionManager _sessionManager;        

        #region PageProperties
        [BindProperty]
        public string ErrorMessage { get; set; }

        [BindProperty]
        public string SuccessMessage { get; set; }
        [BindProperty]
        public ResetPasswordDto userResetPasswordDto { get; set; }
        #endregion

        public resetPasswordModel(ISessionManager sessionManager, IAccountService accountService)
        {
            _sessionManager = sessionManager;
            _accountService = accountService;
         
        }
        
        public async Task OnGet(string code)
        {
            ResetPasswordDto userResetPasswordDto1 = new ResetPasswordDto();
            userResetPasswordDto1.code = code;
            TempData["code"] = code;
            this.userResetPasswordDto = userResetPasswordDto1;
        }
        public async Task<IActionResult> OnPostResetPasswordAsync()
        {

            if(!ModelState.IsValid)
            {
                this.ErrorMessage = "Cannot process your request at the moment";
            }


            if (TempData["code"] == null)
            {
                this.ErrorMessage = "Please verify your inputs";

                return Page();
            }


            if(userResetPasswordDto.ConfirmPassword != userResetPasswordDto.Password)
            {
                this.ErrorMessage = "Password does not match, please verify your inputs";

                return Page();
            }

            //TODO CHECK FOR BASE64 STRING
            userResetPasswordDto.code = TempData["code"].ToString();

            //TODO
            //var response =  await _accountService.ResetPasswordAsync(userResetPasswordDto);
            //if (response != null && response.HasSucceeded)
            //{
            //    SuccessMessage = "Successfully Reset Password.";
            //    return Page();
            //}
            //else
            //{
            //    ErrorMessage = "Wrong Email id " + response.Message;
            //    return Page();
            //}

            return Page();
        }
    }
}