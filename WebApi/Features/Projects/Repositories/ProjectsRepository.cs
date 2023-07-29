using Dapper;
using DapperSamples.Common.Pagination;
using DapperSamples.Database;
using ProjectManagmentAPI.Database.Repository;
using ProjectManagmentAPI.Features.Projects.Dtos;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ProjectManagmentAPI.Features.Projects.Repositories
{
    public class ProjectsRepository : IProjectsRepository
    {
        private readonly IRepository<ProjectDto> _repository;
        private readonly IDbConnectionFactory _connectionFactory;

        public ProjectsRepository(
            IRepository<ProjectDto> repository,
            IDbConnectionFactory connectionFactory)
        {
            _repository = repository;
            _connectionFactory = connectionFactory;
        }

        public async Task<PagedResult<ProjectDto>> GetAllPaged(string? keyword, int offset = 0, int limit = 10, string? order = "id;desc")
        {
            var table = "[dbo].[Projects]";
            var columns = new List<string> { "[Id]","[Name]", "[Description]" };
            var whereStatement = "(@keyword IS NULL or ([Name] like '%' + @keyword or [Description] like '%' + @keyword))";

            return await _repository.GetAllPaged(table, columns, whereStatement, order, new
            {
                keyword = keyword,
                offset = offset,
                limit = limit,
                order = order
            });
        }

        public async Task<ProjectDetailsDto?> Get(long id)
        {
            var connection = _connectionFactory.Create();
            var sql = @"
                    SELECT 
	                    p.Id ,p.Name, p.Description, u.Id as UserId, u.UserName
	                    FROM Projects p
                    LEFT JOIN UserProjects up ON up.ProjectId = p.Id
                    LEFT JOIN Users u ON up.UserId = u.Id
                    WHERE p.Id = @id";

            var dtos = await connection.QueryAsync<ProjectDetailsDto, ProjectDetailsUserDto, ProjectDetailsDto>(sql, (project, user) =>
            {
                if(user is not null)
                    project.Users.Add(user);
                return project;
            }, splitOn: "UserId", param: new {id} );

            return dtos.FirstOrDefault();
        }

        public async Task<long> Create(CreateProjectDto createProjectDto)
        {
            var table = "[dbo].[Projects]";

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

            return await _repository.CreateAndGetId(table, columns, values, createProjectDto);

        }

        public async Task Update(UpdateProjectDto updateProjectDto)
        {

            var setStatements = new List<string>();

            if (!string.IsNullOrEmpty(updateProjectDto.Name))
                setStatements.Add("[Name] = @Name");

            if (!string.IsNullOrEmpty(updateProjectDto.Description))
                setStatements.Add("[Description] = @Description");

            var requiresUpdate = setStatements.Any() 
                ||
                (
                updateProjectDto.UserIds is not null 
                &&
                updateProjectDto.UserIds.Any()
                );

            if (requiresUpdate)
            {
                using (var connection = GetOpenConnection(_connectionFactory))                
                using (var transaction = connection.BeginTransaction())
                {

                    if(setStatements.Any())
                    {
                        var updateProjectSql = new StringBuilder("UPDATE [dbo].[Projects] SET ");
                        updateProjectSql.AppendJoin(',', setStatements);
                        updateProjectSql.AppendLine(" WHERE [Id] = @Id");

                        await connection.ExecuteAsync(updateProjectSql.ToString(), updateProjectDto, transaction);
                    }

                    if (updateProjectDto.UserIds is not null && updateProjectDto.UserIds.Any())
                    {
                        var deleteOldUserProjectsSql = @"
                            DELETE FROM UserProjects 
	                        WHERE ProjectId = @Id 
                            AND UserId NOT IN @UserIds
                        ";

                        await connection.ExecuteAsync(deleteOldUserProjectsSql, updateProjectDto, transaction);

                        var insertNewUserProjectsSql = @"
                                                  
                            INSERT INTO [dbo].[UserProjects]
                               ([UserId]
                               ,[ProjectId])
                            SELECT u.Id, @Id FROM Users u
                            WHERE u.Id in @UserIds AND NOT EXISTS (
                                SELECT Id FROM [dbo].[UserProjects] up
                                    WHERE up.UserId = u.Id AND up.ProjectId = @Id
                            );
                        ";

                        await connection.ExecuteAsync(insertNewUserProjectsSql, updateProjectDto, transaction);

                    }


                    transaction.Commit();
                }
                                                
            }
        }

        private static SqlConnection GetOpenConnection(IDbConnectionFactory connectionFactory)
        {
            var connection = connectionFactory.Create();
            if (connection.State is not System.Data.ConnectionState.Open)
                connection.Open();

            return connection;
        }

    }
}
