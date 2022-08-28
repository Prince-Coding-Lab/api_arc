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
    public class ad_priceModel : PageModel
    {
        public readonly ISessionManager _sessionManager;
        private readonly IAccountService _accountService;
        private readonly IMarketingService _marketingService;

        #region PageProperties
        [ViewData]
        public string SuccessMessage { get; set; }
        [ViewData]
        public string ErrorMessage { get; set; }
        [BindProperty]
        public List<AdsPricesDto> AdsPricesDto { get; set; }

        #endregion

        public ad_priceModel(ISessionManager sessionManager, IAccountService accountService, IMarketingService marketingService)
        {
            _sessionManager = sessionManager;
            _accountService = accountService;
            _marketingService = marketingService;
        }
        public void OnGet()
        {
            if (_sessionManager.GetObject<UserDto>("userInfo") != null)
            {
                var response = _marketingService.GetAllAdsPricesAsync();
                if(response.Result.HasSucceeded)
                {
                    AdsPricesDto = response.Result.ReturnedObject;
                }
            }
            else
            {
                RedirectToPage("login");
            }
        }

        public async Task<IActionResult> OnPostUpdateAdsPriceAsync(int id, string amount, string dayOfWeek,string currency)
        {
            if (ModelState.IsValid)
            {
                UpdateAdsPricesDto updateAdsPricesDto = new UpdateAdsPricesDto();
                updateAdsPricesDto.AdPriceId = id;
                updateAdsPricesDto.Amount = Convert.ToDecimal(amount);
                updateAdsPricesDto.DayOfWeek = dayOfWeek;
                updateAdsPricesDto.Currency = currency;

                var response = await _marketingService.UpdateUserStatusAsync(updateAdsPricesDto);
                if (response.HasSucceeded)
                {
                    SuccessMessage = "Price updated successfully !";
                }
                else
                {
                    ErrorMessage = "Some error occured : " + response.Message;
                }
                return new JsonResult(response.Message);
            }
            else
            {
                ErrorMessage = "Cannot process your request. Please try again.";
                return Page();
            }
        }
    }
}
