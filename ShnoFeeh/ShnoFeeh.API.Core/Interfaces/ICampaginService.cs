using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.API.Core.Entities;
using System.Threading.Tasks;

namespace ShnoFeeh.API.Core.Interfaces
{
    public interface ICampaginService
    {
        Task<DatabaseResponse> AddCampaignAsync(Campaign campaign);
        Task<DatabaseResponse> UpdateCampaignAsync(Campaign campaign);
        Task<DatabaseResponse> GetCampaignsAsync(int? userId);
        Task<DatabaseResponse> GetCampaignByIdAsync(int id);
        Task<DatabaseResponse> DeleteCampaignAsync(int adId);
    }
}
