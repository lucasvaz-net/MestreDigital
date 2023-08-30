using MestreDigital.Data.Data;
using MestreDigital.Model;
using Microsoft.Data.SqlClient;

namespace MestreDigital.Data
{
    public class ConteudoDAL
    {

        private readonly DatabaseConnectionService _connectionService;

        public ConteudoDAL(DatabaseConnectionService connectionService)
        {
            _connectionService = connectionService;
        }

        public IEnumerable<Conteudo> GetConteudoBySubcategoriasId(int subcategoriaId)
        {
            var conteudos = new List<Conteudo>();

            using (var connection = _connectionService.CreateConnection())
            {
                connection.Open();

                using (var command = new SqlCommand("select * from vw_ListaConteudos where Subcategoriaid = @subcategoriaId", connection))
                {
                    command.Parameters.AddWithValue("@subcategoriaId", subcategoriaId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var conteudo = new Conteudo
                            {
                                ConteudoID = reader.GetInt32(reader.GetOrdinal("ConteudoID")),
                                SubcategoriaID = reader.GetInt32(reader.GetOrdinal("SubcategoriaID")),
                                Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                                Descricao = reader.GetString(reader.GetOrdinal("Descricao")),
                                Link = reader.IsDBNull(reader.GetOrdinal("Link")) ? null : reader.GetString(reader.GetOrdinal("Link")),
                                Texto = reader.IsDBNull(reader.GetOrdinal("Texto")) ? null : reader.GetString(reader.GetOrdinal("Texto"))
                            };

                            conteudos.Add(conteudo);
                        }
                    }
                }
            }

            return conteudos;
        }


        public IEnumerable<Conteudo> GetConteudoById(int conteudoId)
        {
            var conteudos = new List<Conteudo>();

            using (var connection = _connectionService.CreateConnection())
            {
                connection.Open();

                using (var command = new SqlCommand("select * from vw_ListaConteudos where Subcategoriaid = @conteudoId", connection))
                {
                    command.Parameters.AddWithValue("@conteudoId", conteudoId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var conteudo = new Conteudo
                            {
                                ConteudoID = reader.GetInt32(reader.GetOrdinal("ConteudoID")),
                                SubcategoriaID = reader.GetInt32(reader.GetOrdinal("SubcategoriaID")),
                                Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                                Descricao = reader.GetString(reader.GetOrdinal("Descricao")),
                                Link = reader.IsDBNull(reader.GetOrdinal("Link")) ? null : reader.GetString(reader.GetOrdinal("Link")),
                                Texto = reader.IsDBNull(reader.GetOrdinal("Texto")) ? null : reader.GetString(reader.GetOrdinal("Texto"))
                            };

                            conteudos.Add(conteudo);
                        }
                    }
                }
            }

            return conteudos;
        }


    }
}
