using Mango.Web.Models;

namespace Mango.Web.Service.Iservice
{
    public interface IAuthService
    {
        Task<ResponseDto?> LoginAsync(LoginRequestDto requestDto);
        Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto);
        Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto registrationRequestDto);


    }
}
