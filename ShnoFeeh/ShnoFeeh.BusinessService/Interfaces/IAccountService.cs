using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.BusinessService.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShnoFeeh.BusinessService.Interfaces
{
    public interface IAccountService
    {
        Task<UserDto> LoginAsync(AuthenticateModel requestDto);
        Task<ResponseDto<UserDto>> RegisterAsync(UserCreateDto model);
        Task<ResponseDto<UserDto>> UpdateAsync(UserUpdateDto model);        
        Task<ResponseDto<UserPasswordDto>> ChangePasswordAsync(UserPasswordDto model);
        Task<ResponseDto<UserPasswordDto>> ResetAsync(UserPasswordDto model);
        Task<ResponseDto<ResetPasswordDto>> ResetPasswordAsync(ResetPasswordDto model);
        Task<ResponseDto<object>> VerifyEmailToken(string token);
        Task<ResponseDto<UserEmailDto>> ForgotPasswordAsync(UserEmailDto model);
        Task<ResponseDto<VerificationTokenDto>> ActivateEmail(VerificationTokenDto model);
        Task<UploadImageResponse> UploadProfileImage(Image model);

        //Users
        Task<ResponseDto<List<UserDto>>> GetAllUsersAsync(string token);
        Task<ResponseDto<string>> DeleteUserAsync(string token, int userId);
        Task<ResponseDto<UserDto>> UpdateUserStatusAsync(UserDto model, string token);
    }
}
