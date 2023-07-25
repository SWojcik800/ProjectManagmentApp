using Dapper;
using DapperSamples.Common.Pagination;
using DapperSamples.Database;
using System.Text;

namespace ProjectManagmentAPI.Database.Repository
{
    public sealed class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public Repository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<PagedResult<TEntity>> GetAllPaged(string table, ICollection<string> columns, string whereStatement, string? order, object parameters)
        {
            var connection = _connectionFactory.Create();

            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("SELECT");
            stringBuilder.AppendJoin(',', columns);
            stringBuilder.AppendFormat(" FROM {0}", table);
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("WHERE {0}", whereStatement);

            if (string.IsNullOrEmpty(order))
                order = "id;desc";

            var orderList = order.Split(";");
            var orderColumn = "[" + char.ToUpper(orderList.First()[0]) + orderList[0].Substring(1) + "]";
            var orderDir = orderList[1].ToUpper();

            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("ORDER BY {0} {1}", orderColumn, orderDir);
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("OFFSET @offset ROWS");
            stringBuilder.AppendLine("FETCH NEXT @limit ROWS ONLY;");
            stringBuilder.AppendLine();

            stringBuilder.AppendLine("SELECT COUNT([Id])");
            stringBuilder.AppendFormat("FROM {0}", table);
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("WHERE {0}", whereStatement);
            stringBuilder.Append(";");

            var sql = stringBuilder.ToString();

            var result = await connection.QueryMultipleAsync(sql, parameters);

            var dtos = await result.ReadAsync<TEntity>();
            var totalCount = await result.ReadSingleAsync<int>();

            return new PagedResult<TEntity>(totalCount, dtos);
        }

        public async Task<TEntity?> GetById(string table, ICollection<string> columns, long id)
        {
            var connection = _connectionFactory.Create();

            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("SELECT TOP 1 ");
            stringBuilder.AppendJoin(',', columns);
            stringBuilder.AppendFormat("FROM {0} ", table);
            stringBuilder.AppendLine("WHERE [Id] = @id");

            var sql = stringBuilder.ToString();
            return await connection.QuerySingleOrDefaultAsync<TEntity?>(sql, new { id });
        }

        public async Task<long> CreateAndGetId(string table, ICollection<string> columns, ICollection<string> values, object parameters)
        {
            var connection = _connectionFactory.Create();

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("INSERT INTO {0} ", table);
            stringBuilder.AppendLine("(");
            stringBuilder.AppendJoin(',', columns);
            stringBuilder.AppendLine(")");
            stringBuilder.AppendLine("VALUES");
            stringBuilder.AppendLine("(");
            stringBuilder.AppendJoin(',', values);
            stringBuilder.AppendLine(");");
            stringBuilder.AppendLine("SELECT SCOPE_IDENTITY();");

            var sql = stringBuilder.ToString();

            var createdStatusId = await connection.QuerySingleAsync<long>(sql, parameters);
            return createdStatusId;
        }

        public async Task Update(string table, ICollection<string> setStatements, object parameters)
        {
            var connection = _connectionFactory.Create();
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("UPDATE {0} SET ", table);
            stringBuilder.AppendJoin(',', setStatements);

            var sql = stringBuilder.ToString();

            await connection.ExecuteAsync(sql, parameters);
        }
    }
}
