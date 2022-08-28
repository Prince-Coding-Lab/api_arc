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
    /// This class is for Campagin Service.
    /// It implements all methods of ICampaginService.
    /// </summary>
    /// <remarks>
    /// This class can add, update, delete and get methods.
    public sealed class CampaginService : ICampaginService
    {
        #region Fields
        internal IDataAccessHelper _dataHelper = null;
        private readonly IConfiguration _configuration;
        #endregion
        #region Constructors 
        public CampaginService(IDataAccessHelper dataHelper, IConfiguration configuration)
        {
            _dataHelper = dataHelper;
            _configuration = configuration;
        }
        #endregion
        public async Task<DatabaseResponse> AddCampaignAsync(Campaign campaign)
        {
            try
            {
                SqlParameter[] parameters =
                {
                new SqlParameter( "@Name",  SqlDbType.NVarChar ),
                new SqlParameter( "@Description",  SqlDbType.NVarChar ),
                new SqlParameter( "@Goal",  SqlDbType.NVarChar ),
                new SqlParameter( "@CreatedBy",  SqlDbType.Int ),
                new SqlParameter( "@UserId",  SqlDbType.Int )
                };

                parameters[0].Value = campaign.Name;
                parameters[1].Value = campaign.Description;
                parameters[2].Value = campaign.Goal;
                parameters[3].Value = campaign.CreatedBy;
                parameters[4].Value = campaign.UserId;

                _dataHelper.CommandWithParams(ProcedureNames.SpiCampaign, parameters);

                DataSet ds = new DataSet();

                int result = await _dataHelper.RunAsync(ds);

                List<CampaignAdsDto> list = new List<CampaignAdsDto>();

                List<AdsDto> ads = new List<AdsDto>();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {

                        list = (from model in ds.Tables[0].AsEnumerable()
                                select new CampaignAdsDto()
                                {
                                    Id = model.Field<int>("Id"),
                                    Name = model.Field<string>("Name"),
                                    Description = model.Field<string>("Description"),
                                    Goal = model.Field<string>("Goal"),
                                    CreatedDate = model.Field<DateTime?>("CreatedDate"),
                                    ModifiedDate = model.Field<DateTime?>("ModifiedDate"),
                                    CreatedBy = model.Field<string>("CreatedBy"),
                                    ModifiedBy = model.Field<string>("ModifiedBy"),
                                    UserName = model.Field<string>("UserName"),
                                    UserId = model.Field<int?>("UserId"),
                                    Ads = ads
                                }).ToList();
                    }
                }

                return new DatabaseResponse { ResponseCode = result, Results = list };
            }
            finally
            {
                _dataHelper.Dispose();
            }
        }

        public async Task<DatabaseResponse> DeleteCampaignAsync(int adId)
        {
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter( "@Id",  SqlDbType.Int )
                };

                parameters[0].Value = adId;

                _dataHelper.CommandWithParams(ProcedureNames.SpdCampaign, parameters);

                DataTable dt = new DataTable();

                int result = await _dataHelper.RunAsync(dt);
                return new DatabaseResponse { ResponseCode = result };
            }
            finally
            {
                _dataHelper.Dispose();
            }
        }

        public async Task<DatabaseResponse> GetCampaignByIdAsync(int id)
        {
            List<AdsMediaDto> adsMedia = null;
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter( "@Id",  SqlDbType.Int )
                };
                parameters[0].Value = id;

                _dataHelper.CommandWithParams(ProcedureNames.SpsCampaignById, parameters);


                DataSet ds = new DataSet();

                int result = await _dataHelper.RunAsync(ds);

                List<CampaignAdsDto> list = new List<CampaignAdsDto>();

                List<AdsDto> ads = new List<AdsDto>();
                adsMedia = new List<AdsMediaDto>();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                    {
                        adsMedia = (from model in ds.Tables[2].AsEnumerable()
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
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        ads = (from model in ds.Tables[1].AsEnumerable()
                               select new AdsDto()
                               {
                                   Id = model.Field<int>("Id"),
                                   CampaginId = model.Field<int?>("CampaginId"),
                                   CampaginName = model.Field<string>("CampaginName"),
                                   CityId = model.Field<int?>("CityId"),
                                   City = model.Field<string>("City"),
                                   CategoryId = model.Field<int?>("CategoryId"),
                                   CatName = model.Field<string>("CatName"),
                                   StartDate = model.Field<DateTime?>("StartDate"),
                                   EndDate = model.Field<DateTime?>("EndDate"),
                                   Keyword = model.Field<string>("Keyword"),
                                   URL = model.Field<string>("URL"),
                                   Phone = model.Field<string>("Phone"),
                                   ActiveLink = model.Field<string>("ActiveLink"),
                                   StatusId = model.Field<int?>("StatusId"),
                                   Status = model.Field<string>("Status"),
                                   Views = model.Field<int?>("Views"),
                                   Desc = model.Field<string>("Desc"),
                                   ProductPrice = model.Field<decimal?>("ProductPrice"),
                                   CreatedDate = model.Field<DateTime?>("CreatedDate"),
                                   ModifiedDate = model.Field<DateTime?>("ModifiedDate"),
                                   CreatedBy = model.Field<string>("CreatedBy"),
                                   ModifiedBy = model.Field<string>("ModifiedBy"),
                                   Title = model.Field<string>("Title"),
                                   Title_Ar = model.Field<string>("Title_Ar"),
                                   Desc_Ar = model.Field<string>("Desc_Ar"),
                                   Keyword_Ar = model.Field<string>("Keyword_Ar"),
                                   AdsMedia = adsMedia != null && adsMedia.Count > 0 ? adsMedia.Where(r => r.AdId == model.Field<int>("Id")).ToList() : null
                               }).ToList();
                    }
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {

                        list = (from model in ds.Tables[0].AsEnumerable()
                                select new CampaignAdsDto()
                                {
                                    Id = model.Field<int>("Id"),
                                    Name = model.Field<string>("Name"),
                                    Description = model.Field<string>("Description"),
                                    Goal = model.Field<string>("Goal"),
                                    CreatedDate = model.Field<DateTime?>("CreatedDate"),
                                    ModifiedDate = model.Field<DateTime?>("ModifiedDate"),
                                    CreatedBy = model.Field<string>("CreatedBy"),
                                    ModifiedBy = model.Field<string>("ModifiedBy"),
                                    UserName = model.Field<string>("UserName"),
                                    UserId = model.Field<int?>("UserId"),
                                    Ads = ads
                                }).ToList();
                    }
                }

                return new DatabaseResponse { ResponseCode = result, Results = list };
            }
            finally
            {
                _dataHelper.Dispose();
            }
        }

        public async Task<DatabaseResponse> GetCampaignsAsync(int? userId)
        {
            List<AdsMediaDto> adsMedia = null;
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter( "@UserId",  SqlDbType.Int )
                };
                parameters[0].Value = userId;
              
                _dataHelper.CommandWithParams(ProcedureNames.SpsCampaign, parameters);


                DataSet ds = new DataSet();

                int result = await _dataHelper.RunAsync(ds);

                List<CampaignAdsDto> list = new List<CampaignAdsDto>();

                List<AdsDto> ads = new List<AdsDto>();
                adsMedia = new List<AdsMediaDto>();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                    {
                        adsMedia = (from model in ds.Tables[2].AsEnumerable()
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
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        ads = (from model in ds.Tables[1].AsEnumerable()
                               select new AdsDto()
                               {
                                   Id = model.Field<int>("Id"),
                                   CampaginId = model.Field<int?>("CampaginId"),
                                   CampaginName = model.Field<string>("CampaginName"),
                                   CityId = model.Field<int?>("CityId"),
                                   City = model.Field<string>("City"),
                                   Country = model.Field<string>("Country"),
                                   CategoryId = model.Field<int?>("CategoryId"),
                                   CatName = model.Field<string>("CatName"),
                                   StartDate = model.Field<DateTime?>("StartDate"),
                                   EndDate = model.Field<DateTime?>("EndDate"),
                                   Keyword = model.Field<string>("Keyword"),
                                   URL = model.Field<string>("URL"),
                                   Phone = model.Field<string>("Phone"),
                                   ActiveLink = model.Field<string>("ActiveLink"),
                                   StatusId = model.Field<int?>("StatusId"),
                                   Status = model.Field<string>("Status"),
                                   Views = model.Field<int?>("Views"),
                                   Desc = model.Field<string>("Desc"),
                                   ProductPrice = model.Field<decimal?>("ProductPrice"),
                                   CreatedDate = model.Field<DateTime?>("CreatedDate"),
                                   ModifiedDate = model.Field<DateTime?>("ModifiedDate"),
                                   CreatedBy = model.Field<string>("CreatedBy"),
                                   ModifiedBy = model.Field<string>("ModifiedBy"),
                                   Title = model.Field<string>("Title"),
                                   Title_Ar = model.Field<string>("Title_Ar"),
                                   Desc_Ar = model.Field<string>("Desc_Ar"),
                                   Keyword_Ar = model.Field<string>("Keyword_Ar"),
                                   AdsMedia = adsMedia != null && adsMedia.Count > 0 ? adsMedia.Where(r => r.AdId == model.Field<int>("Id")).ToList() : null
                               }).ToList();
                    }
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {

                        list = (from model in ds.Tables[0].AsEnumerable()
                                select new CampaignAdsDto()
                                {
                                    Id = model.Field<int>("Id"),
                                    Name = model.Field<string>("Name"),
                                    Description = model.Field<string>("Description"),
                                    Goal = model.Field<string>("Goal"),
                                    CreatedDate = model.Field<DateTime?>("CreatedDate"),
                                    ModifiedDate = model.Field<DateTime?>("ModifiedDate"),
                                    CreatedBy = model.Field<string>("CreatedBy"),
                                    ModifiedBy = model.Field<string>("ModifiedBy"),
                                    UserName = model.Field<string>("UserName"),
                                    UserId = model.Field<int?>("UserId"),
                                    Ads = ads != null && ads.Count > 0 ? ads.Where(r => r.CampaginId == model.Field<int>("Id")).ToList() : null
                                }).ToList();
                    }
                }

                return new DatabaseResponse { ResponseCode = result, Results = list };
            }
            finally
            {
                _dataHelper.Dispose();
            }
        }

        public async Task<DatabaseResponse> UpdateCampaignAsync(Campaign campaign)
        {
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter( "@Id",  SqlDbType.Int ),
                    new SqlParameter( "@Name",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Description",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Goal",  SqlDbType.NVarChar ),
                    new SqlParameter( "@ModifiedBy",  SqlDbType.Int ),
                    new SqlParameter( "@UserId",  SqlDbType.Int )
                };
                parameters[0].Value = campaign.Id;
                parameters[1].Value = campaign.Name;
                parameters[2].Value = campaign.Description;
                parameters[3].Value = campaign.Goal;
                parameters[4].Value = campaign.ModifiedBy;
                parameters[5].Value = campaign.UserId;
                _dataHelper.CommandWithParams(ProcedureNames.SpuCampaign, parameters);

                DataTable dt = new DataTable();

                int result = await _dataHelper.RunAsync(dt);
                return new DatabaseResponse { ResponseCode = result };
            }
            finally
            {
                _dataHelper.Dispose();
            }
        }
    }
}
