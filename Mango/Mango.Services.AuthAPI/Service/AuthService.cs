using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public AuthService(AppDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
        {
             _db= db;
            _usermanager= userManager;
            _roleManager= roleManager;
            _jwtTokenGenerator= jwtTokenGenerator;
        }

        public async Task<bool> AssignRole(string email, string rolename)
        {
            var user = _db.ApplicatinUsers.FirstOrDefault(b => b.Email.ToLower() == email.ToLower());
            if (user != null) 
            {
                if (!_roleManager.RoleExistsAsync(rolename).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(rolename)).GetAwaiter().GetResult();
                }
                await _usermanager.AddToRoleAsync(user, rolename);
                return true;
            }
            return false;

        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.ApplicatinUsers.FirstOrDefault(b => b.UserName.ToLower() == loginRequestDto.UserName.ToLower());
            bool Isvalid = await _usermanager.CheckPasswordAsync(user, loginRequestDto.Password);
            if (user == null || Isvalid == false)
            {
                return new LoginResponseDto() { User = null, Token = "" };
            }
            var roles = await _usermanager.GetRolesAsync(user);
            var token= _jwtTokenGenerator.GenerateToken(user,roles);
            UserDto userDto = new()
            {
                Email = user.Email,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                ID = user.Id
            };

            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                User = userDto,
                Token = token
            };

            return loginResponseDto;
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser applicationUser = new() {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail=registrationRequestDto.Email.ToUpper(),
                Name = registrationRequestDto.Name,
                PhoneNumber= registrationRequestDto.PhoneNumber
            };
            try
            {
                var result =  await _usermanager.CreateAsync(applicationUser,registrationRequestDto.Password);
                if (result.Succeeded)
                {
                    var userToReturn = _db.ApplicatinUsers.First(b => b.UserName == registrationRequestDto.Email);
                    UserDto userDto = new()
                    {
                        Email = userToReturn.Email,
                        Name = userToReturn.UserName,
                        PhoneNumber = userToReturn.PhoneNumber,
                        ID = userToReturn.Id

                    };

                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }

            }
            catch (Exception ex)
            {
                
            }
            return "Error Encountered";
        }
    }
}
