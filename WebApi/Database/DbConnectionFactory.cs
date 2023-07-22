using Microsoft.AspNetCore.Connections;
using System.Data.SqlClient;
using System.Net;

namespace DapperSamples.Database
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public DbConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SqlConnection Create()
            => new SqlConnection(_configuration.GetConnectionString("Default"));
        
    }
}
