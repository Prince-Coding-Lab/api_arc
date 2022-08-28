using Microsoft.Extensions.Configuration;
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
    /// <summary>
    /// This class is for ads price service.
    /// It implements all methods of IAdsPricesService.
    /// </summary>
    /// <remarks>
    /// This class can add, update, delete and get methods.
    /// </remarks>
    public sealed class AdsPricesService : IAdsPricesService
    {
        #region Fields
        internal IDataAccessHelper _dataHelper = null;
        #endregion

        #region Constructors 
        public AdsPricesService(IDataAccessHelper dataHelper)
        {
            _dataHelper = dataHelper;
        }
        #endregion

        #region Public Methods
        public async Task<DatabaseResponse> GetAdsPricesAsync()
        {
            DataTable dt = null;
            List<AdsPricesDto> adsPrices = null;
            try
            {
                _dataHelper.CommandWithoutParams(ProcedureNames.SpsAdsPrices);

                dt = new DataTable();

                int result = await _dataHelper.RunAsync(dt);

                adsPrices = new List<AdsPricesDto>();

                if (dt != null && dt.Rows.Count > 0)
                {
                    adsPrices = (from model in dt.AsEnumerable()
                                 select new AdsPricesDto()
                                 {
                                     Id = model.Field<int>("Id"),
                                     DayOfWeek = model.Field<string>("DayOfWeek"),
                                     CityId = model.Field<int?>("CityId"),
                                     City = model.Field<string>("City"),
                                     Amount = model.Field<decimal?>("Amount"),
                                     Currency = model.Field<string>("Currency"),
                                     CreatedDate = model.Field<DateTime?>("CreatedDate"),
                                     ModifiedDate = model.Field<DateTime?>("ModifiedDate"),
                                     CreatedBy = model.Field<string>("CreatedBy"),
                                     ModifiedBy = model.Field<string>("ModifiedBy")
                                 }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = adsPrices };
            }
            finally
            {
                dt = null;
                adsPrices = null;
                _dataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> GetPriceByIdAsync(int adPriceId)
        {
            DataTable dt = null;
            List<AdsPricesDto> adsPrices = null;
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter( "@AdPriceId",  SqlDbType.Int )
                };

                parameters[0].Value = adPriceId;
                _dataHelper.CommandWithParams(ProcedureNames.SpsAdsPriceById, parameters);

                dt = new DataTable();

                int result = await _dataHelper.RunAsync(dt);

                adsPrices = new List<AdsPricesDto>();

                if (dt != null && dt.Rows.Count > 0)
                {
                    adsPrices = (from model in dt.AsEnumerable()
                                 select new AdsPricesDto()
                                 {
                                     Id = model.Field<int>("Id"),
                                     DayOfWeek = model.Field<string>("DayOfWeek"),
                                     CityId = model.Field<int?>("CityId"),
                                     Amount = model.Field<decimal?>("Amount"),
                                     City = model.Field<string>("City"),
                                     Currency = model.Field<string>("Currency"),
                                     CreatedDate = model.Field<DateTime?>("CreatedDate"),
                                     ModifiedDate = model.Field<DateTime?>("ModifiedDate"),
                                     CreatedBy = model.Field<string>("CreatedBy"),
                                     ModifiedBy = model.Field<string>("ModifiedBy")
                                 }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = adsPrices };
            }
            finally
            {
                dt = null;
                adsPrices = null;
                _dataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> UpdateAdsPriceAsync(AdsPrices ads)
        {
            try
            {
                SqlParameter[] parameters =
                {
                        new SqlParameter( "@Id",  SqlDbType.Int ),
                        new SqlParameter( "@DayOfWeek",  SqlDbType.NVarChar ),
                        new SqlParameter( "@Amount",  SqlDbType.Decimal ),
                        new SqlParameter( "@CityId",  SqlDbType.Int ),
                        new SqlParameter( "@Currency",  SqlDbType.NVarChar ),
                        new SqlParameter( "@ModifiedBy",  SqlDbType.Int )
                };
                parameters[0].Value = ads.Id;
                parameters[1].Value = ads.DayOfWeek;
                parameters[2].Value = ads.Amount;
                parameters[3].Value = ads.CityId;
                parameters[4].Value = ads.Currency;
                parameters[5].Value = ads.ModifiedBy;

                _dataHelper.CommandWithParams(ProcedureNames.SpuAdsPriceById, parameters);

                int result = await _dataHelper.RunAsync();
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
