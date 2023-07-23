using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagmentAPI.Features.Priorities.Dtos;
using ProjectManagmentAPI.Features.Priorities.Repositories;

namespace ProjectManagmentAPI.Features.Priorities
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class PrioritiesController : ControllerBase
    {
        private readonly IPrioritiesRepository _prioritiesRepository;

        public PrioritiesController(IPrioritiesRepository prioritiesRepository)
        {
            _prioritiesRepository = prioritiesRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string? keyword, int offset = 0, int limit = 10, string? order = "id;desc")
            => Ok(await _prioritiesRepository.GetAllPaged(keyword, offset, limit, order));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var result = await _prioritiesRepository.Get(id);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePriorityDto createPriorityDto)
            => Ok(await _prioritiesRepository.Create(createPriorityDto));

        [HttpPatch]
        public async Task<IActionResult> Update(UpdatePriorityDto updatePriorityDto)
        {
            await _prioritiesRepository.Update(updatePriorityDto);
            return Ok();
        }
    }
}
