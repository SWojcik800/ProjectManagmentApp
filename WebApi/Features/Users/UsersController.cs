using Dapper;
using DapperSamples.Common.Pagination;
using DapperSamples.Database;
using DapperSamples.Features.Users.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagmentAPI.Features.Users.Repositories;

namespace DapperSamples.Features.Users
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IUserRepository _userRepository;

        public UsersController(
            IDbConnectionFactory connectionFactory,
            IUserRepository userRepository
            )
        {
            _connectionFactory = connectionFactory;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<PagedResult<UserDto>> GetAll(string? userName, int offset = 0, int limit = 10, string? order = "id;desc")
            => await _userRepository.GetPagedUsers(userName, offset, limit, order);

        [HttpPost]
        public async Task<long> Create(CreateUserDto createUserDto)
            => await _userRepository.CreateUser(createUserDto);
    }
}
