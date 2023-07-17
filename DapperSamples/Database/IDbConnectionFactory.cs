using System.Data.SqlClient;

namespace DapperSamples.Database
{
    public interface IDbConnectionFactory
    {
        SqlConnection Create();
    }
}