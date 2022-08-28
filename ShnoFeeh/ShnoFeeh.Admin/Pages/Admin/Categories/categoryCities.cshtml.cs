using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.BusinessService.Common.Manager;
using ShnoFeeh.BusinessService.Interfaces;

namespace ShnoFeeh.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class categoryCitiesModel : PageModel
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
        public string CountryName { get; set; }
        [BindProperty]
        public string CountryNameAr { get; set; }

        [BindProperty]
        public List<CityDto> CityDto { get; set; }

        #endregion

        public categoryCitiesModel(ISessionManager sessionManager, IAccountService accountService, IDemographicService demographicService)
        {
            _sessionManager = sessionManager;
            _accountService = accountService;
            _demographicService = demographicService;
        }
        public void OnGet(int countryId, string countryName, string countryNameAr)
        {
            _sessionManager.SetString("countryName", countryName);
            if (!String.IsNullOrEmpty(countryNameAr))
            {
                _sessionManager.SetString("countryNameAr", countryNameAr);
            }
            else
            {
                _sessionManager.SetString("countryNameAr", " ");
            }
            CountryName = countryName;
            CountryNameAr = _sessionManager.GetString("countryNameAr");
            OnGetCityAsync(countryId);
        }
        public void OnGetCityAsync(int countryId)
        {
            _sessionManager.SetInt("countryId", countryId);
            if (_sessionManager.GetObject<UserDto>("userInfo") != null)
            {
                var response = _demographicService.GetAllCitiesAsync(countryId, true);
                if (response.Result.HasSucceeded)
                {
                    CityDto = response.Result.ReturnedObject.OrderByDescending(x => x.CityName).ToList();
                    var resp = _demographicService.GetAllCitiesAsync(countryId,false);
                    if (resp.Result.HasSucceeded)
                    {
                        List<CityDto> inactiveCities = new List<CityDto>();
                        inactiveCities = resp.Result.ReturnedObject.OrderBy(x => x.CityName).ToList();
                        CityDto = CityDto.Concat(inactiveCities).OrderBy(x => x.CityName).ToList();
                    }
                }
            }
            else
            {
                RedirectToPage("login");
            }
        }
        public async Task<IActionResult> OnPostActivateDeactivateCityAsync(int cityId, string IsActive, string cityName,string logo,string cityNameAr)
        {
            if (ModelState.IsValid)
            {
                if (_sessionManager.GetObject<UserDto>("userInfo") != null)
                {
                    UpdateCityDto cityDto = new UpdateCityDto();
                    cityDto.Id = cityId;
                    cityDto.IsActive = IsActive == "true" ? false : true;
                    cityDto.CityName = cityName;
                    cityDto.CityAr = String.IsNullOrEmpty(cityNameAr) ? cityName : cityNameAr;
                    cityDto.Logo = logo;
                    cityDto.CountryId = _sessionManager.GetInt("countryId") ?? 0;
                    var response = await _demographicService.UpdateCity(cityDto);
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
            OnGet(_sessionManager.GetInt("countryId")??0, _sessionManager.GetString("countryName"), _sessionManager.GetString("countryNameAr"));
            return Page();
        }
    }
}
