using Dapper;
using DapperSamples.Common.Pagination;

namespace ProjectManagmentAPI.Database.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<long> CreateAndGetId(string table, ICollection<string> columns, ICollection<string> values, object parameters);
        Task<PagedResult<TEntity>> GetAllPaged(string table, ICollection<string> columns, string whereStatement, string? order, object parameters);
        Task<TEntity?> GetById(string table, ICollection<string> columns, long id);
        Task Update(string table, ICollection<string> setStatements, object parameters);
    }
}