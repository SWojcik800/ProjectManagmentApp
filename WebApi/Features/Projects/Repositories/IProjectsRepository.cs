using DapperSamples.Common.Pagination;
using ProjectManagmentAPI.Features.Projects.Dtos;

namespace ProjectManagmentAPI.Features.Projects.Repositories
{
    public interface IProjectsRepository
    {
        Task<long> Create(CreateProjectDto createPriorityDto);
        Task<ProjectDetailsDto?> Get(long id);
        Task<PagedResult<ProjectDto>> GetAllPaged(string? keyword, int offset = 0, int limit = 10, string? order = "id;desc");
        Task Update(UpdateProjectDto updatePriorityDto);
    }
}