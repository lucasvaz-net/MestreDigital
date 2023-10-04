using MestreDigital.Model;
using Microsoft.Data.SqlClient;

namespace MestreDigital.Data
{
    public class FAQDAL
    {
        private readonly DatabaseConnectionService _connectionService;

        public FAQDAL(DatabaseConnectionService connectionService)
        {
            _connectionService = connectionService;
        }
      
        public IEnumerable<FAQ> GetFAQ()
        {
            var faqs = new List<FAQ>();

            using (var connection = _connectionService.CreateConnection())
            {
                connection.Open();

                using (var command = new SqlCommand("select * from vw_ListaFAQs", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var faq = new FAQ
                            {
                                FAQID = reader.GetInt32(reader.GetOrdinal("FAQID")),
                                Pergunta = reader.GetString(reader.GetOrdinal("Pergunta")),
                                Resposta = reader.GetString(reader.GetOrdinal("Resposta")),
                                SubcategoriaID = reader.IsDBNull(reader.GetOrdinal("SubcategoriaID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("SubcategoriaID")),
                            };

                            faqs.Add(faq);
                        }
                    }
                }
            }

            return faqs;
        }

        public FAQ GetFAQById(int faqId)
        {
            FAQ faq = null;

            using (var connection = _connectionService.CreateConnection())
            {
                connection.Open();

                using (var command = new SqlCommand("select * from vw_ListaFAQs where FAQID = @faqId", connection))
                {
                    command.Parameters.AddWithValue("@faqId", faqId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read()) 
                        {
                            faq = new FAQ
                            {
                                FAQID = reader.GetInt32(reader.GetOrdinal("FAQID")),
                                Pergunta = reader.GetString(reader.GetOrdinal("Pergunta")),
                                Resposta = reader.GetString(reader.GetOrdinal("Resposta")),
                                SubcategoriaID = reader.IsDBNull(reader.GetOrdinal("SubcategoriaID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("SubcategoriaID")),
                            };
                        }
                    }
                }
            }

            return faq;
        }



    }
}
