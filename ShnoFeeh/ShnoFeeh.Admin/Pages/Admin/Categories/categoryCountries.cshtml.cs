using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.BusinessService.Common.Manager;
using ShnoFeeh.BusinessService.Interfaces;

namespace ShnoFeeh.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class categoryCountriesModel : PageModel
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
        public UserDto UserDto { get; set; }

        [BindProperty]
        public List<CountryDto> CountryDto { get; set; }

        #endregion

        public categoryCountriesModel(ISessionManager sessionManager, IAccountService accountService, IDemographicService demographicService)
        {
            _sessionManager = sessionManager;
            _accountService = accountService;
            _demographicService = demographicService;
        }

        public void OnGet()
        {
            if (_sessionManager.GetObject<UserDto>("userInfo") != null)
            {
                OnGetCountriesAsync();
            }
            else
            {
                RedirectToPage("login");
            }
        }

        public void OnGetCountriesAsync()
        {
            if (_sessionManager.GetObject<UserDto>("userInfo") != null)
            {
                var response = _demographicService.GetAllCountriesAsync(true);
                if (response.Result.HasSucceeded)
                {
                    CountryDto = response.Result.ReturnedObject.OrderBy(x => x.Country).ToList();
                    var resp = _demographicService.GetAllCountriesAsync(false);
                    if(resp.Result.HasSucceeded)
                    {
                        List<CountryDto> inactiveCountries = new List<CountryDto>();
                        inactiveCountries = resp.Result.ReturnedObject.OrderBy(x => x.Country).ToList();
                        CountryDto = CountryDto.Concat(inactiveCountries).OrderBy(x => x.Country).ToList();
                        //CountryDto.Where(x => x.CountryAr == null).ForAll(p => p.CountryAr = "لا شيء");
                    }
                    
                }
            }
            else
            {
                RedirectToPage("login");
            }
        }

        public async Task<IActionResult> OnPostActivateDeactivateCountryAsync(int countryId,string IsActive, string countryName,string countryNameAr)
        {
            if (ModelState.IsValid)
            {
                if (_sessionManager.GetObject<UserDto>("userInfo") != null)
                {
                    UpdateCountryDto countryDto = new UpdateCountryDto();
                    countryDto.CountryId = countryId;
                    countryDto.IsActive = IsActive == "true" ? false : true;
                    countryDto.CountryAr = String.IsNullOrEmpty(countryNameAr) ? countryName : countryNameAr;
                    var response = await _demographicService.UpdateCountry(countryDto);
                    if (response.HasSucceeded)
                    {
                        SuccessMessage = response.Message;
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
