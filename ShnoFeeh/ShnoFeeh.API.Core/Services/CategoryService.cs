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
    /// This class is for category service.
    /// It implements all methods of ICategoryService.
    /// </summary>
    /// <remarks>
    /// This class can add, update, delete and get methods.
    /// </remarks>
    public sealed class CategoryService : ICategoryService
    {
        #region Fields
        internal IDataAccessHelper _dataHelper = null;
        private IConfiguration _configuration;
        #endregion
        #region Constructors 
        public CategoryService(IDataAccessHelper dataHelper, IConfiguration configuration)
        {
            _dataHelper = dataHelper;
            _configuration = configuration;
        }
        #endregion
        #region Public Methods
        public async Task<DatabaseResponse> AddCategoryAsync(Categories category)
        {
            try
            {
                SqlParameter[] parameters =
                {
                new SqlParameter( "@CatName",  SqlDbType.NVarChar ),
                new SqlParameter( "@CityId",  SqlDbType.Int ),
                new SqlParameter( "@Keywords",  SqlDbType.NVarChar ),
                new SqlParameter( "@CreatedBy",  SqlDbType.Int ),
                new SqlParameter( "@Logo",  SqlDbType.NVarChar ),
                new SqlParameter( "@CategoryAr",  SqlDbType.NVarChar ),
                new SqlParameter( "@Photo",  SqlDbType.NVarChar )
                };

                parameters[0].Value = category.CatName;
                parameters[1].Value = category.CityId;
                parameters[2].Value = category.Keywords;
                parameters[3].Value = category.CreatedBy;
                parameters[4].Value = category.Logo;
                parameters[5].Value = category.CategoryAr;
                parameters[6].Value = _configuration.GetSection("AdvHeaderImage").Value;

                _dataHelper.CommandWithParams(ProcedureNames.SpiCategories, parameters);

                DataTable dt = new DataTable();

                int result = await _dataHelper.RunAsync(dt);

                return new DatabaseResponse { ResponseCode = result };
            }
            finally
            {
                _dataHelper.Dispose();
            }
        }

        public async Task<DatabaseResponse> DeleteCategoryAsync(int categoryId)
        {
            try
            {
                SqlParameter[] parameters =
                {
                new SqlParameter( "@CategoryId",  SqlDbType.Int )
                };

                parameters[0].Value = categoryId;

                _dataHelper.CommandWithParams(ProcedureNames.SpdCategories, parameters);

                DataTable dt = new DataTable();

                int result = await _dataHelper.RunAsync(dt);
                return new DatabaseResponse { ResponseCode = result };
            }
            finally
            {
                _dataHelper.Dispose();
            }
        }

        public async Task<DatabaseResponse> GetCategoriesAsync(int? cityId, string lang)
        {
            try
            {
                SqlParameter[] parameters =
                {
                        new SqlParameter( "@CityId",  SqlDbType.Int ),
                        new SqlParameter( "@Lang",  SqlDbType.NVarChar )
                    };

                parameters[0].Value = cityId;
                parameters[1].Value = lang;

                _dataHelper.CommandWithParams(ProcedureNames.SpsCategories, parameters);

                DataTable dt = new DataTable();

                int result = await _dataHelper.RunAsync(dt);

                List<CategoryDto> categories = new List<CategoryDto>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    categories = (from model in dt.AsEnumerable()
                                  select new CategoryDto()
                                  {
                                      Id = model.Field<int>("Id"),
                                      CatName = model.Field<string>("CatName"),
                                      City = model.Field<string>("City"),
                                      Keywords = model.Field<string>("Keywords"),
                                      CreatedDate = model.Field<DateTime?>("CreatedDate"),
                                      ModifiedDate = model.Field<DateTime?>("ModifiedDate"),
                                      CreatedBy = model.Field<string>("CreatedBy"),
                                      ModifiedBy = model.Field<string>("ModifiedBy"),
                                      Logo = model.Field<string>("Logo"),
                                      CategoryAr = model.Field<string>("CategoryAr")
                                  }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = categories };
            }
            finally
            {
                _dataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> UpdateCategoryAsync(Categories category)
        {
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("@CategoryId",  SqlDbType.Int ),
                    new SqlParameter("@CatName",  SqlDbType.NVarChar ),
                    new SqlParameter("@CityId",  SqlDbType.Int ),
                    new SqlParameter("@Keywords",  SqlDbType.NVarChar ),
                    new SqlParameter("@ModifiedBy",  SqlDbType.Int ),
                    new SqlParameter("@Logo",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CategoryAr",  SqlDbType.NVarChar ),
                };

                parameters[0].Value = category.Id;
                parameters[1].Value = category.CatName;
                parameters[2].Value = category.CityId;
                parameters[3].Value = category.Keywords;
                parameters[4].Value = category.ModifiedBy;
                parameters[5].Value = category.Logo;
                parameters[6].Value = category.CategoryAr;

                _dataHelper.CommandWithParams(ProcedureNames.SpuCategories, parameters);

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
