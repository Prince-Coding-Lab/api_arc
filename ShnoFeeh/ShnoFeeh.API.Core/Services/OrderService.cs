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
    /// This class is for Order Service.
    /// It implements all methods of IOrderService.
    /// </summary>
    /// <remarks>
    /// This class can add, update, delete and get methods.
    public class OrderService : IOrderService
    {
        #region Fields
        internal IDataAccessHelper _dataHelper = null;
        #endregion
        #region Constructors 
        public OrderService(IDataAccessHelper dataHelper)
        {
            _dataHelper = dataHelper;
        }
        #endregion
        public async Task<DatabaseResponse> AddOrderAsync(Orders order)
        {
            CommonHelper helper = new CommonHelper();
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("@OrderItemsjson",  SqlDbType.NVarChar ),
                    new SqlParameter("@UserId",  SqlDbType.Int ),
                    new SqlParameter("@OrderNumber",  SqlDbType.NVarChar ),
                    new SqlParameter("@PaymentMethod",  SqlDbType.NVarChar ),
                    new SqlParameter("@PaymentId",  SqlDbType.Int ),
                    new SqlParameter("@TotalPrice",  SqlDbType.Decimal ),
                    new SqlParameter("@City",  SqlDbType.NVarChar ),
                    new SqlParameter("@Country",  SqlDbType.NVarChar ),
                    new SqlParameter("@PhoneNumber",  SqlDbType.NVarChar ),
                    new SqlParameter("@Currency",  SqlDbType.NVarChar ),
                    new SqlParameter("@CountryCode",  SqlDbType.NVarChar ),
                    new SqlParameter("@StatusId",  SqlDbType.Int ),
                    new SqlParameter("@AddressType",  SqlDbType.NVarChar ),
                    new SqlParameter("@AddressLine1",  SqlDbType.NVarChar ),
                    new SqlParameter("@AddressLine2",  SqlDbType.NVarChar ),
                    new SqlParameter("@AddressLine3",  SqlDbType.NVarChar ),
                    new SqlParameter("@CreatedBy",  SqlDbType.Int )
                };

                parameters[0].Value = order.OrderAds != null ? helper.GetJsonString(order.OrderAds) : null;
                parameters[1].Value = order.UserId;
                parameters[2].Value = order.OrderNumber;
                parameters[3].Value = order.PaymentMethod;
                parameters[4].Value = order.PaymentId;
                parameters[5].Value = order.TotalPrice;
                parameters[6].Value = order.City;
                parameters[7].Value = order.Country;
                parameters[8].Value = order.PhoneNumber;
                parameters[9].Value = order.Currency;
                parameters[10].Value = order.CountryCode;
                parameters[11].Value = order.StatusId;
                parameters[12].Value = order.AddressType;
                parameters[13].Value = order.AddressLine1;
                parameters[14].Value = order.AddressLine2;
                parameters[15].Value = order.AddressLine3;
                parameters[16].Value = order.CreatedBy;

                _dataHelper.CommandWithParams(ProcedureNames.SpiOrders, parameters);

                int result = await _dataHelper.RunAsync();



                //TODO SEND EMAIL TO USER AND ADMIN



                return new DatabaseResponse { ResponseCode = result };
            }
            finally
            {
                _dataHelper.Dispose();
            }
        }

        public async Task<DatabaseResponse> GetOrdersAsync()
        {
            List<OrderDto> orders = null;
            List<OrderAdsDto> orderAds = null;
            DataSet ds = null;
            try
            {
                _dataHelper.CommandWithoutParams(ProcedureNames.SpsOrders);

                ds = new DataSet();

                int result = await _dataHelper.RunAsync(ds);

                orders = new List<OrderDto>();
                orderAds = new List<OrderAdsDto>();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        orderAds = (from model in ds.Tables[1].AsEnumerable()
                                    select new OrderAdsDto()
                                    {
                                        Id = model.Field<int>("Id"),
                                        OrderId = model.Field<int>("OrderId"),
                                        AdId = model.Field<int>("AdId"),
                                        CampaginName = model.Field<string>("CampaginName"),
                                        AdDesc = model.Field<string>("AdDesc"),
                                        Status = model.Field<string>("Status"),
                                        StatusCode = model.Field<string>("StatusCode"),
                                        Price = model.Field<decimal?>("Price"),
                                        City = model.Field<string>("CityName"),
                                        CatName = model.Field<string>("CatName"),
                                        StartDate = model.Field<DateTime?>("StartDate"),
                                        EndDate = model.Field<DateTime?>("EndDate"),
                                        URL = model.Field<string>("URL"),
                                        Phone = model.Field<string>("Phone"),
                                        Title = model.Field<string>("Title"),
                                        Title_Ar = model.Field<string>("Title_Ar"),
                                        Keyword_Ar = model.Field<string>("Keyword_Ar"),
                                        Desc_Ar = model.Field<string>("Desc_Ar"),
                                        Description = model.Field<string>("Description"),
                                        ActiveLink = model.Field<string>("ActiveLink"),
                                        CreatedDate = model.Field<DateTime?>("CreatedDate"),
                                        ModifiedDate = model.Field<DateTime?>("ModifiedDate"),
                                        CreatedBy = model.Field<string>("CreatedBy"),
                                        ModifiedBy = model.Field<string>("ModifiedBy")
                                    }).ToList();
                    }
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        orders = (from model in ds.Tables[0].AsEnumerable()
                               select new OrderDto()
                               {
                                   Id = model.Field<int>("Id"),
                                   UserId = model.Field<int?>("UserId"),
                                   OrderNumber = model.Field<string>("OrderNumber"),
                                   PaymentMethod = model.Field<string>("PaymentMethod"),
                                   PaymentId = model.Field<int?>("PaymentId"),
                                   TotalPrice = model.Field<decimal?>("TotalPrice"),
                                   City = model.Field<string>("City"),
                                   CountryCode = model.Field<string>("CountryCode"),
                                   Country = model.Field<string>("Country"),
                                   PhoneNumber = model.Field<string>("PhoneNumber"),
                                   Currency = model.Field<string>("Currency"),
                                   AddressLine1 = model.Field<string>("AddressLine1"),
                                   AddressLine2 = model.Field<string>("AddressLine2"),
                                   AddressLine3 = model.Field<string>("AddressLine3"),
                                   Status = model.Field<string>("Status"),
                                   StatusCode = model.Field<string>("StatusCode"),
                                   CreatedDate = model.Field<DateTime?>("CreatedDate"),
                                   ModifiedDate = model.Field<DateTime?>("ModifiedDate"),
                                   CreatedBy = model.Field<string>("CreatedBy"),
                                   ModifiedBy = model.Field<string>("ModifiedBy"),
                                   OrderAds = orderAds != null && orderAds.Count > 0 ? orderAds.Where(r => r.OrderId == model.Field<int>("Id")).ToList() : null
                               }).ToList();
                    }
                }

                return new DatabaseResponse { ResponseCode = result, Results = orders };
            }
            finally
            {
                orders = null;
                orderAds = null;
                ds = null;
                _dataHelper.Dispose();
            }
        }

        public async Task<DatabaseResponse> UpdateOrderAsync(Orders order)
        {
            CommonHelper helper = new CommonHelper();
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("@OrderItemsjson",  SqlDbType.NVarChar ),
                    new SqlParameter("@UserId",  SqlDbType.Int ),
                    new SqlParameter("@OrderNumber",  SqlDbType.NVarChar ),
                    new SqlParameter("@PaymentMethod",  SqlDbType.NVarChar ),
                    new SqlParameter("@PaymentId",  SqlDbType.Int ),
                    new SqlParameter("@TotalPrice",  SqlDbType.Decimal ),
                    new SqlParameter("@City",  SqlDbType.NVarChar ),
                    new SqlParameter("@Country",  SqlDbType.NVarChar ),
                    new SqlParameter("@PhoneNumber",  SqlDbType.NVarChar ),
                    new SqlParameter("@Currency",  SqlDbType.NVarChar ),
                    new SqlParameter("@CountryCode",  SqlDbType.NVarChar ),
                    new SqlParameter("@StatusId",  SqlDbType.Int ),
                    new SqlParameter("@AddressType",  SqlDbType.NVarChar ),
                    new SqlParameter("@AddressLine1",  SqlDbType.NVarChar ),
                    new SqlParameter("@AddressLine2",  SqlDbType.NVarChar ),
                    new SqlParameter("@AddressLine3",  SqlDbType.NVarChar ),
                    new SqlParameter("@ModifiedBy",  SqlDbType.Int ),
                    new SqlParameter("@OrderId",  SqlDbType.Int )
                };

                parameters[0].Value = order.OrderAds != null ? helper.GetJsonString(order.OrderAds) : null;
                parameters[1].Value = order.UserId;
                parameters[2].Value = order.OrderNumber;
                parameters[3].Value = order.PaymentMethod;
                parameters[4].Value = order.PaymentId;
                parameters[5].Value = order.TotalPrice;
                parameters[6].Value = order.City;
                parameters[7].Value = order.Country;
                parameters[8].Value = order.PhoneNumber;
                parameters[9].Value = order.Currency;
                parameters[10].Value = order.CountryCode;
                parameters[11].Value = order.StatusId;
                parameters[12].Value = order.AddressType;
                parameters[13].Value = order.AddressLine1;
                parameters[14].Value = order.AddressLine2;
                parameters[15].Value = order.AddressLine3;
                parameters[16].Value = order.ModifiedBy;
                parameters[17].Value = order.Id;
                _dataHelper.CommandWithParams(ProcedureNames.SpuOrders, parameters);

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
