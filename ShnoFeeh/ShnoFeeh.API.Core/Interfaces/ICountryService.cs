using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.API.Core.Entities;
using System.Threading.Tasks;

namespace ShnoFeeh.API.Core.Interfaces
{
    public interface ICountryService
    {
        Task<DatabaseResponse> UpdateCountryAsync(Countries country);
        Task<DatabaseResponse> GetCountriesAsync(bool isActive,string lang);
    }
}
