using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.BusinessService.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShnoFeeh.BusinessService.Interfaces
{
    public interface IDemographicService
    {
        Task<ResponseDto<List<CountryDto>>> GetAllCountriesAsync(bool active = false);
        Task<ResponseDto<List<CityDto>>> GetAllCitiesAsync(int countryId, bool active = false);
        Task<ResponseDto<CountryDto>> UpdateCountry(UpdateCountryDto updateCountryDto);
        Task<ResponseDto<CityDto>> UpdateCity(UpdateCityDto updateCityDto);
    }
}
