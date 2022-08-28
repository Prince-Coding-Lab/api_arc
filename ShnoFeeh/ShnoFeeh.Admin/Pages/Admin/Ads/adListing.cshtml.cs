using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShnoFeeh.API.Core.DomainModels;
using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.API.Core.Entities;
using ShnoFeeh.BusinessService.Common.Constant;
using ShnoFeeh.BusinessService.Common.Manager;
using ShnoFeeh.BusinessService.Dto;
using ShnoFeeh.BusinessService.Interfaces;

namespace ShnoFeeh.Admin.Pages.Ads
{
    [Authorize(Roles = "Admin,Company")]
    public class ads_listingModel : PageModel
    {
        public readonly ISessionManager _sessionManager;
        private readonly IMarketingService _marketingService;
        private readonly IManagementService _managementService;
        private readonly IDemographicService _demographicService;
        private readonly IAccountService _accountService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #region PageProperties
        [ViewData]
        public string SuccessMessage { get; set; }
        [ViewData]
        public string ErrorMessage { get; set; }
        public List<CampaignAdsDto> campaignDto { get; set; }
        [BindProperty]
        public decimal totalPrice { get; set; }
        [BindProperty]
        public int Days { get; set; }
        [BindProperty]
        public List<CategoryDto> CategoryDto { get; set; }
        [BindProperty]
        public UpdateAdsDto UpdateAdsDto { get; set; }
        [BindProperty]
        public UpdateCampaignDto UpdateCampaignDto { get; set; }
        [BindProperty]
        public List<IFormFile> AdImages { get; set; }
        [BindProperty]
        public IFormFile MainImage { get; set; }
        [BindProperty]
        public List<AdsMedia> AdsMedia { get; set; }
        public decimal orderPrice { get; set; }

        #endregion
        public ads_listingModel(IHttpContextAccessor httpContextAccessor, ISessionManager sessionManager, IMarketingService marketingService, IManagementService managementService, IDemographicService demographicService, IAccountService accountService)
        {
            _sessionManager = sessionManager;
            _marketingService = marketingService;
            _managementService = managementService;
            _demographicService = demographicService;
            _accountService = accountService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task OnGetAsync()
        {
            if (_sessionManager.GetObject<UserDto>("userInfo") != null)
            {
                await SetCampaignDto();
            }
            else
            {
                RedirectToPage("login");
            }
        }

        public IActionResult OnPostAdDetails(int adId)
        {
            _sessionManager.SetInt("adId", adId);
            return Redirect("/Admin/Ads/AdDetails");
        }

        public async Task<IActionResult> OnPostActivateDeactivateCampaignAsync(int adId, int statusId)
        {
            if (_sessionManager.GetObject<UserDto>("userInfo") != null)
            {
                    await SetCampaignDto();
                    CampaignAdsDto adsDto = campaignDto.Where(x => x.Ads.Any(p => p.Id == adId)).FirstOrDefault();
                    if (adsDto != null)
                    {
                        List<AdsMedia> media = new List<AdsMedia>();
                        foreach (var aMedia in adsDto.Ads[0].AdsMedia)
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
                            ActiveLink = adsDto.Ads[0].ActiveLink,
                            AdId = adsDto.Ads[0].Id,
                            CampaginId = adsDto.Ads[0].CampaginId,
                            Title = adsDto.Ads[0].Title,
                            Title_Ar = adsDto.Ads[0].Title_Ar,
                            CategoryId = adsDto.Ads[0].CategoryId,
                            CityId = adsDto.Ads[0].CityId,
                            EndDate = adsDto.Ads[0].EndDate,
                            Keyword = String.IsNullOrEmpty(adsDto.Ads[0].Keyword) ? "C" : adsDto.Ads[0].Keyword,
                            Keyword_Ar = String.IsNullOrEmpty(adsDto.Ads[0].Keyword_Ar) ? "" : adsDto.Ads[0].Keyword_Ar,
                            ModifiedBy = null,
                            Phone = adsDto.Ads[0].Phone,
                            StartDate = adsDto.Ads[0].StartDate,
                            StatusId = statusId == 5 && (adsDto.Ads[0].StartDate >= DateTime.Now) ? 1 : statusId,
                            URL = adsDto.Ads[0].URL,
                            AdsMedia = media,
                            Desc = adsDto.Ads[0].Desc.Length < 3 ? adsDto.Ads[0].Desc + "CCC" : adsDto.Ads[0].Desc,
                            Desc_Ar = adsDto.Ads[0].Desc_Ar,
                            ProductPrice = adsDto.Ads[0].ProductPrice,
                            Views = adsDto.Ads[0].Views
                        };
                        var response = await _marketingService.UpdateAdsStatusAsync(updateAdsDto);
                        if (response.HasSucceeded)
                        {
                            SuccessMessage = "Updated the status successfully !";
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
            await OnGetAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateAdsAsync(int uadId, int ucampaignId)
        {
            ModelState.Clear();
            if (TryValidateModel(UpdateAdsDto))
            {
                UpdateAdsDto dto = UpdateAdsDto;
                if (_sessionManager.GetObject<UserDto>("userInfo") != null)
                {
                    await SetCampaignDto();
                    CampaignAdsDto adsDto = campaignDto.Where(x => x.Ads.Any(p => p.Id == uadId)).FirstOrDefault();
                    if (adsDto != null)
                    {
                        UpdateCampaignDto cDto = new UpdateCampaignDto()
                        {
                            CampaignId = ucampaignId,
                            Description = "N/A",
                            Goal = "N/A",
                            Name = UpdateCampaignDto.Name,
                            ModifiedBy = _sessionManager.GetInt(SessionItems.Id)
                        };
                        bool campaignChange = (/*adsDto.Description == cDto.Description && adsDto.Goal == cDto.Goal && */adsDto.Name == cDto.Name);
                        if (campaignChange)
                        {
                            List<AdsMedia> media = new List<AdsMedia>();
                            foreach (var aMedia in adsDto.Ads[0].AdsMedia)
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
                            await UploadPhotos(media, uadId);
                            UpdateAdsDto updateAdsDto = new UpdateAdsDto()
                            {
                                ActiveLink = UpdateAdsDto.ActiveLink,
                                AdId = adsDto.Ads[0].Id,
                                CampaginId = adsDto.Ads[0].CampaginId,
                                Title = UpdateAdsDto.Title,
                                Title_Ar = UpdateAdsDto.Title,
                                CategoryId = UpdateAdsDto.CategoryId,
                                CityId = adsDto.Ads[0].CityId,
                                EndDate = adsDto.Ads[0].EndDate,
                                Keyword = UpdateAdsDto.Keyword,
                                Keyword_Ar = UpdateAdsDto.Keyword,
                                ModifiedBy = _sessionManager.GetInt(SessionItems.Id),
                                Phone = UpdateAdsDto.Phone,
                                StartDate = adsDto.Ads[0].StartDate,
                                StatusId = adsDto.Ads[0].StatusId == 2 ? 4 : adsDto.Ads[0].StatusId,
                                URL = UpdateAdsDto.URL,
                                AdsMedia = media,
                                Desc = UpdateAdsDto.Desc,
                                Desc_Ar = UpdateAdsDto.Desc,
                                ProductPrice = UpdateAdsDto.ProductPrice,
                                Views = adsDto.Ads[0].Views
                            };
                            var response = await _marketingService.UpdateAdsStatusAsync(updateAdsDto);
                            if (response.HasSucceeded)
                            {
                                SuccessMessage = "Ad updated successfully !";
                            }
                            else
                            {
                                ErrorMessage = "Some error occurred : " + response.Message;
                            }
                        }
                        else
                        {
                            var resp = await _marketingService.UpdateCampaignAsync(cDto);
                            if (resp.HasSucceeded)
                            {
                                List<AdsMedia> media = new List<AdsMedia>();
                                foreach (var aMedia in adsDto.Ads[0].AdsMedia)
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
                                await UploadPhotos(media, uadId);
                                UpdateAdsDto updateAdsDto = new UpdateAdsDto()
                                {
                                    ActiveLink = UpdateAdsDto.ActiveLink,
                                    AdId = adsDto.Ads[0].Id,
                                    CampaginId = adsDto.Ads[0].CampaginId,
                                    Title = UpdateAdsDto.Title,
                                    Title_Ar = UpdateAdsDto.Title,
                                    CategoryId = UpdateAdsDto.CategoryId,
                                    CityId = adsDto.Ads[0].CityId,
                                    EndDate = adsDto.Ads[0].EndDate,
                                    Keyword = UpdateAdsDto.Keyword,
                                    Keyword_Ar = UpdateAdsDto.Keyword,
                                    ModifiedBy = _sessionManager.GetInt(SessionItems.Id),
                                    Phone = UpdateAdsDto.Phone,
                                    StartDate = adsDto.Ads[0].StartDate,
                                    StatusId = adsDto.Ads[0].StatusId == 2 ? 4 : adsDto.Ads[0].StatusId,
                                    URL = UpdateAdsDto.URL,
                                    AdsMedia = media,
                                    Desc = UpdateAdsDto.Desc,
                                    Desc_Ar = UpdateAdsDto.Desc,
                                    ProductPrice = UpdateAdsDto.ProductPrice,
                                    Views = adsDto.Ads[0].Views
                                };
                                var response = await _marketingService.UpdateAdsStatusAsync(updateAdsDto);
                                if (response.HasSucceeded)
                                {
                                    SuccessMessage = "Ad updated successfully !";
                                }
                                else
                                {
                                    ErrorMessage = "Some error occurred : " + response.Message;
                                }
                            }
                            else
                            {
                                ErrorMessage = "Some error occurred : " + resp.Message;
                            }
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
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                ErrorMessage = "Cannot process your request. Please try again. " + message;
            }
            await OnGetAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostCreateOrderAsync(int adId, int paymentType)
        {
            //if (ModelState.IsValid)
            //{
            var adsResponse = await _marketingService.GetAdByIdAsync(adId);
            if (adsResponse.HasSucceeded)
            {
                List<OrderAds> orderAds = new List<OrderAds>();

                OrderAds ads = new OrderAds();

                ads.CreatedDate = adsResponse.ReturnedObject[0].CreatedDate;
                ads.ActiveLink = adsResponse.ReturnedObject[0].ActiveLink;
                ads.AdId = adsResponse.ReturnedObject[0].Id;
                ads.CampaginId = adsResponse.ReturnedObject[0].CampaginId;
                ads.CampaginName = adsResponse.ReturnedObject[0].CampaginName;
                ads.CategoryId = adsResponse.ReturnedObject[0].CategoryId;
                ads.CategoryName = adsResponse.ReturnedObject[0].CatName;
                ads.CityId = adsResponse.ReturnedObject[0].CityId;
                ads.CityName = adsResponse.ReturnedObject[0].City;
                ads.CreatedBy = _sessionManager.GetInt(SessionItems.Id);
                ads.Description = adsResponse.ReturnedObject[0].Desc;
                ads.Desc_Ar = adsResponse.ReturnedObject[0].Desc_Ar;
                ads.EndDate = adsResponse.ReturnedObject[0].EndDate.ToString();
                ads.Keyword = adsResponse.ReturnedObject[0].Keyword;
                ads.Keyword_Ar = adsResponse.ReturnedObject[0].Keyword_Ar;
                ads.ModifiedBy = null;
                ads.ModifiedDate = adsResponse.ReturnedObject[0].ModifiedDate;
                ads.Phone = adsResponse.ReturnedObject[0].Phone;
                ads.Price = adsResponse.ReturnedObject[0].ProductPrice;
                ads.StartDate = adsResponse.ReturnedObject[0].StartDate.ToString();
                ads.StatusId = adsResponse.ReturnedObject[0].StatusId;
                ads.URL = adsResponse.ReturnedObject[0].URL;
                ads.Title = adsResponse.ReturnedObject[0].Title;
                ads.Title_Ar = adsResponse.ReturnedObject[0].Title_Ar;
                orderAds.Add(ads);
                await OnGetPriceAsync(ads.StartDate, ads.EndDate);
                AddOrderDto addOrderDto = new AddOrderDto()
                {
                    CreatedBy = _sessionManager.GetInt(SessionItems.Id),
                    Currency = "KWD",
                    PhoneNumber = adsResponse.ReturnedObject[0].Phone,
                    StatusId = 3,
                    TotalPrice = orderPrice,
                    UserId = _sessionManager.GetInt(SessionItems.Id),
                    OrderAds = orderAds,
                    City = adsResponse.ReturnedObject[0].City,

                };


                _sessionManager.SetObject("order", addOrderDto);
                //Initiate payment is actually not required
                InitiatePaymentRequest iniPaymentRequest = new InitiatePaymentRequest()
                {
                    CurrencyIso = "KWD",
                    InvoiceAmount = addOrderDto.TotalPrice ?? 0.00m

                };
                var intiateResponse = await _marketingService.InitiatePaymentAsync(iniPaymentRequest);
                if (intiateResponse.HasSucceeded)
                {
                    ExecutePaymentRequest paymentrequest = new ExecutePaymentRequest();
                    paymentrequest.CallBackUrl = _httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host.Value + "/Admin/Ads/paymentStatus?IsSuccess=1";
                    //paymentrequest.CallBackUrl = "https://g.com" + "/Ads/paymentStatus?IsSuccess=1";

                    paymentrequest.CustomerAddress = (new CustomerAddress() { Address = "", AddressInstructions = "", Block = "", HouseBuildingNo = "", Street = "" });
                    paymentrequest.CustomerCivilId = "";
                    paymentrequest.CustomerEmail = _sessionManager.GetString(SessionItems.Email);
                    paymentrequest.CustomerMobile = _sessionManager.GetString(SessionItems.PhoneNumber);
                    paymentrequest.CustomerName = _sessionManager.GetString(SessionItems.UserName);
                    paymentrequest.CustomerReference = "noshipping-nosupplier";
                    paymentrequest.DisplayCurrencyIso = "KWD";
                    paymentrequest.ErrorUrl = _httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host.Value + "/Admin/Ads/paymentStatus?IsSuccess=0";
                    //paymentrequest.ErrorUrl = "https://g.com" + "/Ads/paymentStatus?IsSuccess=0";
                    paymentrequest.ExpiryDate = "";
                    paymentrequest.InvoiceItems = new List<InvoiceItems>() { new InvoiceItems() { ItemName = ads.CampaginName, Quantity = 1, UnitPrice = addOrderDto.TotalPrice ?? 0 } };
                    paymentrequest.InvoiceValue = addOrderDto.TotalPrice ?? 0.00m;
                    paymentrequest.Language = "en";
                    paymentrequest.MobileCountryCode = _sessionManager.GetString(SessionItems.CountryCode);
                    paymentrequest.PaymentMethodId = paymentType;
                    paymentrequest.SupplierCode = "";
                    paymentrequest.UserDefinedField = "Custom Field";
                    var paymentResponse = await _marketingService.ExecutePaymentAsync(paymentrequest, _sessionManager.GetInt(SessionItems.Id) ?? 0);
                    if (paymentResponse.HasSucceeded)
                    {
                        addOrderDto.PaymentId = paymentResponse.ReturnedObject.InvoiceId;
                        addOrderDto.PaymentMethod = paymentType == 1 ? "KNET" : "Visa/Master";
                        addOrderDto.StatusId = 1;
                        addOrderDto.OrderNumber = "P" + paymentResponse.ReturnedObject.InvoiceId;
                        _sessionManager.SetObject("order", addOrderDto);
                        return Redirect(paymentResponse.ReturnedObject.PaymentURL);
                    }
                    else
                    {
                        ErrorMessage = "Some error occurred : " + paymentResponse.Message;
                    }
                }
                else
                {
                    ErrorMessage = "Some error occurred : " + intiateResponse.Message;
                }



            }
            await OnGetAsync();
            return Page();
            //}
            //else
            //{
            //    var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            //    ErrorMessage = "Cannot process your request. Please try again. " + message;
            //    await OnGetAsync();
            //    return Page();
            //}
        }
        public async Task SetCampaignDto()
        {
            if (_sessionManager.GetString(SessionItems.RoleName) == "Admin")
            {
                var response = await _marketingService.GetCampaigns(null);
                if (response.HasSucceeded)
                {
                    campaignDto = response.ReturnedObject.OrderByDescending(x=>x.CreatedDate).ToList<CampaignAdsDto>() ;
                }
            }
            else
            {
                var response = await _marketingService.GetCampaigns(_sessionManager.GetInt(SessionItems.Id));
                if (response.HasSucceeded)
                {
                    campaignDto = response.ReturnedObject.OrderByDescending(x => x.CreatedDate).ToList<CampaignAdsDto>();
                }
            }
        }

        public async Task<IActionResult> OnPostDeleteCampaignAsync(int campaignId, int adId)
        {
            //if (ModelState.IsValid)
            //{
                if (_sessionManager.GetObject<UserDto>("userInfo") != null)
                {
                    var response = await _marketingService.DeleteAdAsync(campaignId, adId);
                    if (response.HasSucceeded)
                    {
                        var resp = await _marketingService.DeleteCampaignAsync(campaignId);
                        if (resp.HasSucceeded)
                        {

                            SuccessMessage = (_sessionManager.GetString(SessionItems.RoleName) == "Admin") ? "Ad deleted successfully." : "Campaign deleted successfully.";
                        }
                        else
                        {
                            ErrorMessage = "Some Error occurred : " + resp.Message;
                        }
                    }
                    else
                    {
                        ErrorMessage = "Some Error occurred : " + response.Message;
                    }
                }
                else
                {
                    RedirectToPage("login");
                }
            //}
            //else
            //{
            //    ErrorMessage = "Cannot process your request. Please try again";
            //}
            await OnGetAsync();
            return Page();
        }

        public IActionResult OnGetCategoryAsync(int cityId)
        {
            if (_sessionManager.GetObject<UserDto>("userInfo") != null)
            {
                var response = _managementService.GetAllCategoriesAsync(cityId);
                if (response.Result.HasSucceeded)
                {
                    CategoryDto = response.Result.ReturnedObject.OrderBy(x => x.CatName).ToList();
                }
            }
            else
            {
                RedirectToPage("login");
            }
            return new JsonResult(CategoryDto);
        }
        public async Task UploadPhotos(List<AdsMedia> media, int adId)
        {
            int i = 0;
            if (MainImage != null)
            {
                Image image = new Image();
                image.File = MainImage;
                string fileUrl = String.Empty;
                if (image.File != null)
                {
                    var response = await _marketingService.UploadAdsImage(image);

                    if (response != null && response.HasSucceed)
                    {
                        media.Where(x => x.IsMain == true).FirstOrDefault().ImageUrl = response.FileUrl;
                    }
                    else
                    {
                        this.ErrorMessage = "Cannot upload profile photo " + response.Message;
                    }
                }

            }
            foreach (var file in AdImages)
            {
                Image image = new Image();
                image.File = file;
                string fileUrl = String.Empty;
                if (image.File != null)
                {
                    var response = await _marketingService.UploadAdsImage(image);

                    if (response != null && response.HasSucceed)
                    {
                        media.Add(new AdsMedia()
                        {
                            ImageUrl = response.FileUrl,
                            CreatedBy = _sessionManager.GetInt(SessionItems.Id),
                            CreatedDate = DateTime.Now,
                            Id = 0,
                            AdId = adId,
                            IsMain = false
                        });
                    }
                    else
                    {
                        this.ErrorMessage = "Cannot upload profile photo " + response.Message;
                    }
                }
            }
        }
        public async Task OnGetPriceAsync(string startDate, string endDate)
        {
            int day = 0;
            decimal cost = 0.00m;
            var response = await _marketingService.GetAllAdsPricesAsync();
            if (response.HasSucceeded)
            {
                List<AdsPricesDto> adPrices = response.ReturnedObject;
                DateTime sDate = new DateTime(Convert.ToInt32(startDate.Split(' ')[0].Split('/')[2]), Convert.ToInt32(startDate.Split(' ')[0].Split('/')[0]), Convert.ToInt32(startDate.Split(' ')[0].Split('/')[1]));
                DateTime eDate = new DateTime(Convert.ToInt32(endDate.Split(' ')[0].Split('/')[2]), Convert.ToInt32(endDate.Split(' ')[0].Split('/')[0]), Convert.ToInt32(endDate.Split(' ')[0].Split('/')[1]));
                for (var date = sDate; date < eDate; date = date.AddDays(1))
                {
                    day++;
                    foreach (var adPrice in adPrices)
                    {
                        if (date.DayOfWeek.ToString() == adPrice.DayOfWeek)
                        {
                            cost += (adPrice.Amount ?? 0);
                        }
                    }
                }
            }
            orderPrice = cost;

        }
    }
}
