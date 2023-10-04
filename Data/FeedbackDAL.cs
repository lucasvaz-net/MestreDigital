using MestreDigital.Model;
using Microsoft.Data.SqlClient;
using System;

namespace MestreDigital.Data
{
    public class FeedbackDAL
    {
        private readonly DatabaseConnectionService _connectionService;

        public FeedbackDAL(DatabaseConnectionService connectionService)
        {
            _connectionService = connectionService;
        }

        public bool InsertFeedback(Feedback feedback)
        {
            try
            {
                using (var connection = _connectionService.CreateConnection())
                {
                    connection.Open();

                    using (var command = new SqlCommand("sp_InserirFeedback", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                     
                        command.Parameters.AddWithValue("@ConteudoID", feedback.ConteudoID);
                        command.Parameters.AddWithValue("@Comentario", string.IsNullOrEmpty(feedback.Comentario) ? (object)DBNull.Value : feedback.Comentario); // Se for nulo, insere DBNull
                        command.Parameters.AddWithValue("@Avaliacao", feedback.Avaliacao);

                        int result = command.ExecuteNonQuery();

                        return result > 0; 
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
    }
}
