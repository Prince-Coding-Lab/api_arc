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
    public sealed class AdvertismentService : IAdvertismentService
    {
        #region Fields
        internal IDataAccessHelper _dataHelper = null;
        private readonly IConfiguration _configuration;
        #endregion

        #region Constructors 
        public AdvertismentService(IDataAccessHelper dataHelper, IConfiguration configuration)
        {
            _dataHelper = dataHelper;
            _configuration = configuration;
        }
        #endregion

        #region Public Methods
        public async Task<DatabaseResponse> AddAdvertismentAsync(Advertisments advertisment)
        {
            CommonHelper helper = new CommonHelper();
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("@CategoryId",  SqlDbType.Int ),
                    new SqlParameter("@CreatedBy",  SqlDbType.Int ),
                    new SqlParameter("@ImagesJson",  SqlDbType.NVarChar ),
                    new SqlParameter("@CityId",  SqlDbType.Int )
                };

                parameters[0].Value = advertisment.CategoryId;
                parameters[1].Value = advertisment.CreatedBy;
                parameters[2].Value = advertisment.AdvertismentsImages != null ? helper.GetJsonString(advertisment.AdvertismentsImages) : null;
                parameters[3].Value = advertisment.CityId;

                _dataHelper.CommandWithParams(ProcedureNames.SpiAdvertisments, parameters);

                int result = await _dataHelper.RunAsync();
                return new DatabaseResponse { ResponseCode = result };
            }
            finally
            {
                _dataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> DeleteAdvertismentAsync(int AdvertismentId)
        {
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("@Id",  SqlDbType.Int )
                };

                parameters[0].Value = AdvertismentId;

                _dataHelper.CommandWithParams(ProcedureNames.SpdAdvertisments, parameters);

                int result = await _dataHelper.RunAsync();
                return new DatabaseResponse { ResponseCode = result };
            }
            finally
            {
                _dataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> GetAdvertismentsAsync(int? categoryId, int? cityId)
        {
            List<AdvertismentDto> advertisments = null;
            List<AdvertismentImagesDto> images = null;
            DataSet ds = null;
            try
            {
                SqlParameter[] parameters =
                 {
                    new SqlParameter("@CategoryId",  SqlDbType.Int ),
                    new SqlParameter("@CityId",  SqlDbType.Int )
                };

                parameters[0].Value = categoryId;
                parameters[1].Value = cityId;
                _dataHelper.CommandWithParams(ProcedureNames.SpsAdvertisments, parameters);

                ds = new DataSet();

                int result = await _dataHelper.RunAsync(ds);

                advertisments = new List<AdvertismentDto>();
                images = new List<AdvertismentImagesDto>();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        images = (from model in ds.Tables[1].AsEnumerable()
                                    select new AdvertismentImagesDto()
                                    {
                                        Id = model.Field<int>("Id"),
                                        AdvertismentId = model.Field<int>("AdvertismentId"),
                                        ImageUrl = model.Field<string>("ImageUrl")
                                    }).ToList();
                    }
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        advertisments = (from model in ds.Tables[0].AsEnumerable()
                                  select new AdvertismentDto()
                                  {
                                      Id = model.Field<int>("Id"),
                                      Category = model.Field<string>("Category"),
                                      CategoryAr = model.Field<string>("CategoryAr"),
                                      City = model.Field<string>("City"),
                                      CreatedDate = model.Field<DateTime?>("CreatedDate"),
                                      ModifiedDate = model.Field<DateTime?>("ModifiedDate"),
                                      CreatedBy = model.Field<string>("CreatedBy"),
                                      ModifiedBy = model.Field<string>("ModifiedBy"),
                                      Images = images != null && images.Count > 0 ? images.Where(r => r.AdvertismentId == model.Field<int>("Id")).ToList() : null
                                  }).ToList();
                    }
                }

                return new DatabaseResponse { ResponseCode = result, Results = advertisments };
            }
            finally
            {
                advertisments = null;
                images = null;
                ds = null;
                _dataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> UpdateAdvertismentAsync(Advertisments advertisment)
        {
            CommonHelper helper = new CommonHelper();
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("@CategoryId",  SqlDbType.Int ),
                    new SqlParameter("@ModifiedBy",  SqlDbType.Int ),
                    new SqlParameter("@ImagesJson",  SqlDbType.NVarChar ),
                    new SqlParameter("@AdvertismentId",  SqlDbType.NVarChar ),
                    new SqlParameter("@CityId",  SqlDbType.Int )
                };

                parameters[0].Value = advertisment.CategoryId;
                parameters[1].Value = advertisment.ModifiedBy;
                parameters[2].Value = advertisment.AdvertismentsImages != null ? helper.GetJsonString(advertisment.AdvertismentsImages) : null;
                parameters[3].Value = advertisment.Id;
                parameters[4].Value = advertisment.CityId;

                _dataHelper.CommandWithParams(ProcedureNames.SpuAdvertisments, parameters);

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
