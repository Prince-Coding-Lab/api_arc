using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.API.Core.Entities;
using System.Threading.Tasks;

namespace ShnoFeeh.API.Core.Interfaces
{
    public interface IAdsService
    {
        Task<DatabaseResponse> AddAdsAsync(Ads ads);
        Task<DatabaseResponse> UpdateAdsAsync(Ads ads);
        Task<DatabaseResponse> DeleteAdsAsync(int? campaginId, int? adId);
        Task<DatabaseResponse> GetAdsAsync(int? cityId, int? catId, int? statusId, string lang);
        Task<DatabaseResponse> GetAdByIdAsync(int adId, string lang);
    }
}
