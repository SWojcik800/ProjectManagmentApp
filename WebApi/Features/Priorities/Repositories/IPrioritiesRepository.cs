using DapperSamples.Common.Pagination;
using ProjectManagmentAPI.Features.Priorities.Dtos;

namespace ProjectManagmentAPI.Features.Priorities.Repositories
{
    public interface IPrioritiesRepository
    {
        Task<long> Create(CreatePriorityDto createPriorityDto);
        Task<PriorityDto?> Get(long id);
        Task<PagedResult<PriorityDto>> GetAllPaged(string? keyword, int offset = 0, int limit = 10, string? order = "id;desc");
        Task Update(UpdatePriorityDto updatePriorityDto);
    }
}