using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.API.Core.Entities;
using System.Threading.Tasks;

namespace ShnoFeeh.API.Core.Interfaces
{
    public interface IUserService
    {
        Task<DatabaseResponse> CreateUserAsync(AdminUsers user);
        Task<DatabaseResponse> UpdateUserAsync(AdminUsers user);
        Task<DatabaseResponse> GetUserByIdAsync(int userId);
        Task<DatabaseResponse> GetUsersAsync(int? RoleId);
        Task<DatabaseResponse> DeleteUserAsync(int userId);
        Task<UserDto> AuthenticateAsync(string username, string password);
        Task<DatabaseResponse> ChangePasswordAsync(UserPasswordDto user);
        //Task<DatabaseResponse> ForgotPasswordAsync();
        Task<DatabaseResponse> ResetPasswordAsync(ResetPasswordDto user);
        Task<DatabaseResponse> CheckValidUserAsync(string email);
        //Task CreateVerificationTokenAsync(VerificationTokenDto verifyRequest);

        Task<DatabaseResponse> ConfirmVerificationToken(string token);

        Task<DatabaseResponse> ActivateEmail(VerificationTokenDto model);
    }
}
