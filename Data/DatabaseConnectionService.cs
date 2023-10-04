using Microsoft.Data.SqlClient;
    namespace MestreDigital.Data
    {
        public class DatabaseConnectionService
        {
            private readonly IConfiguration _configuration;

            public DatabaseConnectionService(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public SqlConnection CreateConnection()
            {
                var connectionString = _configuration.GetConnectionString("MestreDigitalDb");
                return new SqlConnection(connectionString);
            }
        }
    }
