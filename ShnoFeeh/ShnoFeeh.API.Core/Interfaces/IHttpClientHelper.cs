using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShnoFeeh.API.Core.Interfaces
{
    public interface IHttpClientHelper
    {
        Task<TResponse> GetSingleItemRequest<TResponse>(string apiUrl, CancellationToken token = default(CancellationToken));
        Task<TResponse[]> GetMultipleItemsRequest<TResponse>(string apiUrl, CancellationToken token = default(CancellationToken));
        Task<TResponse> PostRequest<TRequest, TResponse>(string apiUrl, TRequest postObject, CancellationToken token = default(CancellationToken));
        Task<TResponse> PostRequest<TResponse>(string apiUrl, string id, CancellationToken token = default(CancellationToken));
        Task<TResponse> PostRequest<TResponse>(string apiUrl, CancellationToken token = default(CancellationToken));
        Task<TResponse> PutRequest<TRequest, TResponse>(string apiUrl, TRequest putObject, CancellationToken token = default(CancellationToken));
        Task<TResponse> DeleteRequest<TResponse>(string apiUrl, CancellationToken token = default(CancellationToken));
    }
}
