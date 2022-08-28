using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ShnoFeeh.API.Core.Interfaces;
using ShnoFeeh.API.Core.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShnoFeeh.API.Infrastructure.Data
{
    public class HttpClientHelper : BaseService, IHttpClientHelper
    {
        public HttpClientHelper(IConfiguration configuration) : base(configuration)
        {
        }
        private static readonly HttpClient Client = new HttpClient();
        /// <summary>
        /// For getting a single item from a web api uaing GET
        /// </summary>
        /// <param name="apiUrl">Added to the base address to make the full url of the 
        ///     api get method, e.g. "products/1" to get a product with an id of 1</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The item requested</returns>
        public async Task<TResponse> GetSingleItemRequest<TResponse>(string apiUrl, CancellationToken cancellationToken)
        {
            var result = default(TResponse);
            if (Client.DefaultRequestHeaders.Authorization == null)
                Client.DefaultRequestHeaders.Add("authorization", SecretKey);
            var response = await Client.GetAsync(apiUrl, cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync().ContinueWith(x =>
                {
                    if (typeof(TResponse).Namespace != "System")
                    {
                        result = JsonConvert.DeserializeObject<TResponse>(x?.Result);
                    }
                    else result = (TResponse)Convert.ChangeType(x?.Result, typeof(TResponse));
                }, cancellationToken);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }
            return result;
        }

        /// <summary>
        /// For getting multiple (or all) items from a web api using GET
        /// </summary>
        /// <param name="apiUrl">Added to the base address to make the full url of the 
        ///     api get method, e.g. "products?page=1" to get page 1 of the products</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The items requested</returns>
        public async Task<TResponse[]> GetMultipleItemsRequest<TResponse>(string apiUrl, CancellationToken cancellationToken)
        {
            TResponse[] result = null;
            if (Client.DefaultRequestHeaders.Authorization == null)
                Client.DefaultRequestHeaders.Add("authorization", SecretKey);
            var response = await Client.GetAsync(apiUrl, cancellationToken).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    result = JsonConvert.DeserializeObject<TResponse[]>(x.Result);
                }, cancellationToken);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }
            return result;
        }

        /// <summary>
        /// For creating a new item over a web api using POST
        /// </summary>
        /// <param name="apiUrl">Added to the base address to make the full url of the 
        ///     api post method, e.g. "products" to add products</param>
        /// <param name="postObject">The object to be created</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The item created</returns>
        public async Task<TResponse> PostRequest<TRequest, TResponse>(string apiUrl, TRequest postObject, CancellationToken cancellationToken)
        {
            TResponse result = default(TResponse);
            string payload = JsonConvert.SerializeObject(postObject);
            var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");
            if (Client.DefaultRequestHeaders.Authorization == null)
                Client.DefaultRequestHeaders.Add("authorization", SecretKey);
            var response = await Client.PostAsync(apiUrl, httpContent, cancellationToken).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    result = JsonConvert.DeserializeObject<TResponse>(x.Result);
                }, cancellationToken);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }
            return result;
        }

        /// <summary>
        /// For updating an existing item over a web api using PUT
        /// </summary>
        /// <param name="apiUrl">Added to the base address to make the full url of the 
        ///     api put method, e.g. "products/3" to update product with id of 3</param>
        /// <param name="putObject">The object to be edited</param>
        /// <param name="cancellationToken"></param>
        public async Task<TResponse> PutRequest<TRequest, TResponse>(string apiUrl, TRequest putObject, CancellationToken cancellationToken)
        {
            TResponse result = default(TResponse);
            string payload = JsonConvert.SerializeObject(putObject);
            var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");
            if (Client.DefaultRequestHeaders.Authorization == null)
                Client.DefaultRequestHeaders.Add("authorization", SecretKey);
            var response = await Client.PutAsync(apiUrl, httpContent, cancellationToken).ConfigureAwait(false);
            //if (!response.IsSuccessStatusCode)
            //{
            //    var content = await response.Content.ReadAsStringAsync();
            //    response.Content?.Dispose();
            //    throw new HttpRequestException($"{response.StatusCode}:{content}");
            //}
            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    result = JsonConvert.DeserializeObject<TResponse>(x.Result);
                }, cancellationToken);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }
            return result;
        }

        /// <summary>
        /// For deleting an existing item over a web api using DELETE
        /// </summary>
        /// <param name="apiUrl">Added to the base address to make the full url of the 
        ///     api delete method, e.g. "products/3" to delete product with id of 3</param>
        /// <param name="cancellationToken"></param>
        public async Task<TResponse> DeleteRequest<TResponse>(string apiUrl, CancellationToken cancellationToken)
        {
            TResponse result = default(TResponse);
            var response = await Client.DeleteAsync(apiUrl, cancellationToken).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    result = JsonConvert.DeserializeObject<TResponse>(x.Result);
                }, cancellationToken);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }
            return result;
        }

        public async Task<TResponse> PostRequest<TResponse>(string apiUrl, string id, CancellationToken token = default)
        {
            TResponse result = default(TResponse);
            var httpContent = new StringContent(id, Encoding.UTF8, "application/json");
            if (Client.DefaultRequestHeaders.Authorization == null)
                Client.DefaultRequestHeaders.Add("authorization", SecretKey);
            var response = await Client.PostAsync(apiUrl, httpContent, token).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    result = JsonConvert.DeserializeObject<TResponse>(x.Result);
                }, token);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }
            return result;
        }

        public async Task<TResponse> PostRequest<TResponse>(string apiUrl, CancellationToken token = default)
        {
            TResponse result = default(TResponse);
            if (Client.DefaultRequestHeaders.Authorization == null)
                Client.DefaultRequestHeaders.Add("authorization", SecretKey);
            var response = await Client.PostAsync(apiUrl, null, token).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    result = JsonConvert.DeserializeObject<TResponse>(x.Result);
                }, token);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }
            return result;
        }
    }
}
