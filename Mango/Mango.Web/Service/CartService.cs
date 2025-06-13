using Mango.Web.Models;
using Mango.Web.Service.Iservice;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class CartService: ICartService
    {
        private readonly IBaseServies _baseServices;
        public CartService(IBaseServies baseServices)
        {
            _baseServices = baseServices;
        }

        public async Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto)
        {
            return await _baseServices.SendAsync(new RequestDto()
            {
                ApiType= SD.ApiType.Post,
                Data = cartDto,
                Url= SD.ShoppingCartAPIBase + "/api/Cart/ApplyCoupon"
            });
        }

        public async Task<ResponseDto?> GetCartByUserIdAsync(string userid)
        {
            return await _baseServices.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.Get,
                Url = SD.ShoppingCartAPIBase + "/api/Cart/GetCart/"+userid
            });
        }

        public async Task<ResponseDto?> RemoveFromCartAsync(int CartDetailsId)
        {
            return await _baseServices.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.Post,
                Data=CartDetailsId,
                Url = SD.ShoppingCartAPIBase + "/api/Cart/RemoveCart"
            });
        }

        public async Task<ResponseDto?> UpsertCartAsync(CartDto cartDto)
        {
            return await _baseServices.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.Post,
                Data = cartDto,
                Url = SD.ShoppingCartAPIBase + "/api/Cart/CartUpsert"
            });
        }
    }
}
