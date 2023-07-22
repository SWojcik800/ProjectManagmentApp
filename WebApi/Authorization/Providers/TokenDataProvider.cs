using Dapper;
using DapperSamples.Authorization.Models;
using DapperSamples.Database;
using DapperSamples.Features.Account.Dtos;

namespace ProjectManagmentAPI.Authorization.Providers
{
    public sealed class TokenDataProvider : ITokenDataProvider
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public TokenDataProvider(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<UserTokenData> GetTokenData(AuthorizeDto authorizeDto)
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
                    if (role is not null)
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
            return tokenData;
        }
    }
}
