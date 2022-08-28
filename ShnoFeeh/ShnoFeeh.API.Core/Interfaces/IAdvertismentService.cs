using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.API.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShnoFeeh.API.Core.Interfaces
{
    public interface IAdvertismentService
    {
        Task<DatabaseResponse> AddAdvertismentAsync(Advertisments advertisment);
        Task<DatabaseResponse> UpdateAdvertismentAsync(Advertisments advertisment);
        Task<DatabaseResponse> DeleteAdvertismentAsync(int AdvertismentId);
        Task<DatabaseResponse> GetAdvertismentsAsync(int? categoryId, int? cityId);
    }
}
