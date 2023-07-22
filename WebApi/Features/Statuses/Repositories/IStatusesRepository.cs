using DapperSamples.Common.Pagination;
using ProjectManagmentAPI.Features.Statuses.Dtos;

namespace ProjectManagmentAPI.Features.Statuses.Repositories
{
    public interface IStatusesRepository
    {
        Task<long> Create(CreateStatusDto createStatusDto);
        Task<StatusDto?> Get(long id);
        Task<PagedResult<StatusDto>> GetPagedStatuses(string? keyword, int offset = 0, int limit = 10, string? order = "id;desc");
        Task Update(UpdateStatusDto updateStatusDto);
    }
}