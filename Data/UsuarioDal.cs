using Microsoft.Data.SqlClient;
using MestreDigital.Models;
using System.Data;

namespace MestreDigital.Data
{
    public class UsuarioDal
    {
        private readonly DatabaseConnectionService _connectionService;

        public UsuarioDal(DatabaseConnectionService connectionService)
        {
            _connectionService = connectionService;
        }

        public IEnumerable<Usuario> ListarTodosUsuarios()
        {
            var usuarios = new List<Usuario>();

            using (var connection = _connectionService.CreateConnection())
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT * FROM vw_Usuarios", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var usuario = new Usuario
                            {
                                UsuarioID = reader.GetInt32(reader.GetOrdinal("UsuarioID")),
                                Nome = reader.GetString(reader.GetOrdinal("Nome")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Senha = reader.GetString(reader.GetOrdinal("Senha")),
                                TipoDeUsuario = new TipoDeUsuario
                                {
                                    Descricao = reader.GetString(reader.GetOrdinal("TipoUsuario"))
                                }
                            };

                            usuarios.Add(usuario);
                        }
                    }
                }
            }

            return usuarios;
        }

        public Usuario GetUsuarioPorID(int usuarioId)
        {
            Usuario usuario = null;

            using (var connection = _connectionService.CreateConnection())
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT * FROM vw_Usuarios WHERE UsuarioID = @usuarioId", connection))
                {
                    command.Parameters.AddWithValue("@usuarioId", usuarioId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuario = new Usuario
                            {
                                UsuarioID = reader.GetInt32(reader.GetOrdinal("UsuarioID")),
                                Nome = reader.GetString(reader.GetOrdinal("Nome")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Senha = reader.GetString(reader.GetOrdinal("Senha")),
                                TipoDeUsuario = new TipoDeUsuario
                                {
                                    Descricao = reader.GetString(reader.GetOrdinal("TipoUsuario"))
                                }
                            };
                        }
                    }
                }
            }

            return usuario;
        }

        public string GetHashedPasswordByEmail(string email)
        {
            using (var connection = _connectionService.CreateConnection())
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("dbo.sp_GetUserByEmail", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", email);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetString(reader.GetOrdinal("Senha"));
                        }
                    }
                }
            }

            return null;
        }



        public int? CheckUserCredentials(string email, string hashedPassword)
        {
            int? userId = null;

            using (var connection = _connectionService.CreateConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("dbo.sp_CheckUserCredentials", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@senha", hashedPassword);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userId = reader.GetInt32(reader.GetOrdinal("usuarioid"));
                        }
                    }
                }
            }

            return userId;
        }


        public int? GetUserIdByToken(string token)
        {
            int? userId = null;

            using (var connection = _connectionService.CreateConnection())
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("dbo.sp_GetUserByToken", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@token", token);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userId = reader.GetInt32(reader.GetOrdinal("UsuarioID"));
                        }
                    }
                }
            }

            return userId;
        }


    }
}
