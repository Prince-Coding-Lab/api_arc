using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    [Authorize(Roles = "Company")]
    public class create_adModel : PageModel
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
        [BindProperty]
        public AddCampaignDto addCampaignDto { get; set; }
        [BindProperty]
        public UserDto UserDto { get; set; }
        [BindProperty]
        public AddAdsDto addAdsDto { get; set; }
        [BindProperty]
        public List<CountryDto> CountryDto { get; set; }

        [BindProperty]
        public List<CityDto> CityDto { get; set; }

        [BindProperty]
        public List<CategoryDto> CategoryDto { get; set; }
        [BindProperty]
        public List<AdsMedia> AdsMedia { get; set; }
        [BindProperty]
        public List<IFormFile> AdImages { get; set; }

        public List<CampaignAdsDto> campaignDto { get; set; }
        [BindProperty]
        public decimal totalPrice { get; set; }
        [BindProperty]
        public int Days { get; set; }
        [BindProperty]
        public int PaymentType { get; set; }
        public string[] PaymentTypes = new[] { "KNET", "Visa/Master" };

        #endregion


        public create_adModel(ISessionManager sessionManager, IMarketingService marketingService, IManagementService managementService, IDemographicService demographicService, IAccountService accountService, IHttpContextAccessor httpContextAccessor)
        {
            _sessionManager = sessionManager;
            _marketingService = marketingService;
            _managementService = managementService;
            _demographicService = demographicService;
            _accountService = accountService;
            _httpContextAccessor = httpContextAccessor;
        }
        public void OnGetAsync()
        {
            if (_sessionManager.GetObject<UserDto>("userInfo") != null)
            {
                this.UserDto = _sessionManager.GetObject<UserDto>("userInfo");
                OnGetCountriesAsync();
            }
            else
            {
                RedirectToPage("login");
            }
        }

        public IActionResult OnGetCityAsync(int countryId)
        {
            if (_sessionManager.GetObject<UserDto>("userInfo") != null)
            {
                var response = _demographicService.GetAllCitiesAsync(countryId, true);
                if (response.Result.HasSucceeded)
                {
                    CityDto = response.Result.ReturnedObject.OrderBy(x => x.CityName).ToList();
                }
            }
            else
            {
                RedirectToPage("login");
            }
            return new JsonResult(CityDto);
        }

        public IActionResult OnGetPriceAsync(string startDate, string endDate)
        {
            int day = 0;
            decimal cost = 0.00m;
            var response = _marketingService.GetAllAdsPricesAsync();
            if (response.Result.HasSucceeded)
            {
                List<AdsPricesDto> adPrices = response.Result.ReturnedObject;
                DateTime sDate = (DateTime.ParseExact(startDate, "dd-MM-yyyy", CultureInfo.InvariantCulture));
                DateTime eDate = (DateTime.ParseExact(endDate, "dd-MM-yyyy", CultureInfo.InvariantCulture));
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
            _sessionManager.SetString("Amount", cost.ToString());
            return new JsonResult(new { noOfDays = day, amount = cost });
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

        public async Task<IActionResult> OnPostAddCampaignAsync(string hdStart, string hdEnd)
        {
            if (ModelState.IsValid)
            {
                addCampaignDto.CreatedBy = addCampaignDto.UserId = _sessionManager.GetInt(SessionItems.Id);
                addCampaignDto.Goal = "N/A";
                addCampaignDto.Description = "N/A";
                var response = await _marketingService.AddCampaign(addCampaignDto);
                if (response!=null && response.HasSucceeded)
                {
                    int campaignId = response.ReturnedObject[0].Id;
                    await UploadPhotos();
                    addAdsDto.CampaginId = campaignId;
                    addAdsDto.AdsMedia = AdsMedia;
                    addAdsDto.CreatedBy = _sessionManager.GetInt(SessionItems.Id);
                    addAdsDto.StatusId = 3;
                    addAdsDto.StartDate = DateTime.ParseExact(hdStart, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    addAdsDto.EndDate = DateTime.ParseExact(hdEnd, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    addAdsDto.Desc_Ar = addAdsDto.Desc;
                    addAdsDto.Title_Ar = addAdsDto.Title;
                    addAdsDto.Keyword_Ar = addAdsDto.Keyword;
                    List<OrderAds> orderAds = new List<OrderAds>();
                    var adsResponse = await _marketingService.AddAds(addAdsDto);
                    if (adsResponse.HasSucceeded)
                    {
                        //SuccessMessage = "Ad submitted for review.";
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

                        AddOrderDto addOrderDto = new AddOrderDto()
                        {
                            CreatedBy = _sessionManager.GetInt(SessionItems.Id),
                            Currency = "KWD",
                            PhoneNumber = addAdsDto.Phone,
                            StatusId = 3,
                            TotalPrice = Convert.ToDecimal(_sessionManager.GetString("Amount")),
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
                            paymentrequest.CustomerAddress = (new CustomerAddress() { Address = "", AddressInstructions = "", Block = "", HouseBuildingNo = "", Street = "" });
                            paymentrequest.CustomerCivilId = "";
                            paymentrequest.CustomerEmail = _sessionManager.GetString(SessionItems.Email);
                            paymentrequest.CustomerMobile = _sessionManager.GetString(SessionItems.PhoneNumber);
                            paymentrequest.CustomerName = _sessionManager.GetString(SessionItems.UserName);
                            paymentrequest.CustomerReference = "noshipping-nosupplier";
                            paymentrequest.DisplayCurrencyIso = "KWD";
                            paymentrequest.ErrorUrl = _httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host.Value + "/Admin/Ads/paymentStatus?IsSuccess=0";
                            paymentrequest.ExpiryDate = "";
                            paymentrequest.InvoiceItems = new List<InvoiceItems>() { new InvoiceItems() { ItemName = ads.CampaginName, Quantity = 1, UnitPrice = addOrderDto.TotalPrice ?? 0 } };
                            paymentrequest.InvoiceValue = addOrderDto.TotalPrice ?? 0.00m;
                            paymentrequest.Language = "en";
                            paymentrequest.MobileCountryCode = _sessionManager.GetString(SessionItems.CountryCode);
                            paymentrequest.PaymentMethodId = PaymentType;
                            paymentrequest.SupplierCode = "";
                            paymentrequest.UserDefinedField = "Custom Field";
                            var paymentResponse = await _marketingService.ExecutePaymentAsync(paymentrequest, _sessionManager.GetInt(SessionItems.Id) ?? 0);
                            if (paymentResponse.HasSucceeded)
                            {
                                addOrderDto.PaymentId = paymentResponse.ReturnedObject.InvoiceId;
                                addOrderDto.PaymentMethod = PaymentType == 1 ? "KNET" : "Visa/Master";
                                addOrderDto.StatusId = 1;
                                addOrderDto.OrderNumber = "P" + paymentResponse.ReturnedObject.InvoiceId;
                                _sessionManager.SetObject("order", addOrderDto);
                                return Redirect(paymentResponse.ReturnedObject.PaymentURL);
                            }
                            else
                            {
                                ErrorMessage = "Some error occurred while creating payment : " + paymentResponse.Message + ". Ad has been created, you can retry payment from Campaign Listing Page." ;
                            }
                        }
                        else
                        {
                            ErrorMessage = "Some error occurred : " + intiateResponse.Message;
                        }
                    }
                    else
                    {
                        ErrorMessage = "Some error occurred : " + response.Message;
                    }


                }
                else if(response!=null)
                {
                    ErrorMessage = "Error while creating campaign " + response.Message;
                    
                }
                addAdsDto = null;
                addCampaignDto = null;
                ModelState.Clear();
                OnGetAsync();
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
        public async Task UploadPhotos()
        {
            int i = 0;
            foreach (var file in AdImages)
            {
                i++;
                Image image = new Image();
                image.File = file;
                string fileUrl = String.Empty;
                if (image.File != null)
                {
                    var response = await _marketingService.UploadAdsImage(image);

                    if (response != null && response.HasSucceed)
                    {
                        AdsMedia.Add(new AdsMedia()
                        {
                            ImageUrl = response.FileUrl,
                            CreatedBy = _sessionManager.GetInt(SessionItems.Id),
                            CreatedDate = DateTime.Now,
                            Id = ((new Random(1)).Next()),
                            IsMain = i == 1 ? true : false
                        });
                    }
                    else
                    {
                        this.ErrorMessage = "Cannot upload profile photo " + response.Message;
                    }
                }
            }
        }
    }
}
