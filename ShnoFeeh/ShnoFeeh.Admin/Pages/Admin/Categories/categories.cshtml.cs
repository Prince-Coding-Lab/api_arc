using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.API.Core.Entities;
using ShnoFeeh.BusinessService.Common.Constant;
using ShnoFeeh.BusinessService.Common.Manager;
using ShnoFeeh.BusinessService.Common.Methods;
using ShnoFeeh.BusinessService.Dto;
using ShnoFeeh.BusinessService.Interfaces;
using ShnoFeeh.BusinessService.Models;

namespace ShnoFeeh.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class categoriesModel : PageModel
    {
        public readonly ISessionManager _sessionManager;
        private readonly IAccountService _accountService;
        private readonly IManagementService _managementService;
        private readonly ICommonService _commonService;

        #region PageProperties
        [ViewData]
        public string SuccessMessage { get; set; }
        [ViewData]
        public string ErrorMessage { get; set; }
        [BindProperty]
        public UserDto UserDto { get; set; }
        [BindProperty]
        public string CityName { get; set; }
        [BindProperty]
        public string CityNameAr { get; set; }
        [BindProperty]
        public string CountryName { get; set; }
        [BindProperty]
        public string CountryNameAr { get; set; }

        [BindProperty]
        public List<CategoryDto> CategoryDto { get; set; }
        [BindProperty]
        public AddCategoryDto addCategoryDto { get; set; }
        [BindProperty]
        public IFormFile FileUpload { get; set; }
        [BindProperty]
        public IFormFile AdImageUpload { get; set; }
        [BindProperty]
        public List<CategoryCheck> categoryCheck { get; set; }
        [BindProperty]
        public List<AdvertismentDto> AdvertismentDto { get; set; }

        #endregion

        public categoriesModel(ISessionManager sessionManager, IAccountService accountService, IManagementService managementService, ICommonService commonService)
        {
            _sessionManager = sessionManager;
            _accountService = accountService;
            _managementService = managementService;
            _commonService = commonService;
        }
        public void OnGet(int cityId, string cityName, string cityNameAr)
        {
            if (cityId != 0 && cityName != null && cityNameAr != null)
            {
                _sessionManager.SetInt("cityId", cityId);
                _sessionManager.SetString("cityName", cityName);
                _sessionManager.SetString("cityNameAr", cityNameAr);
                CityName = _sessionManager.GetString("cityName") != null ? _sessionManager.GetString("cityName") : "";
                CityNameAr = cityNameAr;
                CountryName = _sessionManager.GetString("countryName") != null ? _sessionManager.GetString("countryName") : "";
                CountryNameAr = _sessionManager.GetString("countryNameAr") != null ? _sessionManager.GetString("countryNameAr") : "";
                GetAllCategories(cityId);
                GetAllAdvertisements(cityId);
            }
        }
        public void GetAllCategories(int cityId)
        {
            if (_sessionManager.GetObject<UserDto>("userInfo") != null)
            {
                var response = _managementService.GetAllCategoriesAsync(cityId);
                if (response.Result.HasSucceeded)
                {
                    CategoryDto = response.Result.ReturnedObject;
                    categoryCheck = new List<CategoryCheck>();
                    CategoryDto.ForEach(x =>
                    {
                        categoryCheck.Add(new CategoryCheck() { CatName = x.CatName, Id = x.Id, Logo = x.Logo, isChecked = false, CatNameAr = x.CategoryAr });
                    });
                }
                else
                {
                    ErrorMessage = "Some Error Occurred : " + response.Result.Message;
                }
            }
            else
            {
                RedirectToPage("login");
            }
        }

        public void GetAllAdvertisements(int cityId)
        {
            if (_sessionManager.GetObject<UserDto>("userInfo") != null)
            {
                var response = _managementService.GetAllAdvertisementsAsync(cityId);
                if (response.Result.HasSucceeded)
                {
                    AdvertismentDto = response.Result.ReturnedObject;
                }
                else
                {
                    ErrorMessage = "Some Error Occurred : " + response.Result.Message;
                }
            }
            else
            {
                RedirectToPage("login");
            }
        }

        public async Task OnPostAddCategoryAsync()
        {
            ModelState.Clear();
            if (TryValidateModel(addCategoryDto))
            {
                if (_sessionManager.GetObject<UserDto>("userInfo") != null)
                {
                    addCategoryDto.CityId = _sessionManager.GetInt("cityId");
                    addCategoryDto.CreatedBy = _sessionManager.GetInt(SessionItems.Id);
                    addCategoryDto.Logo = await UploadPhoto();
                    var response = await _managementService.AddCategoryAsync(addCategoryDto);
                    if (response.HasSucceeded)
                    {
                        SuccessMessage = "Category Added Successfully !";
                    }
                    else
                    {
                        ErrorMessage = "Some Error Occured : " + response.Message;
                    }
                    addCategoryDto = null;
                    OnGet(_sessionManager.GetInt("cityId") ?? 0, _sessionManager.GetString("cityName"), _sessionManager.GetString("cityNameAr"));
                }
                else
                {
                    RedirectToPage("login");
                }
            }
            else
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                ErrorMessage = "Cannot process your request. Please try again. " + message;
                OnGet(_sessionManager.GetInt("cityId") ?? 0, _sessionManager.GetString("cityName"), _sessionManager.GetString("cityNameAr"));
            }
        }
        public async Task OnPostUpdateAdvertisementAsync(int adId, string editCatName, int imgId)
        {

            if (_sessionManager.GetObject<UserDto>("userInfo") != null)
            {
                List<AdvertismentsImages> images = new List<AdvertismentsImages>();
                images.Add(new AdvertismentsImages() { AdvertismentId = adId, Id = imgId });
                await UploadAdPhoto(images);
                UpdateAdvertismentDto adDto = new UpdateAdvertismentDto()
                {
                    AdvertismentId = adId,
                    AdvertismentsImages = images,
                    CityId = _sessionManager.GetInt("cityId"),
                    ModifiedBy = _sessionManager.GetInt(SessionItems.Id),
                    CategoryId = (_managementService.GetAllCategoriesAsync(_sessionManager.GetInt("cityId"))).Result.ReturnedObject.Where(x => x.CatName == editCatName).FirstOrDefault().Id
                };
                var response = await _managementService.UpdateAdvertisementImage(adDto);
                if (response.HasSucceeded)
                {
                    SuccessMessage = "Advertisement Updated Successfully !";
                }
                else
                {
                    ErrorMessage = "Some Error Occured : " + response.Message;
                }
                OnGet(_sessionManager.GetInt("cityId") ?? 0, _sessionManager.GetString("cityName"), _sessionManager.GetString("cityNameAr"));
            }
            else
            {
                RedirectToPage("login");
            }


        }

        public async Task<string> UploadPhoto()
        {
            Image image = new Image();
            image.File = FileUpload;
            string fileUrl = String.Empty;
            if (image.File != null)
            {
                var response = await _managementService.UploadCategoryImage(image);

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

        public async Task UploadAdPhoto(List<AdvertismentsImages> images)
        {
            Image image = new Image();
            image.File = AdImageUpload;
            if (image.File != null)
            {
                var response = await _managementService.UploadCategoryImage(image);

                if (response != null && response.HasSucceed)
                {
                    images[0].ImageUrl = response.FileUrl;
                }
                else
                {
                    this.ErrorMessage = "Cannot upload profile photo " + response.Message;
                }
            }
        }

        public async Task OnPostDeleteCategoriesAsync()
        {
            List<CategoryCheck> chk = new List<CategoryCheck>();
            chk = categoryCheck.Where(x => x.isChecked == true).ToList();
            ErrorMessage = "Failed to delete category : ";
            foreach (var category in chk)
            {
                GetAllCategories(_sessionManager.GetInt("cityId") ?? 0);
                var categ = CategoryDto.Where(x => x.Id == category.Id).FirstOrDefault();
                var response = await _managementService.DeleteCategoryAsync(category.Id);
                if (response.HasSucceeded)
                {
                    if (response.Message.Contains("Active"))
                    {
                        ErrorMessage += " Category : " + categ.CatName + " : Active category cannot be deleted.";
                    }
                }
                else
                {
                    GetAllCategories(_sessionManager.GetInt("cityId") ?? 0);
                    var cat = CategoryDto.Where(x => x.Id == category.Id).FirstOrDefault();
                    GetAllAdvertisements(_sessionManager.GetInt("cityId") ?? 0);
                    var adId = AdvertismentDto.Where(x => x.Category == cat.CatName).FirstOrDefault();
                    if (adId != null)
                    {
                        var resp = await _managementService.DeleteAdvertisementAsync(adId.Id);
                        if (resp.HasSucceeded)
                        {
                            var res = await _managementService.DeleteCategoryAsync(category.Id);
                            if (res.HasSucceeded)
                            {
                                if (res.Message.Contains("Active"))
                                {
                                    ErrorMessage += " Category : " + cat.CatName + " : Active category cannot be deleted.";
                                }
                            }
                            else
                            {
                                ErrorMessage = "Some error occurred : Category: " + cat.CatName + " " + res.Message;
                            }
                        }
                        else
                        {
                            ErrorMessage = "Some error occurred : Category: " + cat.CatName + " " + resp.Message;
                        }
                    }
                }
            }
            if (ErrorMessage == "Failed to delete category : ")
            {
                SuccessMessage = "Updated Category List.";
                ErrorMessage = "";
            }
            categoryCheck.ForEach(x => x.isChecked = false);
            OnGet(_sessionManager.GetInt("cityId") ?? 0, _sessionManager.GetString("cityName"), _sessionManager.GetString("cityNameAr"));
        }

        public async Task OnPostDeleteAdAsync(int adDelId, int adDelImgId, string adDelCatName)
        {
            //var response = await _managementService.DeleteAdvertisementAsync(adDelId);
            //if (response.HasSucceeded)
            //{
            //    SuccessMessage = "Advertisement deleted successfully.";
            //}
            //else
            //{
            //    ErrorMessage = "Some error occurred : " + response.Message;
            //}
            List<AdvertismentsImages> images = new List<AdvertismentsImages>();
            images.Add(new AdvertismentsImages() { AdvertismentId = adDelId, Id = adDelImgId, ImageUrl = _commonService.GetUriWithoutBase(ApiUris.DefaultImage) });

            UpdateAdvertismentDto adDto = new UpdateAdvertismentDto()
            {
                AdvertismentId = adDelId,
                AdvertismentsImages = images,
                CityId = _sessionManager.GetInt("cityId"),
                ModifiedBy = _sessionManager.GetInt(SessionItems.Id),
                CategoryId = (_managementService.GetAllCategoriesAsync(_sessionManager.GetInt("cityId"))).Result.ReturnedObject.Where(x => x.CatName == adDelCatName).FirstOrDefault().Id
            };
            var response = await _managementService.UpdateAdvertisementImage(adDto);
            if (response.HasSucceeded)
            {
                SuccessMessage = "Advertisement Deleted Successfully !";
            }
            else
            {
                ErrorMessage = "Some Error Occured : " + response.Message;
            }
            OnGet(_sessionManager.GetInt("cityId") ?? 0, _sessionManager.GetString("cityName"), _sessionManager.GetString("cityNameAr"));
        }
    }
}
