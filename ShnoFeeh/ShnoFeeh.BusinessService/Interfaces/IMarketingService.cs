using ShnoFeeh.API.Core.DomainModels;
using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.BusinessService.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShnoFeeh.BusinessService.Interfaces
{
    public interface IMarketingService
    {
        Task<ResponseDto<List<AdsPricesDto>>> GetAllAdsPricesAsync();
        Task<ResponseDto<AdsPricesDto>> UpdateUserStatusAsync(UpdateAdsPricesDto model);
        Task<ResponseDto<List<CampaignDto>>> AddCampaign(AddCampaignDto addCampaignDto);
        Task<ResponseDto<List<AdsDto>>> AddAds(AddAdsDto addAdsDto);
        Task<ResponseDto<List<CampaignAdsDto>>> GetCampaigns(int? userId);
        Task<ResponseDto<OrderAdsDto>> AddOrder(AddOrderDto addOrderDto);
        Task<ResponseDto<List<AdsDto>>> UpdateAdsStatusAsync(UpdateAdsDto model);
        Task<ResponseDto<string>> DeleteAdAsync(int campaignId, int adId);
        Task<ResponseDto<string>> DeleteCampaignAsync(int campaignId);
        Task<ResponseDto<List<AdsDto>>> GetAdByIdAsync(int adId);
        Task<ResponseDto<List<CampaignDto>>> UpdateCampaignAsync(UpdateCampaignDto model);
        Task<ResponseDto<List<OrderDto>>> GetAllOrdersAsync();
        Task<ResponseDto<SendPaymentResponse>> CreatePayment(SendPaymentRequest sendPayment, int userId);
        Task<ResponseDto<ExecutePaymentResponse>> ExecutePaymentAsync(ExecutePaymentRequest payment, int userId);
        Task<ResponseDto<InitiatePaymentResponse>> InitiatePaymentAsync(InitiatePaymentRequest initPayment);
        Task<ResponseDto<PaymentStatusResponse>> GetPaymentStatusAsync(PaymentStatusRequest payment);
        Task<UploadImageResponse> UploadAdsImage(Image model);
    }
}
