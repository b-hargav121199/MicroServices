using Mango.Web.Models;
using Mango.Web.Service.Iservice;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseServies _baseServices;
        public CouponService(IBaseServies baseServices)
        {
            _baseServices = baseServices;
        }

        public async Task<ResponseDto?> CreateCouponsAsync(CouponDto couponDto)
        {
            return await _baseServices.SendAsync(new RequestDto()
            {

                ApiType = SD.ApiType.Post,
                Data= couponDto,
                Url = SD.CouponAPIBase + "/api/Coupon" 
            });
        }

        public async Task<ResponseDto?> DeleteCouponsAsync(int id)
        {
            return await _baseServices.SendAsync(new RequestDto()
            {

                ApiType = SD.ApiType.Delete,
                Url = SD.CouponAPIBase + "/api/Coupon/" + id
            });
        }

        public async Task<ResponseDto?> GetAllCouponAsync()
        {
            return await _baseServices.SendAsync(new RequestDto()
            {

                ApiType = SD.ApiType.Get,
                Url=SD.CouponAPIBase+"/api/Coupon"
            });

        }

        public async Task<ResponseDto?> GetCouponAsync(string couponCode)
        {
            return await _baseServices.SendAsync(new RequestDto()
            {

                ApiType = SD.ApiType.Get,
                Url = SD.CouponAPIBase + "/api/Coupon/GetByCode/" + couponCode
            });
        }

        public async Task<ResponseDto?> GetCouponByIdAsync(int id)
        {
            return await _baseServices.SendAsync(new RequestDto()
            {

                ApiType = SD.ApiType.Get,
                Url = SD.CouponAPIBase + "/api/Coupon/" + id
            });
        }

        public async Task<ResponseDto?> UpdateCouponsAsync(CouponDto couponDto)
        {
            return await _baseServices.SendAsync(new RequestDto()
            {

                ApiType = SD.ApiType.Put,
                Data = couponDto,
                Url = SD.CouponAPIBase + "/api/Coupon"
            });
        }
    }
}
