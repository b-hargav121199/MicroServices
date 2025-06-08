using Mango.Web.Models;
using Mango.Web.Service.Iservice;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new();
            return View(loginRequestDto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto obj)
        {

            ResponseDto responseDto = await _authService.LoginAsync(obj);
            if (responseDto != null && responseDto.IsSuccess)
            {
                LoginResponseDto loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result));
                await SignInUser(loginResponseDto);
                _tokenProvider.SetToken(loginResponseDto.Token);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = responseDto.Message;
                return View(obj);
            }

        }

        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
          {
                new SelectListItem{ Text= SD.RoleAdmin, Value=SD.RoleAdmin },
                new SelectListItem{ Text= SD.RoleCustomer, Value=SD.RoleCustomer}

          };

            ViewBag.roleList = roleList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto obj)
        {

            ResponseDto result = await _authService.RegisterAsync(obj);
            ResponseDto assignRole;
            if (result != null && result.IsSuccess)
            {
                if (string.IsNullOrEmpty(obj.Role))
                {
                    obj.Role = SD.RoleCustomer;
                }
                assignRole = await _authService.AssignRoleAsync(obj);
                if (assignRole != null)
                {
                    TempData["success"] = "Registration Successful";
                    return RedirectToAction(nameof(Login));
                }

            }
            else
            {
                TempData["error"] = result.Message;
            }
            var roleList = new List<SelectListItem>()
             {
                new SelectListItem{ Text= SD.RoleAdmin, Value=SD.RoleAdmin },
                new SelectListItem{ Text= SD.RoleCustomer, Value=SD.RoleCustomer}

              };

            ViewBag.roleList = roleList;
            return View(obj);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index","Home");
        }

        private async Task SignInUser(LoginResponseDto loginResponse)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(loginResponse.Token);
            var identitty = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identitty.AddClaim(new Claim(JwtRegisteredClaimNames.Email,jwt.Claims.FirstOrDefault(b=>b.Type== JwtRegisteredClaimNames.Email).Value ));
            identitty.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(b => b.Type == JwtRegisteredClaimNames.Sub).Value));
            identitty.AddClaim(new Claim(JwtRegisteredClaimNames.Name, jwt.Claims.FirstOrDefault(b => b.Type == JwtRegisteredClaimNames.Name).Value));
            identitty.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(b => b.Type == JwtRegisteredClaimNames.Email).Value));
            identitty.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(b => b.Type == "role").Value));
            var principal = new ClaimsPrincipal(identitty);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

    }
}
