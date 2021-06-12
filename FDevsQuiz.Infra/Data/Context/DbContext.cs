using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using FDevsQuiz.Domain.Interface;

namespace FDevsQuiz.Infra.Data.Context
{
    public class DbContext : IDbContext
    {
        public IDbConnection Connection { get; }

        public DbContext(IConfiguration config)
        {
            Connection = new SqlConnection(config.GetConnectionString("FDevsQuizConnection"));
            Connection.Open();
        }

        public void Dispose()
        {
            if (Connection.State != ConnectionState.Closed)
                Connection.Close();
        }
    }

}
