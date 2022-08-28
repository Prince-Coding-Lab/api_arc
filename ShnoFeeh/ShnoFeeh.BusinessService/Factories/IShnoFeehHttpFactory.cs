using ShnoFeeh.BusinessService.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ShnoFeeh.BusinessService.Factories
{
    public interface IShnoFeehHttpFactory
    {
        Task<U> PostAsyncReturnsStruct<T, U>(T contentObject, string URI) where U : struct;
        Task<U> PostAsyncReturnsObject<T, U>(T contentObject, string URI, string token = null, bool? checkResponse = true) where U : class;
        Task<U> PostAsyncReturnsObject<T, U>(string contentObject, string URI, string token = null, bool? checkResponse = true) where U : class;
        Task<T> GetData<T>(string requestUri, string scheme = null, string schemeToken = null, string acceptLangauage = null, List<Dictionary<string, string>> headers = null) where T : class;
        Task<T> GetAsyncReturnsObject<T>(string requestUri, string scheme = null, string schemeToken = null, string acceptLangauage = null) where T : class;

        Task<T> DeleteAsyncReturnsObject<T>(string URI, string token = null) where T : class;
        Task<U> PutAsyncReturnsObject<T, U>(T contentObject, string URI, string token = null) where U : class;

        Task<U> UploadFileAsyncReturnsObject<T, U>(Image contentObject, string URI, string token = null, bool? checkResponse = true) where U : class;
    }
}
