using Microsoft.Extensions.Configuration;
using ShnoFeeh.API.Core.Common;
using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.API.Core.Entities;
using ShnoFeeh.API.Core.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ShnoFeeh.API.Core.Services
{
    /// <summary>
    /// This class is for Country service.
    /// It implements all methods of ICountryService.
    /// </summary>
    /// <remarks>
    /// This class can add, update, delete and get methods.
    /// </remarks>
    public sealed class CountryService : ICountryService
    {
        #region Fields
        internal IDataAccessHelper _dataHelper = null;
        private readonly IConfiguration _configuration;
        #endregion
        #region Constructors 
        public CountryService(IDataAccessHelper dataHelper, IConfiguration configuration)
        {
            _dataHelper = dataHelper;
            _configuration = configuration;
        }
        #endregion
        #region Public Methods
        public async Task<DatabaseResponse> GetCountriesAsync(bool isActive, string lang)
        {

            try
            {
                SqlParameter[] parameters =
                {
                        new SqlParameter( "@IsActive",  SqlDbType.Int ),
                        new SqlParameter( "@Lang",  SqlDbType.NVarChar )
                };

                parameters[0].Value = isActive;
                parameters[1].Value = lang;
                _dataHelper.CommandWithParams(ProcedureNames.SpsCountries, parameters);

                DataTable dt = new DataTable();

                int result = await _dataHelper.RunAsync(dt);

                List<CountryDto> countries = new List<CountryDto>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    countries = (from model in dt.AsEnumerable()
                                 select new CountryDto()
                                 {
                                     Id = model.Field<int>("Id"),
                                     Country = model.Field<string>("Country"),
                                     CountryAr = model.Field<string>("CountryAr"),
                                     IsActive = model.Field<bool>("IsActive"),
                                     Logo = model.Field<string>("Logo")
                                 }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = countries };
            }
            finally
            {
                _dataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> UpdateCountryAsync(Countries country)
        {
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("@CountryId",  SqlDbType.Int ),
                    new SqlParameter("@IsActive",  SqlDbType.Bit ),
                    new SqlParameter("@CountryAr",  SqlDbType.NVarChar )
                };

                parameters[0].Value = country.Id;
                parameters[1].Value = country.IsActive;
                parameters[2].Value = country.CountryAr;

                _dataHelper.CommandWithParams(ProcedureNames.SpuCountries, parameters);

                DataTable dt = new DataTable();

                int result = await _dataHelper.RunAsync(dt);
                return new DatabaseResponse { ResponseCode = result };
            }
            finally
            {
                _dataHelper.Dispose();
            }
        }
        #endregion
    }
}
