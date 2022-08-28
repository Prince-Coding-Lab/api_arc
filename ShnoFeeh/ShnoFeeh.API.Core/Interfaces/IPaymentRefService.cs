using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.API.Core.Entities;
using System.Threading.Tasks;

namespace ShnoFeeh.API.Core.Interfaces
{
    public interface IPaymentRefService
    {
        Task<DatabaseResponse> AddAsync(PaymentReference reference);
        Task<DatabaseResponse> GetAllAsync();
        Task<DatabaseResponse> GetByIdAsync(int id);
    }
}
