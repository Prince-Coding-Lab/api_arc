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

namespace ShnoFeeh.Admin.Pages.Ads
{
    [Authorize(Roles = "Admin,Company")]
    public class ad_detailsModel : PageModel
    {
        public readonly ISessionManager _sessionManager;
        private readonly IMarketingService _marketingService;
        private readonly IManagementService _managementService;
        private readonly IDemographicService _demographicService;
        private readonly IAccountService _accountService;

        #region PageProperties
        [ViewData]
        public string SuccessMessage { get; set; }
        [ViewData]
        public string ErrorMessage { get; set; }
        [BindProperty]
        public List<AdsDto> AdsDto { get; set; }
        [BindProperty]
        public decimal orderPrice { get; set; }

        #endregion

        public ad_detailsModel(ISessionManager sessionManager, IMarketingService marketingService, IManagementService managementService, IDemographicService demographicService, IAccountService accountService)
        {
            _sessionManager = sessionManager;
            _marketingService = marketingService;
            _managementService = managementService;
            _demographicService = demographicService;
            _accountService = accountService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            if (_sessionManager.GetObject<UserDto>("userInfo") != null)
            {
                if (_sessionManager.GetInt("adId") != null)
                {
                    var response = await _marketingService.GetAdByIdAsync(_sessionManager.GetInt("adId") ?? 0);
                    if (response.HasSucceeded)
                    {
                            AdsDto = response.ReturnedObject;
                            await OnGetPriceAsync(AdsDto[0].StartDate, AdsDto[0].EndDate);
                            return Page();
                    }
                    return Page();
                }
                else
                {
                    return Page();
                }
            }
            else
            {
                return RedirectToPage("login");
            }
        }
        public async Task OnGetPriceAsync(DateTime? startDate, DateTime? endDate)
        {
            int day = 0;
            decimal cost = 0.00m;
            var response = await _marketingService.GetAllAdsPricesAsync();
            if (response.HasSucceeded)
            {
                List<AdsPricesDto> adPrices = response.ReturnedObject;
                for (var date = startDate; date < endDate; date = date.Value.AddDays(1))
                {
                    day++;
                    foreach (var adPrice in adPrices)
                    {
                        if (date.Value.DayOfWeek.ToString() == adPrice.DayOfWeek)
                        {
                            cost += (adPrice.Amount ?? 0);
                        }
                    }
                }
            }
            orderPrice = cost;
        }

    }
    public static class IntExt
    {
        public static string Ordinal(this int number)
        {
            var work = number.ToString();
            if ((number % 100) == 11 || (number % 100) == 12 || (number % 100) == 13)
                return work + "th";
            switch (number % 10)
            {
                case 1: work += "st"; break;
                case 2: work += "nd"; break;
                case 3: work += "rd"; break;
                default: work += "th"; break;
            }
            return work;
        }
    }
}
