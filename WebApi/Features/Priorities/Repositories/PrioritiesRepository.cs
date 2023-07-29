using DapperSamples.Common.Pagination;
using ProjectManagmentAPI.Database.Repository;
using ProjectManagmentAPI.Features.Priorities.Dtos;

namespace ProjectManagmentAPI.Features.Priorities.Repositories
{
    public sealed class PrioritiesRepository : IPrioritiesRepository
    {
        private readonly IRepository<PriorityDto> _repository;

        public PrioritiesRepository(
            IRepository<PriorityDto> repository
            )
        {
            _repository = repository;
        }

        public async Task<PagedResult<PriorityDto>> GetAllPaged(string? keyword, int offset = 0, int limit = 10, string? order = "id;desc")
        {
            var table = "[ProjectManagmentDb].[dbo].[TaskPriorities]";
            var columns = new List<string> {
                "[Id]",
                "[Name]",
                "[Description]"
            };
            var parameters = new
            {
                keyword = keyword,
                offset = offset,
                limit = limit,
                order = order
            };

            var whereStatement = "(@keyword is null or (Name like @keyword + '%' or Description like @keyword + '%'))";

            var result = await _repository.GetAllPaged(table, columns, whereStatement, order, parameters);

            return result;
        }

        public async Task<PriorityDto?> Get(long id)
        {
            return await _repository.GetById("[ProjectManagmentDb].[dbo].[TaskPriorities]", new List<string>
            {
                "[Id]",
                "[Name]",
                "[Description]"
            }, id);
        }

        public async Task<long> Create(CreatePriorityDto createPriorityDto)
        {
            var table = "[dbo].[TaskPriorities]";

            var columns = new List<string>
            {
                "[Name]",
                "[Description]"
            };
            var values = new List<string>
            {
                "@Name",
                "@Description"
            };

            return await _repository.CreateAndGetId(table, columns, values, createPriorityDto);
           
        }

        public async Task Update(UpdatePriorityDto updatePriorityDto)
        {

            var setStatements = new List<string>();

            if (!string.IsNullOrEmpty(updatePriorityDto.Name))
                setStatements.Add("[Name] = @Name");

            if (!string.IsNullOrEmpty(updatePriorityDto.Description))
                setStatements.Add("[Description] = @Description");

            var requiresUpdate = setStatements.Count > 0;

            if(requiresUpdate)
                await _repository.Update("[dbo].[TaskPriorities]", setStatements, updatePriorityDto);
        }
    }
}
