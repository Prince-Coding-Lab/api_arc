using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.API.Core.Entities;
using ShnoFeeh.BusinessService.Common.Constant;
using ShnoFeeh.BusinessService.Common.Manager;
using ShnoFeeh.BusinessService.Interfaces;

namespace ShnoFeeh.Admin.Pages.Ads
{
    [Authorize(Roles = "Company")]
    public class paymentStatusModel : PageModel
    {
        public readonly ISessionManager _sessionManager;
        private readonly IMarketingService _marketingService;
        private readonly IAccountService _accountService;

        #region PageProperties
        [ViewData]
        public string SuccessMessage { get; set; }
        [ViewData]
        public string ErrorMessage { get; set; }
        [BindProperty]
        public decimal totalAmount { get; set; }
        [BindProperty]
        public string paymentId { get; set; }

        #endregion
        public paymentStatusModel(ISessionManager sessionManager, IMarketingService marketingService, IAccountService accountService)
        {
            _sessionManager = sessionManager;
            _marketingService = marketingService;
            _accountService = accountService;
        }
        public async Task OnGetAsync(int isSuccess, string paymentId, string id)
        {
            if (isSuccess == 1)
            {
                AddOrderDto orderDto = _sessionManager.GetObject<AddOrderDto>("order");
                if (orderDto != null)
                {
                    orderDto.StatusId = 1;
                    var orderResp = await _marketingService.AddOrder(orderDto);

                    if (orderResp.HasSucceeded)
                    {
                        await OnPostUpdateAdsAsync(orderDto.OrderAds[0].AdId ?? 0);
                        totalAmount = orderDto.TotalPrice ?? 0.00m;
                        this.paymentId = paymentId;
                        SuccessMessage = "Order successfully submitted for review !";
                    }
                    else
                    {
                        ErrorMessage = "Some error occurred : " + orderResp.Message;
                    }
                }
            }
        }

        public async Task<IActionResult> OnPostUpdateAdsAsync(int uadId)
        {
            if (ModelState.IsValid)
            {
                UpdateAdsDto dto = new UpdateAdsDto();
                if (_sessionManager.GetObject<UserDto>("userInfo") != null)
                {

                    AdsDto adsDto = (await _marketingService.GetAdByIdAsync(uadId)).ReturnedObject.FirstOrDefault();
                    if (adsDto != null)
                    {

                        List<AdsMedia> media = new List<AdsMedia>();
                        foreach (var aMedia in adsDto.AdsMedia)
                        {
                            media.Add(new AdsMedia()
                            {
                                AdId = aMedia.AdId,
                                CreatedBy = _sessionManager.GetInt(SessionItems.Id),
                                CreatedDate = aMedia.CreatedDate,
                                ImageUrl = aMedia.ImageUrl,
                                IsMain = aMedia.IsMain,
                                ModifiedBy = _sessionManager.GetInt(SessionItems.Id),
                                ModifiedDate = aMedia.ModifiedDate,
                                Id = aMedia.Id
                            });
                        }
                        UpdateAdsDto updateAdsDto = new UpdateAdsDto()
                        {
                            ActiveLink = adsDto.ActiveLink,
                            AdId = adsDto.Id,
                            CampaginId = adsDto.CampaginId,
                            Title = adsDto.Title,
                            Title_Ar = adsDto.Title,
                            CategoryId = adsDto.CategoryId,
                            CityId = adsDto.CityId,
                            EndDate = adsDto.EndDate,
                            Keyword = String.IsNullOrEmpty(adsDto.Keyword) ? "Ads" : adsDto.Keyword,
                            ModifiedBy = _sessionManager.GetInt(SessionItems.Id),
                            Phone = adsDto.Phone,
                            StartDate = adsDto.StartDate,
                            StatusId = 4,
                            URL = adsDto.URL,
                            AdsMedia = media,
                            Desc = adsDto.Desc,
                            Desc_Ar = adsDto.Desc,
                            ProductPrice = adsDto.ProductPrice,
                            Views = adsDto.Views
                        };
                        var response = await _marketingService.UpdateAdsStatusAsync(updateAdsDto);
                        if (response.HasSucceeded)
                        {
                            SuccessMessage = "Ad submitted for review successfully !";
                        }
                        else
                        {
                            ErrorMessage = "Some error occurred : " + response.Message;
                        }


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
            return Page();
        }
    }
}
