using Dapper;
using DapperSamples.Database;
using Microsoft.AspNetCore.Authorization;

namespace DapperSamples.Authorization
{
    public sealed class AuthorizationService : IAuthorizationService
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public AuthorizationService(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<AuthorizationResult> SignIn(string login, string password)
        {
            var connection = _connectionFactory.Create();

            var authorizationResult = await connection.QuerySingleAsync<bool>("SELECT [dbo].[fn_authorize] (@userName, @password)", new
            {
                userName = login,
                password = password
            });

            if (authorizationResult)
                return AuthorizationResult.Success();

            return AuthorizationResult.Failed();
        }
    }
}
