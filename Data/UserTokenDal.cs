using System.Data;
using Microsoft.Data.SqlClient;

namespace MestreDigital.Data
{

        public class UserTokenDal
        {



            private readonly string _connectionService;

            public UserTokenDal(string connectionString)
            {
                _connectionService = connectionString;
            }




            public string CreateToken(int userId)
            {
                using (SqlConnection connection = new SqlConnection(_connectionService))
                {
                    connection.Open();

                   
                    DateTime createdAt = DateTime.Now;
                    DateTime expiresAt = createdAt.AddHours(1);

                   
                    using (SqlCommand command = new SqlCommand("sp_CreateUserToken", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@userId", userId));
                        command.Parameters.Add(new SqlParameter("@createdAt", createdAt));
                        command.Parameters.Add(new SqlParameter("@expiresAt", expiresAt));

                        var tokenOutput = new SqlParameter("@tokenValue", SqlDbType.VarChar, 255)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(tokenOutput);

                        command.ExecuteNonQuery();

                        return tokenOutput.Value?.ToString();
                    }
                }
            }




            public bool ValidateToken(string token)
            {
                using (SqlConnection connection = new SqlConnection(_connectionService))
                {
                    connection.Open();

                    
                    using (SqlCommand command = new SqlCommand("SELECT ExpiresAt FROM UserTokens WHERE TokenValue = @token", connection))
                    {
                        command.Parameters.Add(new SqlParameter("@token", token));

                        var expiresAt = command.ExecuteScalar() as DateTime?;
                        if (expiresAt != null)
                        {
                            return DateTime.Now < expiresAt; 
                        }
                    }
                }

                return false; 
            }




            public void LogoutUser(int userId)
            {
                using (SqlConnection connection = new SqlConnection(_connectionService))
                {
                    connection.Open();

                    
                    using (SqlCommand command = new SqlCommand("sp_LogoutUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@userId", userId));

                        command.Parameters.Add(new SqlParameter("@newExpirationDate", DateTime.Now));

                        command.ExecuteNonQuery();
                    }
                }
            }






        }

    }

