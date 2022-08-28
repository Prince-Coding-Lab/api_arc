using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShnoFeeh.BusinessService.Common.Constant;
using ShnoFeeh.BusinessService.Common.Manager;
using ShnoFeeh.BusinessService.Dto;
using ShnoFeeh.BusinessService.Interfaces;
using ShnoFeeh.API.Core.Dto;

namespace ShnoFeeh.Admin.Pages.Account
{
    public class loginModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly ISessionManager _sessionManager;

        #region PageProperties
        [ViewData]
        public string ErrorMessage { get; set; }

        [ViewData]
        public string SuccessMessage { get; set; }

        [BindProperty]
        public AuthenticateModel AuthenticateModel { get; set; }
        [BindProperty]

        public string RedirectURL { get; set; }
        [BindProperty]
        public bool RememberMe { get; set; }

        #endregion

        public loginModel(IAccountService accountService,
            ISessionManager sessionManager)
        {
            _accountService = accountService;
            _sessionManager = sessionManager;
        }
        public async Task<IActionResult> OnGet(string SuccessMessage = "")
        {
            HttpContext.Session.Remove(SessionItems.UserName);
            HttpContext.Session.Remove(SessionItems.Token);
            HttpContext.Session.Remove(SessionItems.Id);
            HttpContext.Session.Remove("userInfo");
            this.SuccessMessage = SuccessMessage;

            if (TempData["success"] != null)
                this.SuccessMessage = TempData["success"].ToString();

            if (TempData["Exist"] != null)
                this.ErrorMessage = TempData["Exist"].ToString();


            if (!String.IsNullOrEmpty(_sessionManager.GetCookie("Shnofeeh")))
            {
                AuthenticateModel = new AuthenticateModel();
                string cookie = _sessionManager.GetCookie("Shnofeeh", false);
                AuthenticateModel.Username = cookie.Split(':')[0];
                AuthenticateModel.Password = cookie.Split(':')[1];
                IActionResult result = await OnPostLoginAsync();
                return result;
            }
            return Page();
        }
        //Login Validation
        public async Task<IActionResult> OnPostLoginAsync(string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var response = await _accountService.LoginAsync(AuthenticateModel);
                if (response != null)
                {
                    if (response.Id != 0)
                    {

                        // CHECK IF EMAIL CONFIRMED
                        if (!response.EmailConfirmed)
                        {

                            _sessionManager.SetString(SessionItems.Token, response.Token);

                            var EmailResponse = await _accountService.ActivateEmail(new VerificationTokenDto()
                            {
                                Email = response.Email,
                                UserID = response.Id,
                                UserName = response.FirstName + " " + response.LastName

                            });

                            if (EmailResponse != null && EmailResponse.HasSucceeded)
                                this.SuccessMessage = "You will receive an email to verify your email address";
                            else
                                this.ErrorMessage = "Something went wrong. Please try again later " + EmailResponse.Message;

                            return Page();

                        }


                        if (response.RoleId == 1 || response.RoleId == 2)
                        {
                            _sessionManager.SetSession(response);
                            _sessionManager.SetObject("userInfo", response);

                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, response.FirstName + response.LastName),
                                new Claim(ClaimTypes.Email,response.Email),
                                new Claim(ClaimTypes.MobilePhone,response.PhoneNumber),
                                new Claim(ClaimTypes.Role,response.RoleName)
                            };
                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));


                            if (RememberMe)
                            {
                                if (String.IsNullOrEmpty(_sessionManager.GetCookie("Shnofeeh")))
                                {
                                    _sessionManager.SetCookie("Shnofeeh", AuthenticateModel.Username + ":" + AuthenticateModel.Password, 20);
                                }
                            }

                            if (!string.IsNullOrEmpty(Request.Query["returnUrl"]) && Url.IsLocalUrl(Request.Query["returnUrl"]))
                                return Redirect(returnUrl);
                            else
                            {
                                return RedirectToPage("/Admin/Dashboard");
                            }
                        }
                        else
                        {
                            ErrorMessage = "Only Admin/Company Users are allowed to login.";
                            return Page();
                        }
                    }
                    else
                    {
                        ErrorMessage = "Something went wrong. Please try again later.";
                        return Page();
                    }
                }
                else
                {
                    ErrorMessage = "Username and/or password is incorrect";
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
