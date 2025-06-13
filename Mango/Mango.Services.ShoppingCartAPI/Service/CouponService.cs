using Mango.Services.ShoppingCartAPI.Models.Dto;
using Mango.Services.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mango.Services.ShoppingCartAPI.Service
{
    public class CouponService : ICouponService
    {
        private IHttpClientFactory _httpClientFactory;
        public CouponService(IHttpClientFactory httpClientFactory)
        {
                _httpClientFactory = httpClientFactory;
        }
        public async Task<CouponDto> GetCoupon(string couponcode)
        {
            var client = _httpClientFactory.CreateClient("Coupon");
            var response = await client.GetAsync($"/api/Coupon/GetByCode/{couponcode}");
            var apiContet = await response.Content.ReadAsStringAsync();
            var reps = JsonConvert.DeserializeObject<ResponseDto>(apiContet);
            if (reps.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(reps.Result));
            }
            return new CouponDto();
        }
    }
}
