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

namespace ShnoFeeh.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class userManagementModel : PageModel
    {
        private readonly ISessionManager _sessionManager;
        private readonly IAccountService _accountService;

        #region PageProperties
        [ViewData]
        public string ErrorMessage { get; set; }

        [ViewData]
        public string SuccessMessage { get; set; }

        [BindProperty]
        public List<UserDto> UserDto { get; set; }

        #endregion
        public userManagementModel(ISessionManager sessionManager, IAccountService accountService)
        {
            _sessionManager = sessionManager;
            _accountService = accountService;
        }
        public void OnGet()
        {
            if (_sessionManager.GetObject<UserDto>("userInfo") != null)
            {
                var response = _accountService.GetAllUsersAsync(_sessionManager.GetString(SessionItems.Token));
                if (!response.Result.HasSucceeded && response.Result.ReturnedObject != null)
                {
                    UserDto = response.Result.ReturnedObject;
                }
                else
                {
                    if (response.Result.HasSucceeded)
                    {
                        UserDto = response.Result.ReturnedObject;
                    }
                    else
                    {
                        ErrorMessage = "Some Error Occured, Please try after some time. " + response.Result.Message;
                    }
                }
            }
            else
            {
                RedirectToPage("/Admin/Account/login");
            }
        }
        public async Task<IActionResult> OnPostDeleteAsync(int userId)
        {
            if (_sessionManager.GetObject<UserDto>("userInfo") != null)
            {
                var response = await _accountService.DeleteUserAsync(_sessionManager.GetString(SessionItems.Token), userId);
                if (response.HasSucceeded)
                {
                    SuccessMessage = response.Message;
                    var userResponse = _accountService.GetAllUsersAsync(_sessionManager.GetString(SessionItems.Token));
                    if (userResponse.Result.HasSucceeded || userResponse.Result.ReturnedObject != null)
                    {
                        UserDto = userResponse.Result.ReturnedObject;
                        SuccessMessage = "User Deleted Successfully.";
                    }
                    else
                    {
                        //ErrorMessage = "Some Error Occured, Please try after some time. " + userResponse.Result.Message;
                    }
                }
                else
                {
                    ErrorMessage = "Some Error Occured, Please try after some time. " + response.Message;
                }
            }
            else
            {
                RedirectToPage("/Admin/Account/login");
            }
            OnGet();
            return Page();
        }

        public IActionResult OnPostToProfileAsync(int user)
        {
            if (user != 0)
            {
                _sessionManager.Remove("userManagement");
                UserDto = _accountService.GetAllUsersAsync(_sessionManager.GetString(SessionItems.Token)).Result.ReturnedObject;
                _sessionManager.SetObject("userManagement", UserDto.Where(x => x.Id == user).FirstOrDefault());
            }
            return RedirectToPage("/Admin/Account/profile", new { fromPage = "userProfile" });
        }

        public async Task<IActionResult> OnPostActivateDeactivateUserAsync(int userId)
        {
            if (ModelState.IsValid)
            {
                if (_sessionManager.GetObject<UserDto>("userInfo") != null)
                {
                    UserDto userUpdatedObj = (await _accountService.GetAllUsersAsync(_sessionManager.GetString(SessionItems.Token))).ReturnedObject.FirstOrDefault(x => x.Id == userId);
                    userUpdatedObj.UserStatus = (userUpdatedObj.UserStatus == "InActive" || userUpdatedObj.UserStatus == "Pending Approval") ? "Active" : "InActive";
                    var response = await _accountService.UpdateUserStatusAsync(userUpdatedObj, _sessionManager.GetString(SessionItems.Token));
                    if (response.HasSucceeded)
                    {
                        SuccessMessage = response.Message;
                        var userResponse = await _accountService.GetAllUsersAsync(_sessionManager.GetString(SessionItems.Token));
                        if (userResponse.HasSucceeded || userResponse.ReturnedObject != null)
                        {
                            UserDto = userResponse.ReturnedObject;
                            SuccessMessage = "User Status Updated Successfully.";
                        }
                        else
                        {
                            //ErrorMessage = "Some Error Occured, Please try after some time. " + userResponse.Message;
                        }
                    }
                    else
                    {
                        ErrorMessage = response.Message;
                    }
                }
                else
                {
                    RedirectToPage("/Admin/Account/login");
                }
            }
            else
            {
                ErrorMessage = "Cannot process your request. Please try again";
            }
            OnGet();
            return Page();
        }
    }
}
