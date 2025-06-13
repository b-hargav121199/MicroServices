using Mango.Web.Models;
using Mango.Web.Service;
using Mango.Web.Service.Iservice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
                _cartService = cartService;
        }
        [Authorize]
        public async Task<IActionResult> CartIndex()
        {

            return View(await LoadCartDtoBassedOnLoggedInUser());
        }

        public async Task<IActionResult> Remove(int cartdetailsId)
        {
            var userId = User.Claims.Where(b => b.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault().Value;
            ResponseDto response = await _cartService.RemoveFromCartAsync(cartdetailsId);
            if (response != null && response.IsSuccess)
            {
                TempData["Success"] = "Cart Updated Successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            cartDto.CartHeader.CouponCode = "";
            ResponseDto response = await _cartService.ApplyCouponAsync(cartDto);
            if (response != null && response.IsSuccess)
            {
                TempData["Success"] = "Cart Updated Successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            ResponseDto response = await _cartService.ApplyCouponAsync(cartDto);
            if (response != null && response.IsSuccess)
            {
                TempData["Success"] = "Cart Updated Successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }
        private async Task<CartDto> LoadCartDtoBassedOnLoggedInUser()
        { 
            var userId = User.Claims.Where(b=>b.Type== JwtRegisteredClaimNames.Sub)?.FirstOrDefault().Value;
            ResponseDto response = await _cartService.GetCartByUserIdAsync(userId);
            if (response != null && response.IsSuccess)
            {
                CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
                return cartDto;
            }
            return new CartDto();
        }

    }
}
