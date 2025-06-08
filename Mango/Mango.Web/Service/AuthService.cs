using Mango.Web.Models;
using Mango.Web.Service.Iservice;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class AuthService : IAuthService
    {
        private IBaseServies _baseServies;
        public AuthService(IBaseServies baseServies)
        {
                _baseServies = baseServies;
        }
        public async Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseServies.SendAsync(new RequestDto
            {
                ApiType= SD.ApiType.Post,
                Data = registrationRequestDto,
                Url=  SD.AuthAPIBase + "/api/auth/AssignRole"
            });
        }

        public async Task<ResponseDto?> LoginAsync(LoginRequestDto requestDto)
        {
            return await _baseServies.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.Post,
                Data = requestDto,
                Url = SD.AuthAPIBase + "/api/auth/login"
            });
        }

        public async Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseServies.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.Post,
                Data = registrationRequestDto,
                Url = SD.AuthAPIBase + "/api/auth/register"
            },withBearer: false);
        }
    }
}
