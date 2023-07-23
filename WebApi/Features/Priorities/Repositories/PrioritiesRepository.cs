using Dapper;
using DapperSamples.Common.Pagination;
using DapperSamples.Database;
using ProjectManagmentAPI.Features.Priorities.Dtos;
using System.Text;

namespace ProjectManagmentAPI.Features.Priorities.Repositories
{
    public sealed class PrioritiesRepository : IPrioritiesRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public PrioritiesRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<PagedResult<PriorityDto>> GetAllPaged(string? keyword, int offset = 0, int limit = 10, string? order = "id;desc")
        {
            var connection = _connectionFactory.Create();

            var sql = @"
                SELECT 
                    [Id],
                    [Name],
                    [Description]
              FROM [ProjectManagmentDb].[dbo].[TaskPriorities]
              WHERE (@keyword is null or 
	            (Name like @keyword + '%' or Description like @keyword + '%')
	            )
              ORDER BY
		            case when @order = 'id;desc' then Id end desc,
		            case when @order = 'id;asc' then Id end asc,
		            case when @order = 'name;asc' then Name end asc,
		            case when @order = 'name;desc' then Name end desc,
		            case when @order = 'description;asc' then Description end asc,
		            case when @order = 'description;desc' then Description end desc
	            OFFSET @offset ROWS
	            FETCH NEXT @limit ROWS ONLY;

	        SELECT COUNT([Id])
              FROM [ProjectManagmentDb].[dbo].[TaskPriorities]
                WHERE (@keyword is not null or 
	            (Name like @keyword + '%' or Description like @keyword + '%')
	            );
            ";

            var result = await connection.QueryMultipleAsync(sql, new
            {
                keyword = keyword,
                offset = offset,
                limit = limit,
                order = order
            });

            var dtos = await result.ReadAsync<PriorityDto>();
            var totalCount = await result.ReadSingleAsync<int>();

            return new PagedResult<PriorityDto>(totalCount, dtos);
        }

        public async Task<PriorityDto?> Get(long id)
        {
            var connection = _connectionFactory.Create();

            var sql = @"
            SELECT TOP 1
                [Id],
                [Name],
                [Description]
            FROM [ProjectManagmentDb].[dbo].[TaskPriorities]
            WHERE [Id] = @id";

            var statusDto = await connection.QuerySingleOrDefaultAsync<PriorityDto>(sql, new { id });

            return statusDto;
        }

        public async Task<long> Create(CreatePriorityDto createPriorityDto)
        {
            var connection = _connectionFactory.Create();

            var sql = @"
            INSERT INTO [dbo].[TaskPriorities]
                   ([Name]
                   ,[Description])
             VALUES
                   (
		           @Name,
		           @Description
		           );

            select SCOPE_IDENTITY();
            ";

            var createdStatusId = await connection.QuerySingleAsync<long>(sql, createPriorityDto);
            return createdStatusId;
        }

        public async Task Update(UpdatePriorityDto updatePriorityDto)
        {
            var connection = _connectionFactory.Create();

            var requiresUpdate = !string.IsNullOrEmpty(updatePriorityDto.Name) || !string.IsNullOrEmpty(updatePriorityDto.Description);

            if (!requiresUpdate)
                return;

            var builder = new StringBuilder("UPDATE [dbo].[TaskPriorities] SET");

            var setStatements = new List<string>();

            if (!string.IsNullOrEmpty(updatePriorityDto.Name))
                setStatements.Add("[Name] = @Name");

            if (!string.IsNullOrEmpty(updatePriorityDto.Description))
                setStatements.Add("[Description] = @Description");

            builder.AppendJoin(",", setStatements);
            builder.AppendLine(" WHERE [Id] = @Id");

            var sql = builder.ToString();
            await connection.ExecuteAsync(sql, updatePriorityDto);
        }
    }
}
