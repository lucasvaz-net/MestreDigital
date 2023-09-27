using MestreDigital.Data.Data;
using MestreDigital.Model;
using Microsoft.Data.SqlClient;
using System.Data;

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


        public Conteudo? GetConteudoById(int conteudoId)
        {
            Conteudo? conteudo = null;

            try
            {
                using (var connection = _connectionService.CreateConnection())
                {
                    connection.Open();

                    using (var command = new SqlCommand("select * from vw_ListaConteudos where conteudoid = @conteudoId", connection))
                    {
                        command.Parameters.AddWithValue("@conteudoId", conteudoId);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            { 
                                conteudo = new Conteudo
                                {
                                    ConteudoID = reader.GetInt32(reader.GetOrdinal("ConteudoID")),
                                    SubcategoriaID = reader.GetInt32(reader.GetOrdinal("SubcategoriaID")),
                                    Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                                    Descricao = reader.GetString(reader.GetOrdinal("descricaoconteudo")),
                                    Link = reader.IsDBNull(reader.GetOrdinal("Link")) ? null : reader.GetString(reader.GetOrdinal("Link")),
                                    Texto = reader.IsDBNull(reader.GetOrdinal("Texto")) ? null : reader.GetString(reader.GetOrdinal("Texto")),
                                    Subcategoria = new Subcategoria
                                    {
                                        SubcategoriaID = reader.GetInt32(reader.GetOrdinal("SubcategoriaID")),
                                        Nome = reader.GetString(reader.GetOrdinal("nomeosubcategoria")),
                                        Descricao = reader.GetString(reader.GetOrdinal("descricaosubcategoria")),
                                        CategoriaID = reader.GetInt32(reader.GetOrdinal("categoriaid")),
                                        Categoria = new Categoria
                                        {
                                            CategoriaID = reader.GetInt32(reader.GetOrdinal("categoriaid")),
                                            Nome = reader.GetString(reader.GetOrdinal("nomeocategoria")),
                                            Descricao = reader.GetString(reader.GetOrdinal("descricaocategoria"))
                                        }
                                    }
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               
            }

            return conteudo;
        }


        public IEnumerable<Conteudo> GetConteudo()
        {
            var conteudos = new List<Conteudo>();

            using (var connection = _connectionService.CreateConnection())
            {
                connection.Open();

                using (var command = new SqlCommand("select * from vw_ListaConteudos ", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var conteudo = new Conteudo
                            {
                                ConteudoID = reader.GetInt32(reader.GetOrdinal("ConteudoID")),
                                SubcategoriaID = reader.GetInt32(reader.GetOrdinal("SubcategoriaID")),
                                Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                                Descricao = reader.GetString(reader.GetOrdinal("descricaoconteudo")),
                                Link = reader.IsDBNull(reader.GetOrdinal("Link")) ? null : reader.GetString(reader.GetOrdinal("Link")),
                                Texto = reader.IsDBNull(reader.GetOrdinal("Texto")) ? null : reader.GetString(reader.GetOrdinal("Texto")),
                                Subcategoria = new Subcategoria
                                {
                                    SubcategoriaID = reader.GetInt32(reader.GetOrdinal("SubcategoriaID")),
                                    Nome = reader.GetString(reader.GetOrdinal("nomeosubcategoria")),
                                    Descricao = reader.GetString(reader.GetOrdinal("descricaosubcategoria")),
                                    CategoriaID = reader.GetInt32(reader.GetOrdinal("categoriaid")),
                                    Categoria = new Categoria
                                    {
                                        CategoriaID = reader.GetInt32(reader.GetOrdinal("categoriaid")),
                                        Nome = reader.GetString(reader.GetOrdinal("nomeocategoria")),
                                        Descricao = reader.GetString(reader.GetOrdinal("descricaocategoria"))
                                    }
                                }
                            };

                            conteudos.Add(conteudo);
                        }
                    }
                }
            }

            return conteudos;
        }


        public void UpsertConteudo(Conteudo conteudo, string operation)
        {
            using (var connection = _connectionService.CreateConnection())
            {
                SqlCommand command;

                if (operation == "U") 
                {
                    command = new SqlCommand("[dbo].[sp_Conteudo_up]", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    command.Parameters.AddWithValue("@CONTEUDOID", conteudo.ConteudoID);
                    command.Parameters.AddWithValue("@NOVO_SUBCATEGORIAID", conteudo.SubcategoriaID);
                    command.Parameters.AddWithValue("@NOVO_TITULO", conteudo.Titulo);
                    command.Parameters.AddWithValue("@NOVA_DESCRICAO", conteudo.Descricao);
                    command.Parameters.AddWithValue("@NOVO_LINK", conteudo.Link ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@NOVO_TEXTO", conteudo.Texto ?? (object)DBNull.Value);
                }
                else if (operation == "I") 
                {
                    command = new SqlCommand("[dbo].[sp_Conteudo_ins]", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    command.Parameters.AddWithValue("@SUBCATEGORIAID", conteudo.SubcategoriaID);
                    command.Parameters.AddWithValue("@TITULO", conteudo.Titulo);
                    command.Parameters.AddWithValue("@DESCRICAO", conteudo.Descricao);
                    command.Parameters.AddWithValue("@LINK", conteudo.Link ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@TEXTO", conteudo.Texto ?? (object)DBNull.Value);
                }
                else
                {
                    throw new ArgumentException("Operação inválida. Deve ser 'U' para atualização ou 'I' para inserção.", nameof(operation));
                }

                connection.Open();
                command.ExecuteNonQuery();
            }
        }




    }
}
