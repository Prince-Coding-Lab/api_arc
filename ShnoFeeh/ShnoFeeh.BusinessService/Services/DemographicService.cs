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
    public class DemographicService : IDemographicService
    {
        private readonly ISessionManager _sessionManager;
        private readonly IShnoFeehHttpFactory _httpClientFactory;
        private readonly ICommonService _common;

        public DemographicService(IShnoFeehHttpFactory httpClientFactory, ICommonService common, ISessionManager sessionManager)
        {
            this._httpClientFactory = httpClientFactory;
            _sessionManager = sessionManager;
            _common = common;
        }

        public async Task<ResponseDto<List<CountryDto>>> GetAllCountriesAsync(bool active = false)
        {
            var response = await _httpClientFactory.GetAsyncReturnsObject<ResponseDto<List<CountryDto>>>(string.Format(_common.GetUri(ApiUris.GetAllCountries),active), "Bearer", _sessionManager.GetString(SessionItems.Token));
            return response;
        }

        public async Task<ResponseDto<List<CityDto>>> GetAllCitiesAsync(int countryId, bool active = false)
        {
            var response = await _httpClientFactory.GetAsyncReturnsObject<ResponseDto<List<CityDto>>>(string.Format(_common.GetUri(ApiUris.GetCities),countryId, active), "Bearer", _sessionManager.GetString(SessionItems.Token));
            return response;
        }

        public async Task<ResponseDto<CountryDto>> UpdateCountry(UpdateCountryDto updateCountryDto)
        {
            var response = await _httpClientFactory.PutAsyncReturnsObject<UpdateCountryDto,ResponseDto<CountryDto>>(updateCountryDto, _common.GetUri(ApiUris.UpdateCountry), _sessionManager.GetString(SessionItems.Token));
            return response;
        }

        public async Task<ResponseDto<CityDto>> UpdateCity(UpdateCityDto updateCityDto)
        {
            var response = await _httpClientFactory.PutAsyncReturnsObject<UpdateCityDto, ResponseDto<CityDto>>(updateCityDto, _common.GetUri(ApiUris.UpdateCity), _sessionManager.GetString(SessionItems.Token));
            return response;
        }
    }
}
