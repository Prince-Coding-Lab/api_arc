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
    /// This class is for PaymentRef Service.
    /// It implements all methods of IPaymentRefService.
    /// </summary>
    /// <remarks>
    /// This class can add and get methods.
    /// </remarks>
    public sealed class PaymentRefService : IPaymentRefService
    {
        #region Fields
        internal IDataAccessHelper _dataHelper = null;
        #endregion

        #region Constructors 
        public PaymentRefService(IDataAccessHelper dataHelper)
        {
            _dataHelper = dataHelper;
        }
        #endregion

        #region Public Methods
        public async Task<DatabaseResponse> AddAsync(PaymentReference reference)
        {
            try
            {
                SqlParameter[] parameters =
                {
                     new SqlParameter( "@UserId",  SqlDbType.Int ),
                     new SqlParameter( "@ExternalId",  SqlDbType.NVarChar )
                };

                parameters[0].Value = reference.UserId;
                parameters[1].Value = reference.ExternalId;

                _dataHelper.CommandWithParams(ProcedureNames.SpiPaymentReference, parameters);

                int result = await _dataHelper.RunAsync();

                return new DatabaseResponse { ResponseCode = result };
            }
            finally
            {
                _dataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> GetAllAsync()
        {
            List<PaymentReferenceDto> references = null;
            DataTable dt = null;
            try
            {
                _dataHelper.CommandWithoutParams(ProcedureNames.SpsPaymentReference);

                dt = new DataTable();

                int result = await _dataHelper.RunAsync(dt);

                if (dt != null && dt.Rows.Count > 0)
                {
                    references = new List<PaymentReferenceDto>();

                    references = (from model in dt.AsEnumerable()
                                  select new PaymentReferenceDto()
                                  {
                                      Id = model.Field<int>("Id"),
                                      ExternalId = model.Field<string>("ExternalId"),
                                      UserId = model.Field<int?>("UserId"),
                                      UserName = model.Field<string>("UserName"),
                                      CreatedDate = model.Field<DateTime?>("CreatedDate"),
                                      ModifiedDate = model.Field<DateTime?>("ModifiedDate")
                                  }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = references };
            }
            finally
            {
                references = null;
                dt = null;
                _dataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> GetByIdAsync(int id)
        {
            List<PaymentReferenceDto> references = null;
            DataTable dt = null;
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter( "@Id",  SqlDbType.Int )
                };

                parameters[0].Value = id;

                _dataHelper.CommandWithParams(ProcedureNames.SpsPaymentReferenceById, parameters);

                dt = new DataTable();

                int result = await _dataHelper.RunAsync(dt);

                if (dt != null && dt.Rows.Count > 0)
                {
                    references = new List<PaymentReferenceDto>();

                    references = (from model in dt.AsEnumerable()
                                  select new PaymentReferenceDto()
                                  {
                                      Id = model.Field<int>("Id"),
                                      ExternalId = model.Field<string>("ExternalId"),
                                      UserId = model.Field<int?>("UserId"),
                                      UserName = model.Field<string>("UserName"),
                                      CreatedDate = model.Field<DateTime?>("CreatedDate"),
                                      ModifiedDate = model.Field<DateTime?>("ModifiedDate")
                                  }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = references };
            }
            finally
            {
                references = null;
                dt = null;
                _dataHelper.Dispose();
            }
        }
        #endregion
    }
}
