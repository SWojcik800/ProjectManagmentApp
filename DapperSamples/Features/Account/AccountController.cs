using Dapper;
using DapperSamples.Authorization;
using DapperSamples.Authorization.Jwt;
using DapperSamples.Authorization.Models;
using DapperSamples.Database;
using DapperSamples.Features.Account.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DapperSamples.Features.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IDbConnectionFactory _connectionFactory;


        public AccountController(
            IAuthorizationService authorizationService,
            ITokenGenerator tokenGenerator,
            IDbConnectionFactory connectionFactory
            )
        {
            _authorizationService = authorizationService;
            _tokenGenerator = tokenGenerator;
            _connectionFactory = connectionFactory;
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(AuthorizeDto authorizeDto)
        {
            var result = await _authorizationService.SignIn(authorizeDto.UserName, authorizeDto.Password);

            if(result.Succeeded)
            {
                var connection = _connectionFactory.Create();

                var tokenDataResult = await connection.QueryAsync<UserTokenData, Role, UserTokenData>(
                    @"
                    SELECT u.Id, u.UserName, r.Id AS RoleId, r.Name AS RoleName FROM Users u
                        LEFT JOIN UserRoles ur ON u.Id = ur.UserId
                        LEFT JOIN Roles r ON ur.RoleId = r.Id
                        WHERE u.UserName = @userName
                    ",                    
                    (tokenData, role) =>
                    {
                        if(role is not null)
                            tokenData.Roles.Add(role);
                        return tokenData;
                    },
                    splitOn: "RoleId",
                    param: new
                        {
                            userName = authorizeDto.UserName
                        }
                    );

                var tokenData = tokenDataResult.First();
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
