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
    /// This class is for Exception Service.
    /// It implements all methods of IExceptionService.
    /// </summary>
    /// <remarks>
    /// This class can add and get methods.
    public sealed class ExceptionService : IExceptionService
    {

        internal IDataAccessHelper _dataHelper = null;
        private readonly IConfiguration _configuration;
        public ExceptionService(IDataAccessHelper dataHelper, IConfiguration configuration)
        {
            _dataHelper = dataHelper;
            _configuration = configuration;
        }
        public async Task<DatabaseResponse> GetAllApiExceptions()
        {
            try
            {

                _dataHelper.CommandWithoutParams(ProcedureNames.SpsExceptionApi);

                DataTable dt = new DataTable();
                int result = await _dataHelper.RunAsync(dt);
                ;
                List<ExceptionApi> exceptions = new List<ExceptionApi>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    exceptions = (from model in dt.AsEnumerable()
                                  select new ExceptionApi()
                                  {
                                      Id = model.Field<int>("Id"),
                                      ExceptionType = model.Field<string>("ExceptionType"),
                                      ExceptionInnerException = model.Field<string>("ExceptionInnerException"),
                                      ExceptionMessage = model.Field<string>("ExceptionMessage"),
                                      ExceptionSeverity = model.Field<string>("ExceptionSeverity"),
                                      ExceptionFileName = model.Field<string>("ExceptionFileName"),
                                      ExceptionLineNumber = model.Field<string>("ExceptionLineNumber"),
                                      ExceptionColumnNumber = model.Field<string>("ExceptionColumnNumber"),
                                      ExceptionMethodName = model.Field<string>("ExceptionMethodName"),
                                      CreatedDate = model.Field<DateTime>("CreatedDate"),
                                  }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = exceptions };
            }
            finally
            {
                _dataHelper.Dispose();
            }
        }

        public async Task<DatabaseResponse> GetAllWebExceptions()
        {
            try
            {

                _dataHelper.CommandWithoutParams(ProcedureNames.SpsExceptionWeb);
                DataTable dt = new DataTable();

                int result = await _dataHelper.RunAsync(dt);

                List<ExceptionWeb> exceptions = new List<ExceptionWeb>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    exceptions = (from model in dt.AsEnumerable()
                                  select new ExceptionWeb()
                                  {
                                      ExceptionType = model.Field<string>("ExceptionType"),
                                      ExceptionInnerException = model.Field<string>("ExceptionInnerException"),
                                      ExceptionMessage = model.Field<string>("ExceptionMessage"),
                                      ExceptionSeverity = model.Field<string>("ExceptionSeverity"),
                                      ExceptionFileName = model.Field<string>("ExceptionFileName"),
                                      ExceptionLineNumber = model.Field<string>("ExceptionLineNumber"),
                                      ExceptionColumnNumber = model.Field<string>("ExceptionColumnNumber"),
                                      ExceptionMethodName = model.Field<string>("ExceptionMethodName"),
                                      CreatedDate = model.Field<DateTime>("CreatedDate"),
                                  }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = exceptions };
            }
            finally
            {
                _dataHelper.Dispose();
            }
        }

        public async Task<DatabaseResponse> CreateWebException(ExceptionWeb ex)
        {
            try
            {
                SqlParameter[] parameters =
            {
                    new SqlParameter( "@ExceptionType",  SqlDbType.NVarChar ),
                    new SqlParameter( "@ExceptionInnerException",  SqlDbType.NVarChar ),
                    new SqlParameter( "@ExceptionMessage",  SqlDbType.NVarChar ),
                    new SqlParameter( "@ExceptionSeverity",  SqlDbType.NVarChar ),
                    new SqlParameter( "@ExceptionFileName",  SqlDbType.NVarChar ),
                    new SqlParameter( "@ExceptionLineNumber",  SqlDbType.NVarChar ),
                    new SqlParameter( "@ExceptionColumnNumber",  SqlDbType.NVarChar ),
                    new SqlParameter( "@ExceptionMethodName",  SqlDbType.NVarChar )
                };

                parameters[0].Value = ex.ExceptionType;
                parameters[1].Value = ex.ExceptionInnerException;
                parameters[2].Value = ex.ExceptionMessage;
                parameters[3].Value = ex.ExceptionSeverity;
                parameters[4].Value = ex.ExceptionFileName;
                parameters[5].Value = ex.ExceptionLineNumber;
                parameters[6].Value = ex.ExceptionColumnNumber;
                parameters[7].Value = ex.ExceptionMethodName;
                _dataHelper.CommandWithParams(ProcedureNames.SpiExceptionWeb, parameters);

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
