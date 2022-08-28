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
using System.Threading.Tasks;

namespace ShnoFeeh.API.Core.Services
{
    /// <summary>
    /// This class is for ads service.
    /// It implements all methods of IAdsService.
    /// </summary>
    /// <remarks>
    /// This class can add, update, delete and get methods.
    /// </remarks>
    public sealed class AdsService : IAdsService
    {
        #region Fields
        internal IDataAccessHelper _dataHelper = null;
        private readonly IConfiguration _configuration;
        #endregion

        #region Constructors 
        public AdsService(IDataAccessHelper dataHelper, IConfiguration configuration)
        {
            _dataHelper = dataHelper;
            _configuration = configuration;
        }
        #endregion

        #region Public Methods
        public async Task<DatabaseResponse> GetAdByIdAsync(int adId, string lang)
        {
            List<AdsDto> ads = null;
            List<AdsMediaDto> adsMedia = null;
            DataSet ds = null;
            try
            {
                SqlParameter[] parameters =
                {
                        new SqlParameter( "@AdId",  SqlDbType.Int ),
                        new SqlParameter( "@Lang",  SqlDbType.NVarChar )
                    };

                parameters[0].Value = adId;
                parameters[1].Value = lang;
                _dataHelper.CommandWithParams(ProcedureNames.SpsAdsById, parameters);

                ds = new DataSet();

                int result = await _dataHelper.RunAsync(ds);

                ads = new List<AdsDto>();
                adsMedia = new List<AdsMediaDto>();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        adsMedia = (from model in ds.Tables[1].AsEnumerable()
                                    select new AdsMediaDto()
                                    {
                                        Id = model.Field<int>("Id"),
                                        ImageUrl = model.Field<string>("ImageUrl"),
                                        AdId = model.Field<int?>("AdId"),
                                        IsMain = model.Field<bool>("IsMain"),
                                        CreatedDate = model.Field<DateTime?>("CreatedDate"),
                                        ModifiedDate = model.Field<DateTime?>("ModifiedDate"),
                                        CreatedBy = model.Field<string>("CreatedBy"),
                                        ModifiedBy = model.Field<string>("ModifiedBy")
                                    }).ToList();
                    }
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        ads = (from model in ds.Tables[0].AsEnumerable()
                               select new AdsDto()
                               {
                                   Id = model.Field<int>("Id"),
                                   CampaginId = model.Field<int?>("CampaginId"),
                                   CampaginName = model.Field<string>("CampaginName"),
                                   City = model.Field<string>("City"),
                                   CityId = model.Field<int?>("CityId"),
                                   CatName = model.Field<string>("CatName"),
                                   CategoryId = model.Field<int?>("CategoryId"),
                                   StartDate = model.Field<DateTime?>("StartDate"),
                                   Keyword = model.Field<string>("Keyword"),
                                   EndDate = model.Field<DateTime?>("EndDate"),
                                   URL = model.Field<string>("URL"),
                                   Phone = model.Field<string>("Phone"),
                                   ActiveLink = model.Field<string>("ActiveLink"),
                                   Status = model.Field<string>("Status"),
                                   StatusId = model.Field<int?>("StatusId"),
                                   Views = model.Field<int?>("Views"),
                                   Desc = model.Field<string>("Desc"),
                                   Title = model.Field<string>("Title"),
                                   ProductPrice = model.Field<decimal?>("ProductPrice"),
                                   CreatedDate = model.Field<DateTime?>("CreatedDate"),
                                   ModifiedDate = model.Field<DateTime?>("ModifiedDate"),
                                   CreatedBy = model.Field<string>("CreatedBy"),
                                   ModifiedBy = model.Field<string>("ModifiedBy"),
                                   AdsMedia = adsMedia != null && adsMedia.Count > 0 ? adsMedia.Where(r => r.AdId == model.Field<int>("Id")).ToList() : null
                               }).ToList();
                    }
                }

                return new DatabaseResponse { ResponseCode = result, Results = ads };
            }
            finally
            {
                ads = null;
                adsMedia = null;
                ds = null;
                _dataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> GetAdsAsync(int? cityId, int? catId,int? statusId,string lang)
        {

            List<AdsDto> ads = null;
            List<AdsMediaDto> adsMedia = null;
            DataSet ds = null;
            try
            {
                SqlParameter[] parameters =
                {
                        new SqlParameter( "@CityId",  SqlDbType.Int ),
                        new SqlParameter( "@CatId",  SqlDbType.Int ),
                        new SqlParameter( "@StatusId",  SqlDbType.Int ),
                        new SqlParameter( "@Lang",  SqlDbType.NVarChar )
                    };

                parameters[0].Value = cityId;
                parameters[1].Value = catId;
                parameters[2].Value = statusId;
                parameters[3].Value = lang;

                _dataHelper.CommandWithParams(ProcedureNames.SpsAds, parameters);

                ds = new DataSet();

                int result = await _dataHelper.RunAsync(ds);

                ads = new List<AdsDto>();
                adsMedia = new List<AdsMediaDto>();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        adsMedia = (from model in ds.Tables[1].AsEnumerable()
                                    select new AdsMediaDto()
                                    {
                                        Id = model.Field<int>("Id"),
                                        ImageUrl = model.Field<string>("ImageUrl"),
                                        AdId = model.Field<int?>("AdId"),
                                        IsMain = model.Field<bool>("IsMain"),
                                        CreatedDate = model.Field<DateTime?>("CreatedDate"),
                                        ModifiedDate = model.Field<DateTime?>("ModifiedDate"),
                                        CreatedBy = model.Field<string>("CreatedBy"),
                                        ModifiedBy = model.Field<string>("ModifiedBy")
                                    }).ToList();
                    }
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        ads = (from model in ds.Tables[0].AsEnumerable()
                               select new AdsDto()
                               {
                                   Id = model.Field<int>("Id"),
                                   CampaginId = model.Field<int?>("CampaginId"),
                                   CampaginName = model.Field<string>("CampaginName"),
                                   City = model.Field<string>("City"),
                                   CityId = model.Field<int?>("CityId"),
                                   Country = model.Field<string>("Country"),
                                   CatName = model.Field<string>("CatName"),
                                   CategoryId = model.Field<int?>("CategoryId"),
                                   StartDate = model.Field<DateTime?>("StartDate"),
                                   Keyword = model.Field<string>("Keyword"),
                                   EndDate = model.Field<DateTime?>("EndDate"),
                                   URL = model.Field<string>("URL"),
                                   Phone = model.Field<string>("Phone"),
                                   ActiveLink = model.Field<string>("ActiveLink"),
                                   Status = model.Field<string>("Status"),
                                   StatusId = model.Field<int?>("StatusId"),
                                   Views = model.Field<int?>("Views"),
                                   Desc = model.Field<string>("Desc"),
                                   Title = model.Field<string>("Title"),
                                   ProductPrice = model.Field<decimal?>("ProductPrice"),
                                   CreatedDate = model.Field<DateTime?>("CreatedDate"),
                                   ModifiedDate = model.Field<DateTime?>("ModifiedDate"),
                                   CreatedBy = model.Field<string>("CreatedBy"),
                                   ModifiedBy = model.Field<string>("ModifiedBy"),
                                   AdsMedia = adsMedia != null && adsMedia.Count > 0 ? adsMedia.Where(r => r.AdId == model.Field<int>("Id")).ToList() : null
                               }).ToList();
                    }
                }

                return new DatabaseResponse { ResponseCode = result, Results = ads };
            }
            finally
            {
                ads = null;
                adsMedia = null;
                ds = null;
                _dataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> AddAdsAsync(Ads ads)
        {
            CommonHelper helper = new CommonHelper();
            List<AdsDto> adsDto = null;
            List<AdsMediaDto> adsMedia = null;
            DataSet ds = null;
            try
            {
                SqlParameter[] parameters =
                {
                new SqlParameter("@CampaginId",  SqlDbType.Int ),
                new SqlParameter("@CityId",  SqlDbType.Int ),
                new SqlParameter("@CategoryId",  SqlDbType.Int ),
                new SqlParameter("@StartDate",  SqlDbType.DateTime ),
                new SqlParameter("@Keyword",  SqlDbType.NVarChar ),
                new SqlParameter("@EndDate",  SqlDbType.DateTime ),
                new SqlParameter("@URL",  SqlDbType.NVarChar ),
                new SqlParameter("@Phone",  SqlDbType.NVarChar ),
                new SqlParameter("@ActiveLink",  SqlDbType.NVarChar ),
                new SqlParameter("@StatusId",  SqlDbType.Int ),
                new SqlParameter("@Views",  SqlDbType.Int ),
                new SqlParameter("@Desc",  SqlDbType.NVarChar ),
                new SqlParameter("@CreatedBy",  SqlDbType.Int ),
                new SqlParameter("@AdsMediajson",  SqlDbType.NVarChar ),
                new SqlParameter("@ProductPrice",  SqlDbType.Decimal ),
                new SqlParameter("@Title",  SqlDbType.NVarChar ),
                new SqlParameter("@Desc_Ar",  SqlDbType.NVarChar ),
                new SqlParameter("@Title_Ar",  SqlDbType.NVarChar ),
                new SqlParameter("@Keyword_Ar",  SqlDbType.NVarChar ),
                };

                parameters[0].Value = ads.CampaginId;
                parameters[1].Value = ads.CityId;
                parameters[2].Value = ads.CategoryId;
                parameters[3].Value = ads.StartDate;
                parameters[4].Value = ads.Keyword;
                parameters[5].Value = ads.EndDate;
                parameters[6].Value = ads.URL;
                parameters[7].Value = ads.Phone;
                parameters[8].Value = ads.ActiveLink;
                parameters[9].Value = ads.StatusId;
                parameters[10].Value = ads.Views;
                parameters[11].Value = ads.Desc;
                parameters[12].Value = ads.CreatedBy;
                parameters[13].Value = ads.AdsMedia != null ? helper.GetJsonString(ads.AdsMedia) : null;
                parameters[14].Value = ads.ProductPrice;
                parameters[15].Value = ads.Title;
                parameters[16].Value = ads.Desc_Ar;
                parameters[17].Value = ads.Title_Ar;
                parameters[18].Value = ads.Keyword_Ar;

                _dataHelper.CommandWithParams(ProcedureNames.SpiAds, parameters);

                ds = new DataSet();

                int result = await _dataHelper.RunAsync(ds);

                adsDto = new List<AdsDto>();
                adsMedia = new List<AdsMediaDto>();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        adsMedia = (from model in ds.Tables[1].AsEnumerable()
                                    select new AdsMediaDto()
                                    {
                                        Id = model.Field<int>("Id"),
                                        ImageUrl = model.Field<string>("ImageUrl"),
                                        AdId = model.Field<int?>("AdId"),
                                        IsMain = model.Field<bool>("IsMain"),
                                        CreatedDate = model.Field<DateTime?>("CreatedDate"),
                                        ModifiedDate = model.Field<DateTime?>("ModifiedDate"),
                                        CreatedBy = model.Field<string>("CreatedBy"),
                                        ModifiedBy = model.Field<string>("ModifiedBy")
                                    }).ToList();
                    }
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        adsDto = (from model in ds.Tables[0].AsEnumerable()
                               select new AdsDto()
                               {
                                   Id = model.Field<int>("Id"),
                                   CampaginId = model.Field<int?>("CampaginId"),
                                   CampaginName = model.Field<string>("CampaginName"),
                                   City = model.Field<string>("City"),
                                   CityId = model.Field<int?>("CityId"),
                                   CatName = model.Field<string>("CatName"),
                                   CategoryId = model.Field<int?>("CategoryId"),
                                   StartDate = model.Field<DateTime?>("StartDate"),
                                   Keyword = model.Field<string>("Keyword"),
                                   EndDate = model.Field<DateTime?>("EndDate"),
                                   URL = model.Field<string>("URL"),
                                   Phone = model.Field<string>("Phone"),
                                   ActiveLink = model.Field<string>("ActiveLink"),
                                   Status = model.Field<string>("Status"),
                                   StatusId = model.Field<int?>("StatusId"),
                                   Views = model.Field<int?>("Views"),
                                   Desc = model.Field<string>("Desc"),
                                   Title = model.Field<string>("Title"),
                                   ProductPrice = model.Field<decimal?>("ProductPrice"),
                                   CreatedDate = model.Field<DateTime?>("CreatedDate"),
                                   ModifiedDate = model.Field<DateTime?>("ModifiedDate"),
                                   CreatedBy = model.Field<string>("CreatedBy"),
                                   ModifiedBy = model.Field<string>("ModifiedBy"),
                                   AdsMedia = adsMedia != null && adsMedia.Count > 0 ? adsMedia.Where(r => r.AdId == model.Field<int>("Id")).ToList() : null
                               }).ToList();
                    }
                }

                return new DatabaseResponse { ResponseCode = result, Results = adsDto };
            }
            finally
            {
                ads = null;
                adsMedia = null;
                ds = null;
                _dataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> UpdateAdsAsync(Ads ads)
        {
            try
            {

                // CHECK STATUS ID IF ACCEPTED AND START DATE IS >= TODAY THEN ACTIVATE IMMEDIATELLY
                if(ads.StatusId==1 && ads.StartDate.Value.Date <= DateTime.Now.Date)
                {
                    ads.StatusId = 5;
                }

                CommonHelper helper = new CommonHelper();
                SqlParameter[] parameters =
                {
                new SqlParameter("@Id",  SqlDbType.Int ),
                new SqlParameter("@CampaginId",  SqlDbType.Int ),
                new SqlParameter("@CityId",  SqlDbType.Int ),
                new SqlParameter("@CategoryId",  SqlDbType.Int ),
                new SqlParameter("@StartDate",  SqlDbType.DateTime ),
                new SqlParameter("@Keyword",  SqlDbType.NVarChar ),
                new SqlParameter("@EndDate",  SqlDbType.DateTime ),
                new SqlParameter("@URL",  SqlDbType.NVarChar ),
                new SqlParameter("@Phone",  SqlDbType.NVarChar ),
                new SqlParameter("@ActiveLink",  SqlDbType.NVarChar ),
                new SqlParameter("@StatusId",  SqlDbType.Int ),
                new SqlParameter("@Views",  SqlDbType.Int ),
                new SqlParameter("@Desc",  SqlDbType.NVarChar ),
                new SqlParameter("@ModifiedBy",  SqlDbType.Int ),
                new SqlParameter("@AdsMediajson",  SqlDbType.NVarChar ),
                new SqlParameter("@ProductPrice",  SqlDbType.Decimal ),
                new SqlParameter("@Title",  SqlDbType.NVarChar ),
                new SqlParameter("@Desc_Ar",  SqlDbType.NVarChar ),
                new SqlParameter("@Title_Ar",  SqlDbType.NVarChar ),
                new SqlParameter("@Keyword_Ar",  SqlDbType.NVarChar ),
                };

                parameters[0].Value = ads.Id;
                parameters[1].Value = ads.CampaginId;
                parameters[2].Value = ads.CityId;
                parameters[3].Value = ads.CategoryId;
                parameters[4].Value = ads.StartDate;
                parameters[5].Value = ads.Keyword;
                parameters[6].Value = ads.EndDate;
                parameters[7].Value = ads.URL;
                parameters[8].Value = ads.Phone;
                parameters[9].Value = ads.ActiveLink;
                parameters[10].Value = ads.StatusId;
                parameters[11].Value = ads.Views;
                parameters[12].Value = ads.Desc;
                parameters[13].Value = ads.ModifiedBy;
                parameters[14].Value = ads.AdsMedia != null ? helper.GetJsonString(ads.AdsMedia) : null;
                parameters[15].Value = ads.ProductPrice;
                parameters[16].Value = ads.Title;
                parameters[17].Value = ads.Desc_Ar;
                parameters[18].Value = ads.Title_Ar;
                parameters[19].Value = ads.Keyword_Ar;
                _dataHelper.CommandWithParams(ProcedureNames.SpuAds, parameters);

                int result = await _dataHelper.RunAsync();
                return new DatabaseResponse { ResponseCode = result };
            }
            finally
            {
                _dataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> DeleteAdsAsync(int? campaginId, int? adId)
        {
            try
            {
                SqlParameter[] parameters =
                  {
                        new SqlParameter( "@Id",  SqlDbType.Int ),
                        new SqlParameter( "@CampaginId",  SqlDbType.Int )
                    };

                parameters[0].Value = adId;
                parameters[1].Value = campaginId;

                _dataHelper.CommandWithParams(ProcedureNames.SpdAds, parameters);

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
