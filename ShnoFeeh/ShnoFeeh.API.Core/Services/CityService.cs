using ShnoFeeh.API.Core.Common;
using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.API.Core.Entities;
using ShnoFeeh.API.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShnoFeeh.API.Core.Services
{
    public class CityService : ICityService
    {
        #region Fields
        internal IDataAccessHelper _dataHelper = null;
        #endregion
        #region Constructors 
        public CityService(IDataAccessHelper dataHelper)
        {
            _dataHelper = dataHelper;
        }
        #endregion
        public async Task<DatabaseResponse> AddCityAsync(City city)
        {
            try
            {
                SqlParameter[] parameters =
                {
                new SqlParameter( "@CityName",  SqlDbType.NVarChar ),
                new SqlParameter( "@CountryId",  SqlDbType.Int ),
                new SqlParameter( "@IsActive",  SqlDbType.Bit ),
                new SqlParameter( "@Logo",  SqlDbType.NVarChar ),
                new SqlParameter( "@CityAr",  SqlDbType.NVarChar )
                };

                parameters[0].Value = city.CityName;
                parameters[1].Value = city.CountryId;
                parameters[2].Value = city.IsActive;
                parameters[3].Value = city.Logo;
                parameters[4].Value = city.CityAr;

                _dataHelper.CommandWithParams(ProcedureNames.SpiCities, parameters);

                int result = await _dataHelper.RunAsync();

                return new DatabaseResponse { ResponseCode = result };
            }
            finally
            {
                _dataHelper.Dispose();
            }
        }

        public async Task<DatabaseResponse> DeleteCityAsync(int cityId)
        {
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter( "@Id",  SqlDbType.Int )
                };

                parameters[0].Value = cityId;
                _dataHelper.CommandWithParams(ProcedureNames.SpdCities, parameters);

                int result = await _dataHelper.RunAsync();
                return new DatabaseResponse { ResponseCode = result };
            }
            finally
            {
                _dataHelper.Dispose();
            }
        }

        public async Task<DatabaseResponse> GetCitiesAsync(int countryId, bool isActive,string lang)
        {
            DataTable dt = null;
            List<CityDto> cities = null;

            try
            {
                SqlParameter[] parameters =
                {
                        new SqlParameter( "@CountryId",  SqlDbType.Int ),
                        new SqlParameter( "@IsActive",  SqlDbType.Bit ),
                        new SqlParameter( "@Lang",  SqlDbType.NVarChar )
                };

                parameters[0].Value = countryId;
                parameters[1].Value = isActive;
                parameters[2].Value = lang;
                _dataHelper.CommandWithParams(ProcedureNames.SpsCities, parameters);

                dt = new DataTable();

                int result = await _dataHelper.RunAsync(dt);

                cities = new List<CityDto>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    cities = (from model in dt.AsEnumerable()
                                  select new CityDto()
                                  {
                                      Id = model.Field<int>("Id"),
                                      CityName = model.Field<string>("CityName"),
                                      CityAr = model.Field<string>("CityAr"),
                                      Country = model.Field<string>("Country"),
                                      IsActive = model.Field<bool>("IsActive"),
                                      Logo = model.Field<string>("Logo")
                                  }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = cities };
            }
            finally
            {
                dt = null;
                cities = null;
                _dataHelper.Dispose();
            }
        }

        public async Task<DatabaseResponse> UpdateCityAsync(City city)
        {
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter( "@CityName",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CountryId",  SqlDbType.Int ),
                    new SqlParameter( "@IsActive",  SqlDbType.Bit ),
                    new SqlParameter( "@Logo",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CityId",  SqlDbType.Int ),
                    new SqlParameter( "@CityAr",  SqlDbType.NVarChar ),
                };

                parameters[0].Value = city.CityName;
                parameters[1].Value = city.CountryId;
                parameters[2].Value = city.IsActive;
                parameters[3].Value = city.Logo;
                parameters[4].Value = city.Id;
                parameters[5].Value = city.CityAr;

                _dataHelper.CommandWithParams(ProcedureNames.SpuCities, parameters);

                int result = await _dataHelper.RunAsync();

                return new DatabaseResponse { ResponseCode = result };
            }
            finally
            {
                _dataHelper.Dispose();
            }
        }
    }
}
