using ShnoFeeh.API.Core.DomainModels;
using ShnoFeeh.API.Core.Dto;
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
    public class MarketingService : IMarketingService
    {
        private readonly ISessionManager _sessionManager;
        private readonly IShnoFeehHttpFactory _httpClientFactory;
        private readonly ICommonService _common;

        public MarketingService(IShnoFeehHttpFactory httpClientFactory, ICommonService common, ISessionManager sessionManager)
        {
            this._httpClientFactory = httpClientFactory;
            _sessionManager = sessionManager;
            _common = common;
        }

        public async Task<ResponseDto<List<AdsPricesDto>>> GetAllAdsPricesAsync()
        {
            var response = await _httpClientFactory.GetAsyncReturnsObject<ResponseDto<List<AdsPricesDto>>>(_common.GetUri(ApiUris.GetAllAdsPrices), "Bearer", _sessionManager.GetString(SessionItems.Token));
            return response;
        }

        public async Task<ResponseDto<AdsPricesDto>> UpdateUserStatusAsync(UpdateAdsPricesDto model)
        {
            var response = await _httpClientFactory.PutAsyncReturnsObject<UpdateAdsPricesDto, ResponseDto<AdsPricesDto>>(model, _common.GetUri(ApiUris.UpdateAdPrices), _sessionManager.GetString(SessionItems.Token));
            return response;
        }

        public async Task<ResponseDto<List<CampaignDto>>> AddCampaign(AddCampaignDto addCampaignDto)
        {
            var response = await _httpClientFactory.PostAsyncReturnsObject<AddCampaignDto, ResponseDto<List<CampaignDto>>>(addCampaignDto, _common.GetUri(ApiUris.AddCampaign), _sessionManager.GetString(SessionItems.Token));
            return response;
        }

        public async Task<ResponseDto<List<AdsDto>>> AddAds(AddAdsDto addAdsDto)
        {
            var response = await _httpClientFactory.PostAsyncReturnsObject<AddAdsDto, ResponseDto<List<AdsDto>>>(addAdsDto, _common.GetUri(ApiUris.AddAds), _sessionManager.GetString(SessionItems.Token));
            return response;
        }

        public async Task<ResponseDto<List<CampaignAdsDto>>> GetCampaigns(int? userId)
        {
            var apiurl = userId == null ? (_common.GetUri(ApiUris.GetCampaigns)) : (_common.GetUri(ApiUris.GetCampaigns) + "?userid=" + userId.ToString());
            var response = await _httpClientFactory.GetAsyncReturnsObject<ResponseDto<List<CampaignAdsDto>>>(apiurl, "Bearer", _sessionManager.GetString(SessionItems.Token));
            return response;
        }

        public async Task<ResponseDto<OrderAdsDto>> AddOrder(AddOrderDto addOrderDto)
        {
            var response = await _httpClientFactory.PostAsyncReturnsObject<AddOrderDto, ResponseDto<OrderAdsDto>>(addOrderDto, _common.GetUri(ApiUris.CreateOrder), _sessionManager.GetString(SessionItems.Token));
            return response;
        }

        public async Task<ResponseDto<List<AdsDto>>> UpdateAdsStatusAsync(UpdateAdsDto model)
        {
            var response = await _httpClientFactory.PutAsyncReturnsObject<UpdateAdsDto, ResponseDto<List<AdsDto>>>(model, _common.GetUri(ApiUris.AddAds), _sessionManager.GetString(SessionItems.Token));
            return response;
        }
        public async Task<ResponseDto<List<CampaignDto>>> UpdateCampaignAsync(UpdateCampaignDto model)
        {
            var response = await _httpClientFactory.PutAsyncReturnsObject<UpdateCampaignDto, ResponseDto<List<CampaignDto>>>(model, _common.GetUri(ApiUris.AddCampaign), _sessionManager.GetString(SessionItems.Token));
            return response;
        }
        public async Task<ResponseDto<string>> DeleteAdAsync(int campaignId, int adId)
        {
            var response = await _httpClientFactory.DeleteAsyncReturnsObject<ResponseDto<string>>(String.Format(_common.GetUri(ApiUris.DeleteAd), campaignId,adId), _sessionManager.GetString(SessionItems.Token));
            return response;
        }
        public async Task<ResponseDto<string>> DeleteCampaignAsync(int campaignId)
        {
            var response = await _httpClientFactory.DeleteAsyncReturnsObject<ResponseDto<string>>(String.Format(_common.GetUri(ApiUris.DeleteCampaign), campaignId), _sessionManager.GetString(SessionItems.Token));
            return response;
        }
        public async Task<ResponseDto<List<AdsDto>>> GetAdByIdAsync(int adId)
        {
            var response = await _httpClientFactory.GetAsyncReturnsObject<ResponseDto<List<AdsDto>>>(string.Format(_common.GetUri(ApiUris.GetAddById),adId), "Bearer", _sessionManager.GetString(SessionItems.Token));
            return response;
        }
        public async Task<ResponseDto<List<OrderDto>>> GetAllOrdersAsync()
        {
            var response = await _httpClientFactory.GetAsyncReturnsObject<ResponseDto<List<OrderDto>>>(_common.GetUri(ApiUris.GetOrders), "Bearer", _sessionManager.GetString(SessionItems.Token));
            return response;
        }
        public async Task<ResponseDto<SendPaymentResponse>> CreatePayment(SendPaymentRequest sendPayment,int userId)
        {
            var response = await _httpClientFactory.PostAsyncReturnsObject<SendPaymentRequest, ResponseDto<SendPaymentResponse>>(sendPayment, string.Format(_common.GetUri(ApiUris.CreatePayment),userId), _sessionManager.GetString(SessionItems.Token));
            return response;
        }

        public async Task<ResponseDto<ExecutePaymentResponse>> ExecutePaymentAsync(ExecutePaymentRequest payment,int userId)
        {
            return await _httpClientFactory.PostAsyncReturnsObject<ExecutePaymentRequest, ResponseDto<ExecutePaymentResponse>>(payment,string.Format(_common.GetUri(ApiUris.ExecutePayment),userId), _sessionManager.GetString(SessionItems.Token));
        }

        public async Task<ResponseDto<InitiatePaymentResponse>> InitiatePaymentAsync(InitiatePaymentRequest initPayment)
        {
            return await _httpClientFactory.PostAsyncReturnsObject<InitiatePaymentRequest, ResponseDto<InitiatePaymentResponse>>(initPayment, string.Format(_common.GetUri(ApiUris.InitiatePayment)), _sessionManager.GetString(SessionItems.Token));
        }
        public async Task<ResponseDto<PaymentStatusResponse>> GetPaymentStatusAsync(PaymentStatusRequest payment)
        {
            return await _httpClientFactory.PostAsyncReturnsObject<PaymentStatusRequest, ResponseDto<PaymentStatusResponse>>(payment, string.Format(_common.GetUri(ApiUris.GetPaymentStatus)), _sessionManager.GetString(SessionItems.Token));
        }
        public async Task<UploadImageResponse> UploadAdsImage(Image model)
        {
            var apilink = _common.GetUri(ApiUris.UploadAdsPhoto);

            var response = await _httpClientFactory.UploadFileAsyncReturnsObject<Image, UploadImageResponse>(model, apilink, _sessionManager.GetString(SessionItems.Token));
            return response;
        }
    }
}
