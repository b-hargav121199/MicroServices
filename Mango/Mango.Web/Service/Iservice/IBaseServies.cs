using Mango.Web.Models;

namespace Mango.Web.Service.Iservice
{
    public interface IBaseServies
    {
        Task<ResponseDto> SendAsync(RequestDto requestDto, bool withBearer=true);
    }
}
