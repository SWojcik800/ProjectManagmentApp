using Dapper;
using DapperSamples.Common.Pagination;
using DapperSamples.Database;
using DapperSamples.Features.Users.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Text;

namespace DapperSamples.Features.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public UsersController(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        [HttpGet]
        public async Task<PagedResult<UserDto>> GetAll(string? userName, int offset = 0, int limit = 10, string? order = "id;desc")
        {
            var connection = _connectionFactory.Create();

            Dictionary<string, object> queryParameters = new Dictionary<string, object>
            {
                {"username", userName },
                { "offset", offset},
                { "limit", limit },
                { "sort", order }
            };

            var sql = @"
                SELECT 
                  [Id],
                  [UserName]
                    FROM [ProjectManagmentDb].[dbo].[Users]
                    WHERE (@username is null OR [UserName] like @username + '%')
                    ORDER BY
                        case when @sort = 'id;desc' then Id end desc,
	                    case when @sort = 'id;asc' then Id end asc,
	                    case when @sort = 'username;asc' then UserName end asc,
	                    case when @sort = 'username;desc' then UserName end desc
                    OFFSET @offset ROWS
                    FETCH NEXT @limit ROWS ONLY;

                SELECT COUNT([Id])
                    FROM [ProjectManagmentDb].[dbo].[Users]
                    WHERE (@username is null OR [UserName] like @username + '%');
            ";
            var result = await connection.QueryMultipleAsync(sql, queryParameters);

            var userDtos = await result.ReadAsync<UserDto>();
            var totalCount = await result.ReadSingleAsync<int>();

            return new PagedResult<UserDto>(totalCount, userDtos);
        }
    }
}
