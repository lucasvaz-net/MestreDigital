using MestreDigital.Data.Data;
using MestreDigital.Model;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace MestreDigital.DAL
    {
        public class CategoriaDAL
        {
            private readonly DatabaseConnectionService _connectionService;

            public CategoriaDAL(DatabaseConnectionService connectionService)
            {
                _connectionService = connectionService;
            }

            public IEnumerable<Categoria> GetCategorias()
            {
                var categorias = new List<Categoria>();

                using (var connection = _connectionService.CreateConnection())
                {
                    connection.Open();

                    using (var command = new SqlCommand("SELECT CategoriaID, Nome, Descricao FROM vw_ListaCategorias", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var categoria = new Categoria
                                {
                                    CategoriaID = reader.GetInt32(reader.GetOrdinal("CategoriaID")),
                                    Nome = reader.GetString(reader.GetOrdinal("Nome")),
                                    Descricao = reader.GetString(reader.GetOrdinal("Descricao"))
                                };

                                categorias.Add(categoria);
                            }
                        }
                    }
                }

                return categorias;
            }
    }
    }

