using MestreDigital.Data.Data;
using MestreDigital.Model;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;

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

        public void UpsertCategoria(int? categoriaId, string nome, string descricao, string actionType)
        {
            using (var connection = _connectionService.CreateConnection())
            {
                connection.Open();

                // Definindo a stored procedure correta com base na ação
                string storedProcedureName = actionType.ToLower() == "u" ? "sp_Categoria_up" : "sp_Categoria_ins";

                using (var command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (actionType.ToLower() == "u")
                    {
                        command.Parameters.AddWithValue("@CATEGORIAID", categoriaId);
                        command.Parameters.AddWithValue("@NOVO_NOME", nome);
                        command.Parameters.AddWithValue("@NOVA_DESCRICAO", descricao);
                    }
                    else if (actionType.ToLower() == "i")
                    {
                        command.Parameters.AddWithValue("@NOME", nome);
                        command.Parameters.AddWithValue("@DESCRICAO", descricao);
                    }

                    command.ExecuteNonQuery();
                }
            }
        }





    }
}

