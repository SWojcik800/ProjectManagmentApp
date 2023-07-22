using Dapper;
using DapperSamples.Authorization;
using DapperSamples.Authorization.Jwt;
using DapperSamples.Authorization.Models;
using DapperSamples.Database;
using DapperSamples.Features.Account.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagmentAPI.Authorization.Providers;

namespace DapperSamples.Features.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ITokenDataProvider _tokenDataProvider;

        public AccountController(
            IAuthorizationService authorizationService,
            ITokenGenerator tokenGenerator,
            IDbConnectionFactory connectionFactory,
            ITokenDataProvider tokenDataProvider
            )
        {
            _authorizationService = authorizationService;
            _tokenGenerator = tokenGenerator;
            _connectionFactory = connectionFactory;
            _tokenDataProvider = tokenDataProvider;
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(AuthorizeDto authorizeDto)
        {
            var result = await _authorizationService.SignIn(authorizeDto.UserName, authorizeDto.Password);

            if(result.Succeeded)
            {
                UserTokenData tokenData = await _tokenDataProvider.GetTokenData(authorizeDto);
                var roles = tokenData.Roles.Select(x => x.RoleName.ToString())
                    .ToList();

                var token = _tokenGenerator.GenerateJwtToken(tokenData.UserName, roles, tokenData.Id);
                return Ok(new CreatedTokenResult()
                {
                    Token = token,
                    TokenData = tokenData
                });
            }


            return Unauthorized();
        }

    }
}
