using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RestSharp;
using ShnoFeeh.BusinessService.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ShnoFeeh.BusinessService.Factories
{
    public class ShnoFeehHttpFactory : IShnoFeehHttpFactory
    {
        private readonly IHttpClientFactory _clientFactory;
        public ShnoFeehHttpFactory(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        /// <summary>
        /// GetAsync makes an async GET request to the passed URI
        /// It returns Deserialized object of Type T.
        /// </summary>
        /// <typeparam name="T">Response deserializes to the passed T type.</typeparam>
        /// <param name="requestUri">HTTP Request endpoint url </param>
        /// <param name="scheme">Default null. Used for authorization. </param>
        /// <param name="schemeToken">Default null. Used for authorization.</param>
        /// <param name="acceptLangauage">Http AcceptLanguage header</param>
        /// <returns></returns>
        public async Task<T> GetData<T>(string requestUri, string scheme = null, string schemeToken = null, string acceptLangauage = null, List<Dictionary<string, string>> headers = null) where T : class
        {
            T data = null;
            using (var client = _clientFactory.CreateClient("IMSHttpClientFactory"))
            {
                // check if langauage avaialble then add to httpclient request header.
                if (string.IsNullOrEmpty(acceptLangauage) == false)
                {
                    client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(acceptLangauage));
                }
                // check if scheme avaialble then add to httpclient request header.
                if (string.IsNullOrEmpty(scheme) == false && string.IsNullOrEmpty(schemeToken) == false)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, schemeToken);
                }
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Keys.FirstOrDefault(), header[header.Keys.FirstOrDefault()]);
                    }
                }
                try
                {
                    var responseData = await client.GetStringAsync(requestUri);
                    if (!string.IsNullOrWhiteSpace(responseData))
                    {
                        data = JsonConvert.DeserializeObject<T>(responseData);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return data;
        }

    
        /// <summary>
        /// GetAsyncReturnsObject makes an async GET request to the passed URI
        /// It returns Deserialized object of Type T.
        /// </summary>
        /// <typeparam name="T">Response deserializes to the passed T type.</typeparam>
        /// <param name="requestUri">HTTP Request endpoint url </param>
        /// <param name="scheme">Default null. Used for authorization. </param>
        /// <param name="schemeToken">Default null. Used for authorization.</param>
        /// <param name="acceptLangauage">Http AcceptLanguage header</param>
        /// <returns></returns>
        public async Task<T> GetAsyncReturnsObject<T>(string requestUri, string scheme = null, string schemeToken = null, string acceptLangauage = null) where T : class
        {
            T data = null;
            using (var client = _clientFactory.CreateClient("IMSHttpClientFactory"))
            {
                // check if langauage avaialble then add to httpclient request header.
                if (string.IsNullOrEmpty(acceptLangauage) == false)
                {
                    client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(acceptLangauage));
                }
                // check if scheme avaialble then add to httpclient request header.
                if (string.IsNullOrEmpty(scheme) == false && string.IsNullOrEmpty(schemeToken) == false)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, schemeToken);
                }
                var response = await client.GetAsync(requestUri);
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync(),
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
                }
                if (response.StatusCode == (HttpStatusCode.Unauthorized))
                {
                    throw new UnauthorizedAccessException();
                }
            }
            return null;
        }

        /// <summary>
        /// PostAsync makes an async POST request to the passed URI
        /// It deserializes the passed T type contentObject, and returns Deserialized object of Type U
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="contentObject"></param>
        /// <param name="URI"></param>
        /// <param name="token">Default null. Used for authorization. </param>
        /// <returns></returns>
        public async Task<U> PostAsyncReturnsObject<T, U>(T contentObject, string URI, string token = null, bool? checkResponse = true) where U : class
        {
            try
            {
                HttpResponseMessage response;
                using (var client = _clientFactory.CreateClient("IMSHttpClientFactory"))
                {
                    // check if Bearer token available then add to httpclient request header.
                    if (string.IsNullOrEmpty(token) == false)
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                     var content = new StringContent(JsonConvert.SerializeObject(contentObject), Encoding.UTF8, "application/json");
                    response = await client.PostAsync(URI, content);
                    if (response.IsSuccessStatusCode)
                    {                       
                        dynamic convertedResponse = JsonConvert.DeserializeObject<U>(
                            await response.Content.ReadAsStringAsync(),
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });                        
                        if (checkResponse != null && checkResponse.Value == false) return convertedResponse;
                        var property = convertedResponse.GetType().GetProperty("Message");
                        if (property != null)
                        {
                            if (convertedResponse.Message.ToString() != "Record exists in database")
                                return convertedResponse;
                            convertedResponse.StatusCode = "409";
                        }
                        return convertedResponse;
                    }
                    if (response.StatusCode == (HttpStatusCode.Unauthorized))
                    {
                        throw new UnauthorizedAccessException();
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return JsonConvert.DeserializeObject<U>(ex.Message);
            }
        }
        public async Task<U> PostAsyncReturnsObject<T, U>(string contentObject, string URI, string token = null, bool? checkResponse = true) where U : class
        {
            try
            {
                HttpResponseMessage response;
                using (var client = _clientFactory.CreateClient("IMSHttpClientFactory"))
                {
                    // check if Bearer token available then add to httpclient request header.
                    if (string.IsNullOrEmpty(token) == false)
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }                    
                    var content = new StringContent(JsonConvert.SerializeObject(contentObject), Encoding.UTF8, "application/json");
                    //var content = new StringContent(JsonConvert.SerializeObject(dsa), Encoding.UTF8, "application/json");
                    response = await client.PostAsync(URI, content);
                    if (response.IsSuccessStatusCode)
                    {
                        dynamic convertedResponse = JsonConvert.DeserializeObject<U>(
                            await response.Content.ReadAsStringAsync(),
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        //dynamic x = convertedResponse;
                        if (checkResponse != null && checkResponse.Value == false) return convertedResponse;
                        var property = convertedResponse.GetType().GetProperty("Message");
                        if (property != null)
                        {
                            if (convertedResponse.Message.ToString() != "Record exists in database")
                                return convertedResponse;
                            convertedResponse.StatusCode = "409";
                        }
                        return convertedResponse;
                    }
                    if (response.StatusCode == (HttpStatusCode.Unauthorized))
                    {
                        throw new UnauthorizedAccessException();
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return JsonConvert.DeserializeObject<U>(ex.Message);
            }
        }
        /// <summary>
        /// PostAsync makes an async POST request to the passed URI
        /// It deserializes the passed T type contentObject, and returns Deserialized object of Type U
        /// </summary>
        /// <typeparam name="T"> type of the content object </typeparam>
        /// <typeparam name="U">type of the expected return object </typeparam>
        /// <param name="contentObject"></param>
        /// <param name="URI"></param>
        /// <returns></returns>
        public async Task<U> PostAsyncReturnsStruct<T, U>(T contentObject, string URI) where U : struct
        {
            U defaultResponse = default(U);

            HttpResponseMessage response;

            using (var client = _clientFactory.CreateClient("IMSHttpClientFactory"))
            {
                var content = new StringContent(JsonConvert.SerializeObject(contentObject), Encoding.UTF8, "application/json");
                response = await client.PostAsync(URI, content);

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<U>(await response.Content.ReadAsStringAsync());
                }
                else if (response.StatusCode == (HttpStatusCode.Unauthorized))
                {
                    throw new UnauthorizedAccessException();
                }
            }
            return defaultResponse;
        }

        /// <summary>
        /// PutAsyncReturnsObject makes an async PUT request to the passed URI
        /// It deserializes the passed T type contentObject, and returns Deserialized object of Type U
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="contentObject"></param>
        /// <param name="URI"></param>
        /// <param name="token">Default null. Used for authorization. </param>
        /// <returns></returns>
        public async Task<U> PutAsyncReturnsObject<T, U>(T contentObject, string URI, string token = null) where U : class
        {
            
            HttpResponseMessage response;
            using (var client = _clientFactory.CreateClient("IMSHttpClientFactory"))
            {
                // check if Bearer token available then add to httpclient request header.
                if (string.IsNullOrEmpty(token) == false)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                var content = new StringContent(JsonConvert.SerializeObject(contentObject), Encoding.UTF8, "application/json");
                response = await client.PutAsync(URI, content);
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<U>(await response.Content.ReadAsStringAsync());
                }
                else if (response.StatusCode == (HttpStatusCode.Unauthorized))
                {
                    throw new UnauthorizedAccessException();
                }
                else if(response.StatusCode == (HttpStatusCode.BadRequest))
                {
                    throw new Exception("Bad Request, " + response.ReasonPhrase + ", " + response.Content);
                }
            }
            return null;
        }

        /// <summary>
        /// DeleteAsyncReturnsObject makes an async Delete request to the passed URI
        /// It deserializes the passed T type contentObject, and returns Deserialized object of Type U
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="contentObject"></param>
        /// <param name="URI"></param>
        /// <param name="token">Default null. Used for authorization. </param>
        /// <returns></returns>
        public async Task<T> DeleteAsyncReturnsObject<T>(string URI, string token = null) where T : class
        {
            HttpResponseMessage response;
            using (var client = _clientFactory.CreateClient("IMSHttpClientFactory"))
            {
                // check if Bearer token available then add to httpclient request header.
                if (string.IsNullOrEmpty(token) == false)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                //var content = new StringContent(JsonConvert.SerializeObject(contentObject), Encoding.UTF8, "application/json");
                response = await client.DeleteAsync(URI);

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
                }
                else if (response.StatusCode == (HttpStatusCode.Unauthorized))
                {
                    throw new UnauthorizedAccessException();
                }
            }
            return null;
        }
        public async Task<U> UploadFileAsyncReturnsObject<T, U>(Image file, string URI, string token = null, bool? checkResponse = true) where U : class
        {
            try
            {
                using (var client = _clientFactory.CreateClient("IMSHttpClientFactory"))
                {
                    var stream = file.File.OpenReadStream();
                    MemoryStream mStream = new MemoryStream();
                    stream.CopyTo(mStream);
                    var rClient = new RestClient(URI);
                    rClient.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Authorization", "Bearer " +token);
                    request.AddFile("file", mStream.ToArray(), file.File.FileName);
                    IRestResponse resp = rClient.Execute(request);

                    if (resp.IsSuccessful)
                    {
                        dynamic convertedResponse = JsonConvert.DeserializeObject<U>(
                            resp.Content,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
                        //dynamic x = convertedResponse;
                        if (checkResponse != null && checkResponse.Value == false) return convertedResponse;
                        var property = convertedResponse.GetType().GetProperty("Message");
                        if (property != null)
                        {
                            if (convertedResponse.Message != null && convertedResponse.Message.ToString() != "Record exists in database")
                                return convertedResponse;
                            convertedResponse.StatusCode = "409";
                        }
                        return convertedResponse;
                    }
                    if (resp.StatusCode == (HttpStatusCode.Unauthorized))
                    {
                        throw new UnauthorizedAccessException();
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return JsonConvert.DeserializeObject<U>(ex.Message);
            }
        }
    }
}
