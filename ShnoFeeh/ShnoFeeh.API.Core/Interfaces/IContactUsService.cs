using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.API.Core.Entities;
using System.Threading.Tasks;

namespace ShnoFeeh.API.Core.Interfaces
{
    public interface IContactUsService
    {
        Task<DatabaseResponse> SendEmailAsync(ContactUs contactUs);
        Task<DatabaseResponse> UpdateEmailAsync(ContactUs contactUs);
        Task<DatabaseResponse> GetEmailByIdAsync(int id);
        Task<DatabaseResponse> GetEmailsAsync();
    }
}
