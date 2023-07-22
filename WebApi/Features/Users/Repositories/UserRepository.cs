using Dapper;
using DapperSamples.Common.Pagination;
using DapperSamples.Database;
using DapperSamples.Features.Users.Dtos;

namespace ProjectManagmentAPI.Features.Users.Repositories
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public UserRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task<PagedResult<UserDto>> GetPagedUsers(string? userName, int offset, int limit, string? order)
        {
            var connection = _connectionFactory.Create();

            Dictionary<string, object> queryParameters = new Dictionary<string, object>
            {
                {"username", userName },
                { "offset", offset},
                { "limit", limit },
                { "order", order }
            };

            var sql = @"
                SELECT 
                  [Id],
                  [UserName]
                    FROM [ProjectManagmentDb].[dbo].[Users]
                    WHERE (@username is null OR [UserName] like @username + '%')
                    ORDER BY
                        case when @order = 'id;desc' then Id end desc,
	                    case when @order = 'id;asc' then Id end asc,
	                    case when @order = 'username;asc' then UserName end asc,
	                    case when @order = 'username;desc' then UserName end desc
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

        public async Task<long> CreateUser(CreateUserDto createUserDto)
        {
            var connection = _connectionFactory.Create();

            var createdUserId = await connection.QuerySingleAsync<long>("sp_createUser", new
            {
                userName = createUserDto.UserName,
                password = createUserDto.Password
            }, commandType: System.Data.CommandType.StoredProcedure);

            return createdUserId;
        }
    }
}
