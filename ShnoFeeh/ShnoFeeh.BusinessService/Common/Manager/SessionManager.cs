using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.BusinessService.Common.Constant;
using ShnoFeeh.BusinessService.Dto;
using System;

namespace ShnoFeeh.BusinessService.Common.Manager
{
    public class SessionManager : ISessionManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDataProtector _protector;

        public SessionManager(IHttpContextAccessor httpContextAccessor, IDataProtectionProvider provider)
        {
            _httpContextAccessor = httpContextAccessor;
            _protector = provider.CreateProtector("CookieEncryption");
        }
        public void SetSession(UserDto response)
        {
            _httpContextAccessor.HttpContext.Session.SetInt32(SessionItems.Id, response.Id);
            _httpContextAccessor.HttpContext.Session.SetInt32(SessionItems.RoleId, response.RoleId ?? 0);
            _httpContextAccessor.HttpContext.Session.SetString(SessionItems.RoleName, response.RoleName);
            _httpContextAccessor.HttpContext.Session.SetString(SessionItems.Token, response.Token);
            _httpContextAccessor.HttpContext.Session.SetString(SessionItems.UserName, response.FirstName + " " + response.LastName);
            _httpContextAccessor.HttpContext.Session.SetString(SessionItems.Email, response.Email);
            _httpContextAccessor.HttpContext.Session.SetInt32(SessionItems.EmailConfirmed, response.EmailConfirmed ? 1 : 0);
            _httpContextAccessor.HttpContext.Session.SetString(SessionItems.PhoneNumber, response.PhoneNumber);
            _httpContextAccessor.HttpContext.Session.SetInt32(SessionItems.PhoneNumberConfirmed, response.PhoneNumberConfirmed ? 1 : 0);
            _httpContextAccessor.HttpContext.Session.SetString(SessionItems.CompanyName, response.CompanyName ?? "");
            _httpContextAccessor.HttpContext.Session.SetString(SessionItems.CompanyType, response.CompanyType ?? "");
            _httpContextAccessor.HttpContext.Session.SetInt32(SessionItems.CompanyTypeId, response.CompanyTypeId ?? 0);
            _httpContextAccessor.HttpContext.Session.SetString(SessionItems.CreatedDate, response.CreatedDate.ToString());
            _httpContextAccessor.HttpContext.Session.SetString(SessionItems.Photo, response.Photo ?? "");
            _httpContextAccessor.HttpContext.Session.SetInt32(SessionItems.CountryId, response.CountryId ?? 0);
            _httpContextAccessor.HttpContext.Session.SetInt32(SessionItems.CityId, response.CityId ?? 0);
            _httpContextAccessor.HttpContext.Session.SetString(SessionItems.Country, response.Country??"");
        }

        public string GetString(string key)
        {
            var val = _httpContextAccessor.HttpContext.Session.GetString(SessionItems.RoleId);
            return _httpContextAccessor.HttpContext.Session.GetString(key);
        }
        public void SetString(string key, string value)
        {
            _httpContextAccessor.HttpContext.Session.SetString(key, value);
        }

        public void SetInt(string key, int value)
        {
            _httpContextAccessor.HttpContext.Session.SetInt32(key, value);
        }

        public int? GetInt(string key)
        {
            return _httpContextAccessor.HttpContext.Session.GetInt32(key);
        }
        public void Remove(string key)
        {
            _httpContextAccessor.HttpContext.Session.Remove(key);
        }


        public void SetObject(string key, object value)
        {
            _httpContextAccessor.HttpContext.Session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public T GetObject<T>(string key)
        {
            var value = _httpContextAccessor.HttpContext.Session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public bool IsActive()
        {
            if (_httpContextAccessor.HttpContext.Session.GetString(SessionItems.Id) == null)
                return false;
            else
                return true;
        }

        public string GetCookie(string key, bool name = true)
        {
            if (name)
            {
                return _httpContextAccessor.HttpContext.Request.Cookies[key];
            }
            else
            {
                return _protector.Unprotect(_httpContextAccessor.HttpContext.Request.Cookies[key]);
            }
        }
        /// <summary>  
        /// set the cookie  
        /// </summary>  
        /// <param name="key">key (unique indentifier)</param>  
        /// <param name="value">value to store in cookie object</param>  
        /// <param name="expireTime">expiration time</param>  
        public void SetCookie(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();
            if (expireTime.HasValue)
            {
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
                option.Secure = true;
            }

            else
            {
                option.Expires = DateTime.Now.AddMilliseconds(10);
                option.Secure = true;
            }
            _httpContextAccessor.HttpContext.Response.Cookies.Append(key, _protector.Protect(value), option);
        }
        /// <summary>  
        /// Delete the key  
        /// </summary>  
        /// <param name="key">Key</param>  
        public void RemoveCookie(string key)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(key);
        }
    }
}
