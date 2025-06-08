using Mango.Web.Models;
using Mango.Web.Service.Iservice;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class ProductService : IProductService
    {
        private readonly IBaseServies _baseServices;
        public ProductService(IBaseServies baseServices)
        {
            _baseServices = baseServices;
        }
        public async Task<ResponseDto?> CreateProductAsync(ProductDto productDto)
        {
            return await _baseServices.SendAsync(new RequestDto()
            {

                ApiType = SD.ApiType.Post,
                Data = productDto,
                Url = SD.ProductAPIBase + "/api/Product"
            });
        }

        public async Task<ResponseDto?> DeleteProductAsync(int id)
        {
            return await _baseServices.SendAsync(new RequestDto()
            {

                ApiType = SD.ApiType.Delete,
                Url = SD.ProductAPIBase + "/api/Product/" + id
            });
        }

        public async Task<ResponseDto?> GetAllProductsAsync()
        {
            return await _baseServices.SendAsync(new RequestDto()
            {

                ApiType = SD.ApiType.Get,
                Url = SD.ProductAPIBase + "/api/Product"
            });

        }

        public async Task<ResponseDto?> GetProductByIdAsync(int id)
        {
            return await _baseServices.SendAsync(new RequestDto()
            {

                ApiType = SD.ApiType.Get,
                Url = SD.ProductAPIBase + "/api/Product/" + id
            });
        }

        public async Task<ResponseDto?> UpdateProductAsync(ProductDto productDto)
        {
            return await _baseServices.SendAsync(new RequestDto()
            {

                ApiType = SD.ApiType.Put,
                Data = productDto,
                Url = SD.ProductAPIBase + "/api/Product"
            });
        }
    }
}
