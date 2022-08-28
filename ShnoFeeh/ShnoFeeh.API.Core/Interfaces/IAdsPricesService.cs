using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.API.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShnoFeeh.API.Core.Interfaces
{
    public interface IAdsPricesService
    {
        Task<DatabaseResponse> UpdateAdsPriceAsync(AdsPrices ads);
        Task<DatabaseResponse> GetAdsPricesAsync();
        Task<DatabaseResponse> GetPriceByIdAsync(int adPriceId);
    }
}
