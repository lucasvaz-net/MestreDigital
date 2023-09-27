using MestreDigital.Data.Data;
using MestreDigital.Model;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace MestreDigital.Data
{
    public class SubcategoriaDal
    {
        private readonly DatabaseConnectionService _connectionService;

        public SubcategoriaDal(DatabaseConnectionService connectionService)
        {
            _connectionService = connectionService;
        }

        public IEnumerable<Subcategoria> GetSubcategoriasByCategoriaId(int categoriaId)
        {
            var subcategorias = new List<Subcategoria>();

            using (var connection = _connectionService.CreateConnection())
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT SubcategoriaID, CategoriaID, Nome, Descricao FROM vw_ListaSubcategorias WHERE CategoriaID = @CategoriaID", connection))
                {
                    command.Parameters.AddWithValue("@CategoriaID", categoriaId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var subcategoria = new Subcategoria
                            {
                                SubcategoriaID = reader.GetInt32(reader.GetOrdinal("SubcategoriaID")),
                                CategoriaID = reader.GetInt32(reader.GetOrdinal("CategoriaID")),
                                Nome = reader.GetString(reader.GetOrdinal("Nome")),
                                Descricao = reader.GetString(reader.GetOrdinal("Descricao"))
                            };

                            subcategorias.Add(subcategoria);
                        }
                    }
                }
            }

            return subcategorias;
        }


        public IEnumerable<Subcategoria> GetSubcategorias()
        {
            var subcategorias = new List<Subcategoria>();

            using (var connection = _connectionService.CreateConnection())
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT SubcategoriaID, CategoriaID, Nome, Descricao FROM vw_ListaSubcategorias", connection))
                {

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var subcategoria = new Subcategoria
                            {
                                SubcategoriaID = reader.GetInt32(reader.GetOrdinal("SubcategoriaID")),
                                CategoriaID = reader.GetInt32(reader.GetOrdinal("CategoriaID")),
                                Nome = reader.GetString(reader.GetOrdinal("Nome")),
                                Descricao = reader.GetString(reader.GetOrdinal("Descricao"))
                            };

                            subcategorias.Add(subcategoria);
                        }
                    }
                }
            }

            return subcategorias;
        }
    }
}
 