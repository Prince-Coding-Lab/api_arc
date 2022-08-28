using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.API.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShnoFeeh.API.Core.Interfaces
{
    public interface IOrderService
    {
        Task<DatabaseResponse> AddOrderAsync(Orders order);
        Task<DatabaseResponse> UpdateOrderAsync(Orders order);
        Task<DatabaseResponse> GetOrdersAsync();
    }
}
