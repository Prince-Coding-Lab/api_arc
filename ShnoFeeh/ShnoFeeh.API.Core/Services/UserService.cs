using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ShnoFeeh.API.Core.Common;
using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.API.Core.Entities;
using ShnoFeeh.API.Core.Enums;
using ShnoFeeh.API.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ShnoFeeh.API.Core.Services
{
    /// <summary>
    /// This class is for User service.
    /// It implements all methods of IUserService.
    /// </summary>
    /// <remarks>
    /// This class can add, update, delete and get methods.
    /// </remarks>
    public sealed class UserService : IUserService
    {
        #region Fields
        internal IDataAccessHelper _dataHelper = null;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        #endregion
        #region Constructors 
        public UserService(IDataAccessHelper dataHelper, IConfiguration configuration,
            IEmailSender emailSender)
        {
            _dataHelper = dataHelper;
            _configuration = configuration;
            _emailSender = emailSender;
        }
        #endregion
        #region Public Methods
        public async Task<DatabaseResponse> CreateUserAsync(AdminUsers user)
        {
            try
            {
                SqlParameter[] parameters =
                  {
                        new SqlParameter( "@FirstName",  SqlDbType.NVarChar ),
                        new SqlParameter( "@LastName",  SqlDbType.NVarChar ),
                        new SqlParameter( "@Email",  SqlDbType.NVarChar ),
                        new SqlParameter( "@EmailConfirmed",  SqlDbType.Bit ),
                        new SqlParameter( "@CountryCode",  SqlDbType.NVarChar ),
                        new SqlParameter( "@PhoneNumber",  SqlDbType.NVarChar ),
                        new SqlParameter( "@PhoneNumberConfirmed",  SqlDbType.Bit ),
                        new SqlParameter( "@Password",  SqlDbType.NVarChar ),
                        new SqlParameter( "@TwoFactorEnabled",  SqlDbType.Bit ),
                        new SqlParameter( "@CompanyName",  SqlDbType.NVarChar ),
                        new SqlParameter( "@CompanyTypeId",  SqlDbType.Int ),
                        new SqlParameter( "@UserStatus",  SqlDbType.Int ),
                        new SqlParameter( "@RoleId",  SqlDbType.Int ),
                        new SqlParameter( "@CreatedBy",  SqlDbType.Int ),
                        new SqlParameter( "@CountryId",  SqlDbType.Int ),
                        new SqlParameter( "@CityId",  SqlDbType.Int ),
                        new SqlParameter( "@Photo",  SqlDbType.NVarChar )
                    };

                parameters[0].Value = user.FirstName;
                parameters[1].Value = user.LastName;
                parameters[2].Value = user.Email;
                parameters[3].Value = 0;//user.EmailConfirmed;
                parameters[4].Value = user.CountryCode;
                parameters[5].Value = user.PhoneNumber;
                parameters[6].Value = 0; //user.PhoneNumberConfirmed;
                parameters[7].Value = user.Password;
                parameters[8].Value = 0;//user.TwoFactorEnabled;
                parameters[9].Value = user.CompanyName;
                parameters[10].Value = user.CompanyTypeId;
                parameters[11].Value = 13;// user.UserStatus;
                parameters[12].Value = user.RoleId;
                parameters[13].Value = user.CreatedBy;
                parameters[14].Value = user.CountryId;
                parameters[15].Value = user.CityId;
                parameters[16].Value = user.Photo;

                _dataHelper.CommandWithParams(ProcedureNames.SpiAdminUsers, parameters);

                DataTable dt = new DataTable();
                LoginUserDto userData = null;
                int result = await _dataHelper.RunAsync(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    userData = (from model in dt.AsEnumerable()
                                select new LoginUserDto()
                                {
                                    Id = model.Field<int>("Id"),
                                    FirstName = model.Field<string>("FirstName"),
                                    LastName = model.Field<string>("LastName"),
                                    Email = model.Field<string>("Email"),
                                    UserStatus = model.Field<string>("UserStatus"),
                                    PhoneNumber = model.Field<string>("PhoneNumber"),
                                    PhoneNumberConfirmed = model.Field<bool>("PhoneNumberConfirmed"),
                                    TwoFactorEnabled = model.Field<bool>("TwoFactorEnabled"),
                                    //RoleName = model.Field<string>("RoleName"),
                                    EmailConfirmed = model.Field<bool>("EmailConfirmed"),
                                    RoleId = model.Field<int?>("RoleId"),
                                    Lastlogin = model.Field<DateTime?>("Lastlogin"),
                                    CreatedDate = model.Field<DateTime?>("CreatedDate"),
                                    ModifiedDate = model.Field<DateTime?>("ModifiedDate"),
                                    CountryCode = model.Field<string>("CountryCode"),
                                    CompanyName = model.Field<string>("CompanyName")
                                    //CompanyType = model.Field<string>("CompanyType"),
                                }).FirstOrDefault();

                    //Send email confirmation once user is created succesfully
                    await SendEmailAsync(userData);
                }

                return new DatabaseResponse { ResponseCode = result };
            }
            finally
            {

                _dataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> UpdateUserAsync(AdminUsers user)
        {
            try
            {
                SqlParameter[] parameters =
                  {
                        new SqlParameter( "@FirstName",  SqlDbType.NVarChar ),
                        new SqlParameter( "@LastName",  SqlDbType.NVarChar ),
                        new SqlParameter( "@Email",  SqlDbType.NVarChar ),
                        new SqlParameter( "@EmailConfirmed",  SqlDbType.Bit ),
                        new SqlParameter( "@CountryCode",  SqlDbType.NVarChar ),
                        new SqlParameter( "@PhoneNumber",  SqlDbType.NVarChar ),
                        new SqlParameter( "@PhoneNumberConfirmed",  SqlDbType.Bit ),
                        new SqlParameter( "@TwoFactorEnabled",  SqlDbType.Bit ),
                        new SqlParameter( "@CompanyName",  SqlDbType.NVarChar ),
                        new SqlParameter( "@CompanyTypeId",  SqlDbType.Int ),
                        new SqlParameter( "@UserStatus",  SqlDbType.Int ),
                        new SqlParameter( "@RoleId",  SqlDbType.Int ),
                        new SqlParameter( "@ModifiedBy",  SqlDbType.Int ),
                        new SqlParameter( "@Id",  SqlDbType.Int ),
                        new SqlParameter( "@CountryId",  SqlDbType.Int ),
                        new SqlParameter( "@CityId",  SqlDbType.Int ),
                        new SqlParameter( "@Photo",  SqlDbType.NVarChar )
                    };

                parameters[0].Value = user.FirstName;
                parameters[1].Value = user.LastName;
                parameters[2].Value = user.Email;
                parameters[3].Value = 1;// user.EmailConfirmed;
                parameters[4].Value = user.CountryCode;
                parameters[5].Value = user.PhoneNumber;
                parameters[6].Value = 0;//user.PhoneNumberConfirmed;
                parameters[7].Value = 0;// user.TwoFactorEnabled;
                parameters[8].Value = user.CompanyName;
                parameters[9].Value = user.CompanyTypeId;
                parameters[10].Value = user.UserStatus;
                parameters[11].Value = user.RoleId;
                parameters[12].Value = user.ModifiedBy;
                parameters[13].Value = user.Id;
                parameters[14].Value = user.CountryId;
                parameters[15].Value = user.CityId;
                parameters[16].Value = user.Photo;

                _dataHelper.CommandWithParams(ProcedureNames.SpuAdminUsers, parameters);

                DataTable dt = new DataTable();

                int result = await _dataHelper.RunAsync(dt);

                return new DatabaseResponse { ResponseCode = result };
            }
            finally
            {

                _dataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> GetUserByIdAsync(int userId)
        {
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter( "@UserId",  SqlDbType.Int )
                };

                parameters[0].Value = userId;

                _dataHelper.CommandWithParams(ProcedureNames.SpsAdminUserById, parameters);

                DataTable dt = new DataTable();

                int result = await _dataHelper.RunAsync(dt);

                List<UserDto> user = new List<UserDto>();

                if (dt != null && dt.Rows.Count > 0)
                {
                    user = (from model in dt.AsEnumerable()
                            select new UserDto()
                            {
                                Id = model.Field<int>("Id"),
                                FirstName = model.Field<string>("FirstName"),
                                LastName = model.Field<string>("LastName"),
                                Email = model.Field<string>("Email"),
                                EmailConfirmed = model.Field<bool>("EmailConfirmed"),
                                CountryCode = model.Field<string>("CountryCode"),
                                PhoneNumber = model.Field<string>("PhoneNumber"),
                                PhoneNumberConfirmed = model.Field<bool>("PhoneNumberConfirmed"),
                                RoleId = model.Field<int?>("RoleId"),
                                TwoFactorEnabled = model.Field<bool>("TwoFactorEnabled"),
                                CompanyName = model.Field<string>("CompanyName"),
                                CompanyTypeId = model.Field<int?>("CompanyTypeId"),
                                Lastlogin = model.Field<DateTime?>("Lastlogin"),
                                UserStatus = model.Field<string>("UserStatus"),
                                CreatedDate = model.Field<DateTime?>("CreatedDate"),
                                CreatedBy = model.Field<int?>("CreatedBy"),
                                ModifiedDate = model.Field<DateTime?>("ModifiedDate"),
                                ModifiedBy = model.Field<int?>("ModifiedBy"),
                                RoleName = model.Field<string>("RoleName"),
                                CompanyType = model.Field<string>("CompanyType"),
                                CountryId = model.Field<int?>("CountryId"),
                                Country = model.Field<string>("Country"),
                                CityId = model.Field<int?>("CityId"),
                                Photo = model.Field<string>("Photo")
                            }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = user };
            }
            finally
            {
                _dataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> GetUsersAsync(int? RoleId)
        {
            try
            {
                SqlParameter[] parameters =
                {
                        new SqlParameter( "@RoleId",  SqlDbType.Int ),
                };

                parameters[0].Value = RoleId;

                _dataHelper.CommandWithParams(ProcedureNames.SpsAdminUsers, parameters);

                DataTable dt = new DataTable();

                int result = await _dataHelper.RunAsync(dt);

                List<UserDto> user = new List<UserDto>();

                if (dt != null && dt.Rows.Count > 0)
                {
                    user = (from model in dt.AsEnumerable()
                            select new UserDto()
                            {
                                Id = model.Field<int>("Id"),
                                FirstName = model.Field<string>("FirstName"),
                                LastName = model.Field<string>("LastName"),
                                Email = model.Field<string>("Email"),
                                EmailConfirmed = model.Field<bool>("EmailConfirmed"),
                                CountryCode = model.Field<string>("CountryCode"),
                                PhoneNumber = model.Field<string>("PhoneNumber"),
                                PhoneNumberConfirmed = model.Field<bool>("PhoneNumberConfirmed"),
                                RoleId = model.Field<int>("RoleId"),
                                TwoFactorEnabled = model.Field<bool>("TwoFactorEnabled"),
                                CompanyName = model.Field<string>("CompanyName"),
                                CompanyTypeId = model.Field<int?>("CompanyTypeId"),
                                Lastlogin = model.Field<DateTime?>("Lastlogin"),
                                UserStatus = model.Field<string>("UserStatus"),
                                CreatedDate = model.Field<DateTime?>("CreatedDate"),
                                CreatedBy = model.Field<int?>("CreatedBy"),
                                ModifiedDate = model.Field<DateTime?>("ModifiedDate"),
                                ModifiedBy = model.Field<int?>("ModifiedBy"),
                                RoleName = model.Field<string>("RoleName"),
                                CompanyType = model.Field<string>("CompanyType"),
                                CountryId = model.Field<int?>("CountryId"),
                                Country = model.Field<string>("Country"),
                                CityId = model.Field<int?>("CityId"),
                                Photo = model.Field<string>("Photo")
                            }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = user };
            }
            finally
            {
                _dataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> DeleteUserAsync(int userId)
        {
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter( "@UserId",  SqlDbType.Int )
                };

                parameters[0].Value = userId;

                _dataHelper.CommandWithParams(ProcedureNames.SpdAdminUsers, parameters);

                DataTable dt = new DataTable();

                int result = await _dataHelper.RunAsync(dt);

                return new DatabaseResponse { ResponseCode = result };
            }
            finally
            {

                _dataHelper.Dispose();
            }
        }
        public async Task<UserDto> AuthenticateAsync(string username, string password)
        {
            try
            {
                UserDto user = null;

                SqlParameter[] parameters =
                {
                    new SqlParameter( "@Email",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Password",  SqlDbType.NVarChar )
                };

                parameters[0].Value = username;
                parameters[1].Value = password;
                _dataHelper.CommandWithParams(ProcedureNames.SpsAuthUser, parameters);

                DataTable dt = new DataTable();

                int result = await _dataHelper.RunAsync(dt);

                if (dt != null && dt.Rows.Count > 0)
                {
                    user = (from model in dt.AsEnumerable()
                            select new UserDto()
                            {
                                Id = model.Field<int>("Id"),
                                FirstName = model.Field<string>("FirstName"),
                                LastName = model.Field<string>("LastName"),
                                Email = model.Field<string>("Email"),
                                UserStatus = model.Field<string>("UserStatus"),
                                StatusId = model.Field<int>("StatusId"),
                                PhoneNumber = model.Field<string>("PhoneNumber"),
                                PhoneNumberConfirmed = model.Field<bool>("PhoneNumberConfirmed"),
                                TwoFactorEnabled = model.Field<bool>("TwoFactorEnabled"),
                                RoleName = model.Field<string>("RoleName"),
                                EmailConfirmed = model.Field<bool>("EmailConfirmed"),
                                RoleId = model.Field<int>("RoleId"),
                                Lastlogin = model.Field<DateTime?>("Lastlogin"),
                                CreatedDate = model.Field<DateTime?>("CreatedDate"),
                                ModifiedDate = model.Field<DateTime?>("ModifiedDate"),
                                CountryCode = model.Field<string>("CountryCode"),
                                CompanyTypeId = model.Field<int?>("CompanyTypeId"),
                                CompanyName = model.Field<string>("CompanyName"),
                                CompanyType = model.Field<string>("CompanyType"),
                                CountryId = model.Field<int?>("CountryId"),
                                Country = model.Field<string>("Country"),
                                CityId = model.Field<int?>("CityId"),
                                Photo = model.Field<string>("Photo")
                            }).FirstOrDefault();
                }

                // return null if user not found
                if (user == null)
                    return null;

                // authentication successful so generate jwt token
                user.Token = GenerateJwtToken(user);
                return user;
            }
            finally
            {
                _dataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> ChangePasswordAsync(UserPasswordDto user)
        {
            try
            {
                SqlParameter[] parameters =
               {
                        new SqlParameter( "@UserId",  SqlDbType.Int ),
                        new SqlParameter( "@CurrentPass",  SqlDbType.NVarChar ),
                        new SqlParameter( "@NewPass",  SqlDbType.NVarChar ),
                    };

                parameters[0].Value = user.UserId;
                parameters[1].Value = user.CurrentPassword;
                parameters[2].Value = user.NewPassword;
                _dataHelper.CommandWithParams(ProcedureNames.SpuAdminUsersPassword, parameters);

                DataTable dt = new DataTable();

                int result = await _dataHelper.RunAsync(dt);
                return new DatabaseResponse { ResponseCode = result };
            }
            finally
            {
                _dataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> CheckValidUserAsync(string email)
        {
            UserDto user = null;
            // password = Cryptography.Encrypt(password);
            SqlParameter[] parameters =
               {
                        new SqlParameter( "@Email",  SqlDbType.NVarChar )
                    };

            parameters[0].Value = email;

            _dataHelper.CommandWithParams(ProcedureNames.SpsValidWebuser, parameters);

            DataTable dt = new DataTable();

            int result = await _dataHelper.RunAsync(dt);

            if (dt != null && dt.Rows.Count > 0)
            {
                user = (from model in dt.AsEnumerable()
                        select new UserDto()
                        {
                            Id = model.Field<int>("Id"),
                            FirstName = model.Field<string>("FirstName"),
                            LastName = model.Field<string>("LastName"),
                            Email = model.Field<string>("Email")
                        }).FirstOrDefault();

                string token = new RandomSG().GetString();

                await this.CreateVerificationTokenAsync(new VerificationTokenDto()
                {
                    Token = token,
                    UserID = user.Id,
                    Purpose = "SignUp"
                });
                string EmailToken = Cryptography.Encrypt(user.Id.ToString() + "_" + token);
                string emailBody = "Your user has been created, please click link below to activate your account";
                string url = string.Format(_configuration.GetSection("VerifyEmailUrl").Value, EmailToken);
                EmailAddressDto emailDto = new Email(_configuration).CreateEmailObject(user.FirstName + " " + user.FirstName, user.Email, "Reset Password",
                    emailBody, url, "forAction_cust");
                if (!await _emailSender.SendEmailAsync(emailDto))
                {
                    result = (int)DbReturnValue.EmailNotSent;
                }
            }
            return new DatabaseResponse { ResponseCode = result, Results = null };

        }
        public async Task<DatabaseResponse> ResetPasswordAsync(ResetPasswordDto user)
        {
            string VerifiedToken = string.Empty;
            int user_id; string token = null;

            try
            {
                if (!string.IsNullOrEmpty(user.code))
                {
                    VerifiedToken = Cryptography.Decrypt(user.code);
                }
                else
                    throw new Exception("code not provided");

                if (VerifiedToken.IndexOf("_") >= 0)
                {
                    user_id = int.Parse(VerifiedToken.Split("_")[0]);
                    token = VerifiedToken.Split("_")[1];
                }
                else
                    return new DatabaseResponse { ResponseCode = 105, Results = null };

                user.Password = Cryptography.Encrypt(user.Password);

                SqlParameter[] parameters =
               {
                        new SqlParameter("@UserId",SqlDbType.Int),
                        new SqlParameter("@Password",  SqlDbType.NVarChar ),
                        new SqlParameter("@Code",  SqlDbType.NVarChar ),
                        new SqlParameter("@ModifiedBy",  SqlDbType.Int )
                 };

                parameters[0].Value = user_id;
                parameters[1].Value = user.Password;
                parameters[2].Value = token;
                parameters[3].Value = user.ModifiedBy;

                _dataHelper.CommandWithParams(ProcedureNames.SpuWebUserResetPassword, parameters);

                DataTable dt = new DataTable();

                int result = await _dataHelper.RunAsync(dt);
                return new DatabaseResponse { ResponseCode = result };
            }
            finally
            {
                _dataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> ConfirmVerificationToken(string token)
        {
            // GET TOKEN SENT BY EMAIL AND VERIFY USER

            string VerifiedToken = string.Empty;
            int user_id;

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    VerifiedToken = Cryptography.Decrypt(token);
                }

                if (VerifiedToken.IndexOf("_") >= 0)
                {
                    user_id = int.Parse(VerifiedToken.Split("_")[0]);
                    token = VerifiedToken.Split("_")[1];
                }
                else
                {
                    return new DatabaseResponse { ResponseCode = 105, Results = null };
                }

                SqlParameter[] parameters =
               {
                        new SqlParameter( "@user_id",  SqlDbType.Int ),
                        new SqlParameter( "@token",  SqlDbType.NVarChar ),
                    };

                parameters[0].Value = user_id;
                parameters[1].Value = token;

                _dataHelper.CommandWithParams(ProcedureNames.SpuAdminUsersVerifyToken, parameters);

                DataTable dt = new DataTable();

                int result = await _dataHelper.RunAsync(dt);

                return new DatabaseResponse { ResponseCode = result, Results = null };
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _dataHelper.Dispose();
            }
        }
        #endregion
        #region Private Methods
        private string GenerateJwtToken(UserDto user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings").GetSection("Secret").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role,user.RoleName.ToLower())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _configuration.GetSection("AppSettings").GetSection("Audience").Value,//<string>("AppSettings:Audience"),
                Issuer = _configuration.GetSection("AppSettings").GetSection("Issuer").Value,// _configuration.GetValue<string>("AppSettings:Issuer")
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }
        private async Task SendEmailAsync(LoginUserDto user)
        {
            string token = new RandomSG().GetString();
            await this.CreateVerificationTokenAsync(new VerificationTokenDto()
            {
                Token = token,
                UserID = user.Id,
                Purpose = "SignUp"
            });
            string EmailToken = Cryptography.Encrypt(user.Id.ToString() + "_" + token);
            string emailBody = "Your user has been created, please click link below to activate your account";
            string url = string.Format(_configuration.GetSection("VerifyEmailUrl").Value, EmailToken);
            EmailAddressDto emailDto = new Email(_configuration).CreateEmailObject(user.FirstName + " " + user.LastName, user.Email, "Create User",
                url, emailBody, "forAction_cust");
            await _emailSender.SendEmailAsync(emailDto);
         
        }
        private async Task CreateVerificationTokenAsync(VerificationTokenDto verifyRequest)
        {
            string EmailToken = string.Empty;

            try
            {
                SqlParameter[] parameters =
               {
                        new SqlParameter( "@user_id",  SqlDbType.Int ),
                        new SqlParameter( "@token",  SqlDbType.NVarChar ),
                        new SqlParameter( "@purpose",  SqlDbType.NVarChar ),
                        new SqlParameter( "@valid_to",  SqlDbType.DateTime ),
                    };

                parameters[0].Value = verifyRequest.UserID;
                parameters[1].Value = verifyRequest.Token;
                parameters[2].Value = verifyRequest.Purpose;
                parameters[3].Value = DateTime.Now.AddMinutes(30);

                _dataHelper.CommandWithParams(ProcedureNames.SpiVerificationToken, parameters);

                DataTable dt = new DataTable();

                int result = await _dataHelper.RunAsync(dt);
            }
            finally
            {
                _dataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> ActivateEmail(VerificationTokenDto model)
        {
            try
            {
                // THIS IS USED WHEN USER TRIED TO LOGIN BUT HIS EMAIL IS NOT ACTIVATED
                // IT WILL CREATE VERIFICATION TOKEN AND THEN SEND EMAIL TO USER

                string token = new RandomSG().GetString();

                await this.CreateVerificationTokenAsync(new VerificationTokenDto()
                {
                    Token = token,
                    UserID = model.UserID,
                    Purpose = "Activate Email"
                });

                string EmailToken = Cryptography.Encrypt(model.UserID.ToString() + "_" + token);
                string emailBody = "Please click link below to activate your account";

                string url = string.Format(_configuration.GetSection("VerifyEmailUrl").Value, EmailToken);

                EmailAddressDto emailDto = new Email(_configuration).CreateEmailObject(model.UserName, model.Email, "Activate Email",
                    url, emailBody, "forAction_cust");
                int result = 0;
                if (!await _emailSender.SendEmailAsync(emailDto))
                {
                    result = (int)DbReturnValue.EmailNotSent;
                }
                else
                {
                    result = (int)DbReturnValue.EmailSent;
                }
                return new DatabaseResponse { ResponseCode = result };
            }
            finally
            {

            }
        }
        #endregion
    }
}
