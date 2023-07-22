using DapperSamples.Common.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagmentAPI.Features.Statuses.Dtos;
using ProjectManagmentAPI.Features.Statuses.Repositories;

namespace ProjectManagmentAPI.Features.Statuses
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class StatusesController : ControllerBase
    {
        private readonly IStatusesRepository _statusesRepository;

        public StatusesController(IStatusesRepository statusesRepository)
        {
            _statusesRepository = statusesRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(string? keyword, int offset = 0, int limit = 10, string? order = "id;desc")
            => Ok(await _statusesRepository.GetPagedStatuses(keyword, offset, limit, order));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var result = await _statusesRepository.Get(id);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateStatusDto createStatusDto)
            => Ok(await _statusesRepository.Create(createStatusDto));

        [HttpPatch]
        public async Task<IActionResult> Update(UpdateStatusDto updateStatusDto)
        {
            await _statusesRepository.Update(updateStatusDto);
            return Ok();
        }
    }
}
