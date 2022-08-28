using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.API.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShnoFeeh.API.Core.Interfaces
{
    public interface ICityService
    {
        Task<DatabaseResponse> AddCityAsync(City city);
        Task<DatabaseResponse> UpdateCityAsync(City city);
        Task<DatabaseResponse> GetCitiesAsync(int countryId,bool isActive, string lang);
        Task<DatabaseResponse> DeleteCityAsync(int cityId);
    }
}
