using ShnoFeeh.API.Core.Dto;
using System.Threading.Tasks;

namespace ShnoFeeh.API.Core.Interfaces
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(EmailAddressDto emailDto);
    }
}
