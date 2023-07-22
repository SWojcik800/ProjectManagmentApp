using DapperSamples.Authorization.Jwt.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DapperSamples.Authorization.Jwt
{
    public static class RegisterJwtToken
    {
        public static void UseJwtToken(this IServiceCollection services, IConfiguration configuration)
        {
            var tokenConfiguration = configuration.GetSection("JwtToken").Get<JwtTokenConfiguration>();

            services.AddScoped<ITokenGenerator, JwtTokenGenerator>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   //ValidateIssuer = true,
                   //ValidateAudience = true,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = tokenConfiguration.Issuer,
                   ValidAudience = tokenConfiguration.Audience,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfiguration.SecretKey))
               };
           });
        }
    }
}
