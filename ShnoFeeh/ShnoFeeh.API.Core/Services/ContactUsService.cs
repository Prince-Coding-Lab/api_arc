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
    /// This class is for ContactUs service.
    /// It implements all methods of IContactUsService.
    /// </summary>
    /// <remarks>
    /// This class can add, update, delete and get methods.
    /// </remarks>
    public sealed class ContactUsService : IContactUsService
    {
        #region Fields
        internal IDataAccessHelper _dataHelper = null;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        #endregion
        #region Constructors 
        public ContactUsService(IDataAccessHelper dataHelper,
            IConfiguration configuration, IEmailSender emailSender)
        {
            _dataHelper = dataHelper;
            _configuration = configuration;
            _emailSender = emailSender;
        }
        #endregion
        #region Public Methods
        public async Task<DatabaseResponse> GetEmailByIdAsync(int id)
        {
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter( "@Id",  SqlDbType.Int )
                };

                parameters[0].Value = id;

                _dataHelper.CommandWithParams(ProcedureNames.SpsContactUsById, parameters);

                DataTable dt = new DataTable();

                int result = await _dataHelper.RunAsync(dt);

                List<ContactUsDto> list = new List<ContactUsDto>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    list = (from model in dt.AsEnumerable()
                            select new ContactUsDto()
                            {
                                Id = model.Field<int>("Id"),
                                Name = model.Field<string>("Name"),
                                Subject = model.Field<string>("Subject"),
                                Phone = model.Field<string>("Phone"),
                                Email = model.Field<string>("Email"),
                                Content = model.Field<string>("Content"),
                                StatusId = model.Field<int?>("StatusId"),
                                Status = model.Field<string>("Status"),
                                CreatedDate = model.Field<DateTime?>("CreatedDate"),
                                ModifiedDate = model.Field<DateTime?>("ModifiedDate"),
                                CreatedBy = model.Field<string>("CreatedBy"),
                                ModifiedBy = model.Field<string>("ModifiedBy")
                            }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = list };
            }
            finally
            {
                _dataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> GetEmailsAsync()
        {
            try
            {
                _dataHelper.CommandWithoutParams(ProcedureNames.SpsContactUs);

                DataTable dt = new DataTable();

                int result = await _dataHelper.RunAsync(dt);

                List<ContactUsDto> list = new List<ContactUsDto>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    list = (from model in dt.AsEnumerable()
                            select new ContactUsDto()
                            {
                                Id = model.Field<int>("Id"),
                                Name = model.Field<string>("Name"),
                                Subject = model.Field<string>("Subject"),
                                Phone = model.Field<string>("Phone"),
                                Email = model.Field<string>("Email"),
                                Content = model.Field<string>("Content"),
                                StatusId = model.Field<int?>("StatusId"),
                                Status = model.Field<string>("Status"),
                                CreatedDate = model.Field<DateTime?>("CreatedDate"),
                                ModifiedDate = model.Field<DateTime?>("ModifiedDate"),
                                CreatedBy = model.Field<string>("CreatedBy"),
                                ModifiedBy = model.Field<string>("ModifiedBy")
                            }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = list };
            }
            finally
            {
                _dataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> SendEmailAsync(ContactUs contactUs)
        {
            try
            {

                SqlParameter[] parameters =
                  {
                        new SqlParameter( "@Name",  SqlDbType.NVarChar ),
                        new SqlParameter( "@Subject",  SqlDbType.NVarChar ),
                        new SqlParameter( "@Email",  SqlDbType.NVarChar ),
                        new SqlParameter( "@Phone",  SqlDbType.NVarChar ),
                        new SqlParameter( "@Content",  SqlDbType.NVarChar ),
                        new SqlParameter( "@StatusId",  SqlDbType.Int ),
                        new SqlParameter( "@CreatedBy",  SqlDbType.Int )
                    };

                parameters[0].Value = contactUs.Name;
                parameters[1].Value = contactUs.Subject;
                parameters[2].Value = contactUs.Email;
                parameters[3].Value = contactUs.Phone;
                parameters[4].Value = contactUs.Content;
                parameters[5].Value = contactUs.StatusId;
                parameters[6].Value = contactUs.CreatedBy;

                _dataHelper.CommandWithParams(ProcedureNames.SpiContactUs, parameters);

                DataTable dt = new DataTable();

                int result = await _dataHelper.RunAsync(dt);
                
                string emailBody = "Query : Name : {0}, Subject: {1},Email: {2},Phone: {3},Contact: {4}";
                emailBody = string.Format(emailBody, contactUs.Name, contactUs.Subject, contactUs.Email, contactUs.Phone, contactUs.Content);

                string email = _configuration.GetSection("AdminEmail").Value;

                EmailAddressDto emailDto = new Email(_configuration).CreateEmailObject("Admin", contactUs.Email, contactUs.Subject, "", emailBody, "forInfo");

                if (!await _emailSender.SendEmailAsync(emailDto))
                    throw new Exception("Cannot send email");

                return new DatabaseResponse { ResponseCode = result };
            }
            finally
            {

                _dataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> UpdateEmailAsync(ContactUs contactUs)
        {
            try
            {
                SqlParameter[] parameters =
                  {
                        new SqlParameter( "@Id",  SqlDbType.Int ),
                        new SqlParameter( "@Name",  SqlDbType.NVarChar ),
                        new SqlParameter( "@Subject",  SqlDbType.NVarChar ),
                        new SqlParameter( "@Email",  SqlDbType.NVarChar ),
                        new SqlParameter( "@Phone",  SqlDbType.NVarChar ),
                        new SqlParameter( "@Content",  SqlDbType.NVarChar ),
                        new SqlParameter( "@StatusId",  SqlDbType.Int ),
                        new SqlParameter( "@ModifiedBy",  SqlDbType.Int )
                    };
                parameters[0].Value = contactUs.Id;
                parameters[1].Value = contactUs.Name;
                parameters[2].Value = contactUs.Subject;
                parameters[3].Value = contactUs.Email;
                parameters[4].Value = contactUs.Phone;
                parameters[5].Value = contactUs.Content;
                parameters[6].Value = contactUs.StatusId;
                parameters[7].Value = contactUs.ModifiedBy;

                _dataHelper.CommandWithParams(ProcedureNames.SpuContactUs, parameters);

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
