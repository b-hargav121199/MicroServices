using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Mango.Services.ShoppingCartAPI.Extenstions
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddAppAuthentication(this WebApplicationBuilder builder)
        {
            var secret = builder.Configuration.GetValue<string>("ApiSettings:Secret");
            var issueer = builder.Configuration.GetValue<string>("ApiSettings:Issuer");
            var audience = builder.Configuration.GetValue<string>("ApiSettings:Audience");

            var key = Encoding.ASCII.GetBytes(secret);

            builder.Services.AddAuthentication(b =>
            {
                b.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                b.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(b => {
                b.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = issueer,
                    ValidAudience = audience,
                    ValidateAudience = true
                };
            });

            return builder;
        }
    }
}
