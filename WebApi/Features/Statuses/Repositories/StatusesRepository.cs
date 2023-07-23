using Dapper;
using DapperSamples.Common.Pagination;
using DapperSamples.Database;
using ProjectManagmentAPI.Features.Statuses.Dtos;
using System.Text;

namespace ProjectManagmentAPI.Features.Statuses.Repositories
{
    public sealed class StatusesRepository : IStatusesRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public StatusesRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<PagedResult<StatusDto>> GetAllPaged(string? keyword, int offset = 0, int limit = 10, string? order = "id;desc")
        {
            var connection = _connectionFactory.Create();

            var sql = @"
                SELECT 
                    [Id],
                    [Name],
                    [Description]
              FROM [ProjectManagmentDb].[dbo].[TaskStatuses]
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
              FROM [ProjectManagmentDb].[dbo].[TaskStatuses]
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

            var dtos = await result.ReadAsync<StatusDto>();
            var totalCount = await result.ReadSingleAsync<int>();

            return new PagedResult<StatusDto>(totalCount, dtos);
        }

        public async Task<StatusDto?> Get(long id)
        {
            var connection = _connectionFactory.Create();

            var sql = @"
            SELECT TOP 1
                [Id],
                [Name],
                [Description]
            FROM [ProjectManagmentDb].[dbo].[TaskStatuses]
            WHERE [Id] = @id";

            var statusDto = await connection.QuerySingleOrDefaultAsync<StatusDto>(sql, new { id });

            return statusDto;
        }

        public async Task<long> Create(CreateStatusDto createStatusDto)
        {
            var connection = _connectionFactory.Create();

            var sql = @"
            INSERT INTO [dbo].[TaskStatuses]
                   ([Name]
                   ,[Description])
             VALUES
                   (
		           @Name,
		           @Description
		           );

            select SCOPE_IDENTITY();
            ";

            var createdStatusId = await connection.QuerySingleAsync<long>(sql, createStatusDto);
            return createdStatusId;
        }

        public async Task Update(UpdateStatusDto updateStatusDto)
        {
            var connection = _connectionFactory.Create();

            var requiresUpdate = !string.IsNullOrEmpty(updateStatusDto.Name) || !string.IsNullOrEmpty(updateStatusDto.Description);

            if (!requiresUpdate)
                return;

            var builder = new StringBuilder("UPDATE [dbo].[TaskStatuses] SET");

            var setStatements = new List<string>();

            if (!string.IsNullOrEmpty(updateStatusDto.Name))
                setStatements.Add("[Name] = @Name");

            if (!string.IsNullOrEmpty(updateStatusDto.Description))
                setStatements.Add("[Description] = @Description");

            builder.AppendJoin(",", setStatements);
            builder.AppendLine(" WHERE [Id] = @Id");       

            var sql = builder.ToString();
            await connection.ExecuteAsync(sql, updateStatusDto);
        }
    }
}
