using DapperSamples.Common.Pagination;
using DapperSamples.Features.Users.Dtos;

namespace ProjectManagmentAPI.Features.Users.Repositories
{
    public interface IUserRepository
    {
        Task<long> CreateUser(CreateUserDto createUserDto);
        Task<PagedResult<UserDto>> GetPagedUsers(string? userName, int offset, int limit, string? order);
    }
}