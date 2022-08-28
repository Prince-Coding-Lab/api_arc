using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.BusinessService.Common.Constant;
using ShnoFeeh.BusinessService.Common.Manager;
using ShnoFeeh.BusinessService.Common.Methods;
using ShnoFeeh.BusinessService.Dto;
using ShnoFeeh.BusinessService.Interfaces;

namespace ShnoFeeh.Admin.Pages
{
    [Authorize(Roles = "Admin,Company")]
    public class profileModel : PageModel
    {
        public readonly ISessionManager _sessionManager;
        private readonly IAccountService _accountService;
        private readonly ICommonService _commonService;
        private readonly IDemographicService _demographicService;

        #region PageProperties
        [ViewData]
        public bool isFromUserManagement { get; set; }
        [ViewData]
        public string SuccessMessage { get; set; }
        [ViewData]
        public string ErrorMessage { get; set; }
        [BindProperty]
        public UserDto UserDto { get; set; }
        [BindProperty]
        public UserUpdateDto UserUpdateDto { get; set; }
        [BindProperty]
        public IFormFile FileUpload { get; set; }
        [BindProperty]
        public List<CountryDto> CountryDto { get; set; }

        [BindProperty]
        public List<CityDto> CityDto { get; set; }
        #endregion

        public profileModel(ISessionManager sessionManager, IAccountService accountService, ICommonService commonService, IDemographicService demographicService)
        {
            _sessionManager = sessionManager;
            _commonService = commonService;
            _accountService = accountService;
            _demographicService = demographicService;
        }
        public void OnGetAsync(string fromPage = "")
        {
            this.isFromUserManagement = false;
            if (_sessionManager.GetObject<UserDto>("userInfo") != null)
            {
                this.UserDto = _sessionManager.GetObject<UserDto>("userInfo");
                UserUpdateDto = new UserUpdateDto();
                UserUpdateDto.FirstName = UserDto.FirstName;
                UserUpdateDto.LastName = UserDto.LastName;
                UserUpdateDto.Email = UserDto.Email;
                UserUpdateDto.PhoneNumber = UserDto.PhoneNumber;
                UserUpdateDto.CompanyName = UserDto.CompanyName;
                UserUpdateDto.CityId = UserDto.CityId;
                UserUpdateDto.CountryId = UserDto.CountryId;
                UserUpdateDto.Photo = UserDto.Photo;
                OnGetCountriesAsync();
                var country = CountryDto.Where(x => x.Id == UserDto.CountryId).FirstOrDefault();
                if (country != null && country.IsActive)
                {
                    OnGetCityAsync(UserDto.CountryId ?? 0);
                }
                else
                {
                    OnGetCityAsync(CountryDto.FirstOrDefault().Id);
                }
                if (fromPage != "" && fromPage.Contains("successfully"))
                {
                    this.SuccessMessage = "Profile Password Updated Successfully.";
                }
                else if (fromPage.Contains("userProfile"))
                {
                    this.isFromUserManagement = true;
                    this.UserDto = _sessionManager.GetObject<UserDto>("userManagement");
                }
                else if (fromPage.Contains("updateProfile"))
                {
                    this.SuccessMessage = "Profile Updated Successfully.";
                }
                else
                {
                    this.SuccessMessage = "";
                }
            }
            else
            {
                RedirectToPage("login");
            }
        }

        public IActionResult OnGetLogoutAsync()
        {
            HttpContext.Session.Remove(SessionItems.UserName);
            HttpContext.Session.Remove(SessionItems.Token);
            HttpContext.Session.Remove(SessionItems.Id);
            HttpContext.Session.Remove("userInfo");
            _sessionManager.RemoveCookie("Shnofeeh");
            return RedirectToPage("/Admin/Account/login");
        }

        public IActionResult OnGetCityAsync(int countryId)
        {
            if (_sessionManager.GetObject<UserDto>("userInfo") != null)
            {
                var response = _demographicService.GetAllCitiesAsync(countryId, true);
                if (response.Result.HasSucceeded)
                {
                    CityDto = response.Result.ReturnedObject.OrderByDescending(x => x.CityName).ToList();
                }
            }
            else
            {
                RedirectToPage("login");
            }
            return new JsonResult(CityDto);
        }
        public void OnGetCountriesAsync()
        {
            if (_sessionManager.GetObject<UserDto>("userInfo") != null)
            {
                var response = _demographicService.GetAllCountriesAsync(true);
                if (response.Result.HasSucceeded)
                {
                    CountryDto = response.Result.ReturnedObject.OrderBy(x => x.Country).ToList();
                    _sessionManager.SetObject("countries", CountryDto);
                }
            }
            else
            {
                RedirectToPage("login");
            }
        }


        public async Task<IActionResult> OnPostUpdateProfileAsync()
        {
            if (ModelState.IsValid)
            {
                CopyToUpdateProfile(UserUpdateDto);

                string fileUrl = await UploadPhoto();
                if (!String.IsNullOrEmpty(fileUrl))
                    UserUpdateDto.Photo = fileUrl;
                else
                {
                    UserUpdateDto.Photo = _sessionManager.GetString(SessionItems.Photo);
                }

                var resp = await _accountService.UpdateAsync(UserUpdateDto);
                if (resp.HasSucceeded)
                {
                    UserDto newDto = new UserDto();
                    newDto = _sessionManager.GetObject<UserDto>("userInfo");
                    newDto.FirstName = UserUpdateDto.FirstName;
                    newDto.LastName = UserUpdateDto.LastName;
                    newDto.Email = UserUpdateDto.Email;
                    newDto.CompanyName = UserUpdateDto.CompanyName;
                    newDto.CountryId = UserUpdateDto.CountryId;
                    if (newDto.CountryId != null && _sessionManager.GetObject<List<CountryDto>>("countries") != null)
                        newDto.Country = (_sessionManager.GetObject<List<CountryDto>>("countries")).Where(x => x.Id == newDto.CountryId).FirstOrDefault().Country;
                    newDto.CityId = UserUpdateDto.CityId;
                    newDto.PhoneNumber = UserUpdateDto.PhoneNumber;
                    _sessionManager.SetString(SessionItems.Email, UserUpdateDto.Email);
                    _sessionManager.SetString(SessionItems.CompanyName, UserUpdateDto.CompanyName);
                    _sessionManager.SetString(SessionItems.PhoneNumber, UserUpdateDto.PhoneNumber);
                    _sessionManager.SetInt(SessionItems.CountryId, UserUpdateDto.CountryId ?? 0);
                    _sessionManager.SetInt(SessionItems.CityId, UserUpdateDto.CityId ?? 0);
                    if (newDto.CountryId != null)
                        _sessionManager.SetString(SessionItems.Country, newDto.Country);
                    if (!String.IsNullOrEmpty(fileUrl))
                    {
                        _sessionManager.SetString(SessionItems.Photo, fileUrl);
                        newDto.Photo = UserUpdateDto.Photo;
                    }

                    _sessionManager.SetObject("userInfo", newDto);
                    return RedirectToPage("/Admin/Account/profile", new { fromPage = "updateProfile" });
                }

                return Page();
            }
            else
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                ErrorMessage = "Cannot process your request. Please try again. " + message;
                OnGetAsync();
                return Page();
            }
        }

        public async Task<string> UploadPhoto()
        {
            Image image = new Image();
            image.File = FileUpload;
            string fileUrl = String.Empty;
            if (image.File != null)
            {
                var response = await _accountService.UploadProfileImage(image);

                if (response != null && response.HasSucceed)
                {
                    fileUrl = response.FileUrl;
                }
                else
                {
                    this.ErrorMessage = "Cannot upload profile photo " + response.Message;
                }
            }
            return fileUrl;
        }

        private void CopyToUpdateProfile(UserUpdateDto userUpdateDto)
        {
            userUpdateDto.EmailConfirmed = _sessionManager.GetString(SessionItems.EmailConfirmed) == "true" ? true : false;
            userUpdateDto.CountryCode = _sessionManager.GetString(SessionItems.CountryCode);
            userUpdateDto.PhoneNumberConfirmed = _sessionManager.GetString(SessionItems.PhoneNumberConfirmed) == "true" ? true : false;
            userUpdateDto.TwoFactorEnabled = _sessionManager.GetString(SessionItems.TwoFactorEnabled) == "true" ? true : false;
            userUpdateDto.CompanyTypeId = _sessionManager.GetInt(SessionItems.CompanyTypeId);
            userUpdateDto.UserStatus = _sessionManager.GetString(SessionItems.UserStatus) == "InActive" ? 2 : 1;
            userUpdateDto.RoleId = _sessionManager.GetInt(SessionItems.RoleId) ?? 0;
            userUpdateDto.ModifiedBy = _sessionManager.GetInt(SessionItems.Id) ?? 0;
            userUpdateDto.Id = _sessionManager.GetInt(SessionItems.Id) ?? 0;
        }

    }
}
