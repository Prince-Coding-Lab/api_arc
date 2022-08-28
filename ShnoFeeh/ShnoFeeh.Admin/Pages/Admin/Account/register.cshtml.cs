using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.BusinessService.Common.Manager;
using ShnoFeeh.BusinessService.Interfaces;

namespace ShnoFeeh.Admin.Pages.Account
{
    public class registerModel : PageModel
    {
        public readonly ISessionManager _sessionManager;
        private readonly IAccountService _accountService;
        private readonly IDemographicService _demographicService;

        #region PageProperties
        [ViewData]
        public string SuccessMessage { get; set; }
        [ViewData]
        public string ErrorMessage { get; set; }
        [BindProperty]
        public UserCreateDto UserCreateDto { get; set; }

        [BindProperty]
        public List<CountryDto> CountryDto { get; set; }

        [BindProperty]
        public List<CityDto> CityDto { get; set; }
        [BindProperty]
        public string ConfirmPassword { get; set; }

        #endregion

        public registerModel(ISessionManager sessionManager, IAccountService accountService, IDemographicService demographicService)
        {
            _sessionManager = sessionManager;
            _accountService = accountService;
            _demographicService = demographicService;
        }
        public async Task OnGetAsync()
        {
            await OnGetCountriesAsync();
            await OnGetCityAsync(CountryDto != null ? CountryDto.FirstOrDefault().Id : 1);
        }

        public async Task<IActionResult> OnGetCityAsync(int countryId)
        {

            var response = await _demographicService.GetAllCitiesAsync(countryId, true);
            if (response.HasSucceeded)
            {
                //CityDto.Add(new CityDto { Id = 0, CityName = "City", Country = "", IsActive = true, Logo = "" });
                CityDto = response.ReturnedObject;
            }

            return new JsonResult(CityDto);
        }
        public async Task OnGetCountriesAsync()
        {

            var response = await _demographicService.GetAllCountriesAsync(true);
            if (response.HasSucceeded)
            {
                CountryDto = response.ReturnedObject;
            }

        }

        public async Task OnPostCreateUserAsync()
        {
            if (ModelState.IsValid)
            {
                UserCreateDto.UserStatus = 4;
                UserCreateDto.RoleId = 2;

                if (UserCreateDto.Password == ConfirmPassword)
                {
                    var response = await _accountService.RegisterAsync(UserCreateDto);
                    if (response.HasSucceeded)
                    {
                        SuccessMessage = "User Created Successfully !";
                        UserCreateDto = null;
                    }
                    else
                    {
                        ErrorMessage = "Some Error Occured : " + response.Message;
                    }
                }
                else
                {
                    ErrorMessage = "Passwords do not match.";
                }
            }
            else
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                ErrorMessage = "Cannot process your request. Please try again. " + message;
            }
        }
    }
}
