using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using MestreDigital.Data.Data;
using MestreDigital.Models; 

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
    }
}
