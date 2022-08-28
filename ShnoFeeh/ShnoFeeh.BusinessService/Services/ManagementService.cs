using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.API.Core.Entities;
using ShnoFeeh.API.Core.Interfaces;
using ShnoFeeh.BusinessService.Common.Constant;
using ShnoFeeh.BusinessService.Common.Manager;
using ShnoFeeh.BusinessService.Common.Methods;
using ShnoFeeh.BusinessService.Dto;
using ShnoFeeh.BusinessService.Factories;
using ShnoFeeh.BusinessService.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShnoFeeh.BusinessService.Services
{
    public class ManagementService : IManagementService
    {
        private readonly ISessionManager _sessionManager;
        private readonly IShnoFeehHttpFactory _httpClientFactory;
        private readonly ICommonService _common;

        public ManagementService(IShnoFeehHttpFactory httpClientFactory, ICommonService common, ISessionManager sessionManager)
        {
            this._httpClientFactory = httpClientFactory;
            _sessionManager = sessionManager;
            _common = common;
        }

        public async Task<ResponseDto<List<CategoryDto>>> GetAllCategoriesAsync(int? cityId)
        {
            var response = await _httpClientFactory.GetAsyncReturnsObject<ResponseDto<List<CategoryDto>>>(cityId == null ? _common.GetUri(ApiUris.GetCategories) + "?lang=EN" : _common.GetUri(ApiUris.GetCategories) + "?cityId=" + cityId + "&lang=EN", "Bearer", _sessionManager.GetString(SessionItems.Token));
            return response;
        }

        public async Task<ResponseDto<CategoryDto>> AddCategoryAsync(AddCategoryDto model)
        {
            var response = await _httpClientFactory.PostAsyncReturnsObject<AddCategoryDto, ResponseDto<CategoryDto>>(model, _common.GetUri(ApiUris.AddCategory), _sessionManager.GetString(SessionItems.Token));
            return response;
        }

        public async Task<ResponseDto<CategoryDto>> DeleteCategoryAsync(int categoryId)
        {
            var response = await _httpClientFactory.DeleteAsyncReturnsObject<ResponseDto<CategoryDto>>(string.Format(_common.GetUri(ApiUris.DeleteCategory), categoryId), _sessionManager.GetString(SessionItems.Token));
            return response;
        }

        public async Task<UploadImageResponse> UploadCategoryImage(Image model)
        {
            var apilink = _common.GetUri(ApiUris.UploadCategoryPhoto);

            var response = await _httpClientFactory.UploadFileAsyncReturnsObject<Image, UploadImageResponse>(model, apilink, _sessionManager.GetString(SessionItems.Token));
            return response;
        }
        public async Task<ResponseDto<AdvertismentDto>> UpdateAdvertisementImage(UpdateAdvertismentDto model)
        {
            var apilink = _common.GetUri(ApiUris.UpdateAdvertisement);
            var response = await _httpClientFactory.PutAsyncReturnsObject<UpdateAdvertismentDto, ResponseDto<AdvertismentDto>>(model, apilink, _sessionManager.GetString(SessionItems.Token));
            return response;
        }
        public async Task<ResponseDto<List<AdvertismentDto>>> GetAllAdvertisementsAsync(int cityId)
        {
            var response = await _httpClientFactory.GetAsyncReturnsObject<ResponseDto<List<AdvertismentDto>>>(string.Format(_common.GetUri(ApiUris.GetAdvertisements),cityId), "Bearer", _sessionManager.GetString(SessionItems.Token));
            return response;
        }

        public async Task<ResponseDto<AdvertismentDto>> DeleteAdvertisementAsync(int advertisementId)
        {
            var response = await _httpClientFactory.DeleteAsyncReturnsObject<ResponseDto<AdvertismentDto>>(string.Format(_common.GetUri(ApiUris.DeleteAdvertisement), advertisementId), _sessionManager.GetString(SessionItems.Token));
            return response;
        }
    }
}
