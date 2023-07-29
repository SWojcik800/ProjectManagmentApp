using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagmentAPI.Features.Projects.Dtos;
using ProjectManagmentAPI.Features.Projects.Repositories;

namespace ProjectManagmentAPI.Features.Projects
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectsRepository _projectsRepository;

        public ProjectsController(IProjectsRepository projectsRepository)
        {
            _projectsRepository = projectsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string? keyword, int offset = 0, int limit = 10, string? order = "id;desc")
            => Ok(await _projectsRepository.GetAllPaged(keyword, offset, limit, order));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var result = await _projectsRepository.Get(id);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectDto createProjetcDto)
            => Ok(await _projectsRepository.Create(createProjetcDto));

        [HttpPatch]
        public async Task<IActionResult> Update(UpdateProjectDto updateProjectDto)
        {
            await _projectsRepository.Update(updateProjectDto);
            return Ok();
        }
    }
}
