using Mango.Web.Models;

namespace Mango.Web.Service.Iservice
{
    public interface ICartService
    {
        Task<ResponseDto?> GetCartByUserIdAsync(string userid);
        Task<ResponseDto?> UpsertCartAsync(CartDto cartDto);
        Task<ResponseDto?> RemoveFromCartAsync(int CartDetailsId);
        Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto);

    }
}
