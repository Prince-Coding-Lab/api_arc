using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ShnoFeeh.BusinessService.Interfaces;
using ShnoFeeh.BusinessService.Factories;
using ShnoFeeh.BusinessService.Common.Manager;
using ShnoFeeh.BusinessService.Common.Methods;
using ShnoFeeh.BusinessService.Dto;
using ShnoFeeh.BusinessService.Common.Constant;
using ShnoFeeh.API.Core.Dto;

namespace ShnoFeeh.BusinessService.Services
{
    public class AccountService : IAccountService
    {
        private readonly ISessionManager _sessionManager;
        private readonly IShnoFeehHttpFactory _httpClientFactory;
        private readonly ICommonService _common;        

        public AccountService(IShnoFeehHttpFactory httpClientFactory, ICommonService common,ISessionManager sessionManager)
        {
            this._httpClientFactory = httpClientFactory;
            _sessionManager = sessionManager;
            _common = common;
        }
        public async Task<UserDto> LoginAsync(AuthenticateModel requestDto)
        {
            var response = await _httpClientFactory.PostAsyncReturnsObject<AuthenticateModel, UserDto>(requestDto, _common.GetUri(ApiUris.LoginUri), null, false);
            return response;
        }
        public async Task<ResponseDto<UserDto>> RegisterAsync(UserCreateDto model)
        {
            var response = await _httpClientFactory.PostAsyncReturnsObject<UserCreateDto, ResponseDto<UserDto>>(model, _common.GetUri(ApiUris.AddUser));
            return response;
        }        
        public async Task<ResponseDto<UserDto>> UpdateAsync(UserUpdateDto model)
        {
            var response = await _httpClientFactory.PutAsyncReturnsObject<UserUpdateDto, ResponseDto<UserDto>>(model, _common.GetUri(ApiUris.UpdateUser), _sessionManager.GetString(SessionItems.Token));
            return response;
        }
        public async Task<ResponseDto<UserPasswordDto>> ResetAsync(UserPasswordDto model)
        {
            var response = await _httpClientFactory.PutAsyncReturnsObject<UserPasswordDto, ResponseDto<UserPasswordDto>>(model, _common.GetUri(ApiUris.ResetPassword), _sessionManager.GetString(SessionItems.Token));
            return response;
        }
        public async Task<ResponseDto<UserPasswordDto>> ChangePasswordAsync(UserPasswordDto model)
        {
            var response = await _httpClientFactory.PutAsyncReturnsObject<UserPasswordDto, ResponseDto<UserPasswordDto>>(model, _common.GetUri(ApiUris.ChangePassword), _sessionManager.GetString(SessionItems.Token));
            return response;
        }
       
        public async Task<ResponseDto<ResetPasswordDto>> ResetPasswordAsync(ResetPasswordDto model)
        {
            var response = await _httpClientFactory.PutAsyncReturnsObject<ResetPasswordDto, ResponseDto<ResetPasswordDto>>(model, _common.GetUri(ApiUris.ResetPassword));
            return response;
        }

        public async Task<ResponseDto<object>> VerifyEmailToken(string token)
        {
            ResponseDto<object> response = await _httpClientFactory.PutAsyncReturnsObject<string, ResponseDto<object>>(token,string.Format(_common.GetUri(ApiUris.ConfirmToken),token));

            return response;
        }

        public async Task<ResponseDto<UserEmailDto>> ForgotPasswordAsync(UserEmailDto model)
        {
            var response = await _httpClientFactory.GetAsyncReturnsObject<ResponseDto<UserEmailDto>>(string.Format(_common.GetUri(ApiUris.ForgotPassword),model.email));
            return response;
        }


        public async Task<ResponseDto<VerificationTokenDto>> ActivateEmail(VerificationTokenDto model)
        {

            var response = await _httpClientFactory.PostAsyncReturnsObject<VerificationTokenDto, ResponseDto<VerificationTokenDto>>(model, _common.GetUri(ApiUris.ActivateEmail),_sessionManager.GetString(SessionItems.Token));

            return response;
        }

        public async Task<UploadImageResponse> UploadProfileImage(Image model)
        {
            var apilink = _common.GetUri(ApiUris.UploadProfilePhoto);

            var response = await _httpClientFactory.UploadFileAsyncReturnsObject<Image, UploadImageResponse>(model, apilink, _sessionManager.GetString(SessionItems.Token));
            return response;
        }

        //Users

        public async Task<ResponseDto<List<UserDto>>> GetAllUsersAsync(string token)
        {
            var response = await _httpClientFactory.GetAsyncReturnsObject<ResponseDto<List<UserDto>>>(_common.GetUri(ApiUris.GetAllUsers), "Bearer", token);
            return response;
        }

        public async Task<ResponseDto<string>> DeleteUserAsync(string token, int userId)
        {
            var response = await _httpClientFactory.DeleteAsyncReturnsObject<ResponseDto<string>>(String.Format(_common.GetUri(ApiUris.DeleteUser), userId), token);
            return response;
        }

        public async Task<ResponseDto<UserDto>> UpdateUserStatusAsync(UserDto model, string token)
        {
            UserUpdateDto userUpdateDto = new UserUpdateDto();

            userUpdateDto.FirstName = model.FirstName;
            userUpdateDto.LastName = model.LastName;
            userUpdateDto.Email = model.Email;
            userUpdateDto.EmailConfirmed = model.EmailConfirmed;
            userUpdateDto.CountryCode = model.CountryCode;
            userUpdateDto.PhoneNumber = model.PhoneNumber;
            userUpdateDto.PhoneNumberConfirmed = model.PhoneNumberConfirmed;
            userUpdateDto.TwoFactorEnabled = model.TwoFactorEnabled;
            userUpdateDto.CompanyName = model.CompanyName;
            userUpdateDto.CompanyTypeId = model.CompanyTypeId;                      
            userUpdateDto.UserStatus = model.UserStatus == "InActive" ? 6 : 5;
            userUpdateDto.RoleId = model.RoleId??0;
            userUpdateDto.ModifiedBy = model.Id;
            userUpdateDto.Id = model.Id;
            userUpdateDto.CountryId = model.CountryId;
            userUpdateDto.CityId = model.CityId;


            var response = await _httpClientFactory.PutAsyncReturnsObject<UserUpdateDto, ResponseDto<UserDto>>(userUpdateDto, _common.GetUri(ApiUris.UpdateUser), token);
            return response;
        }

    }
}
