using MestreDigital.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Telegram.Bot.Types;

namespace MestreDigital.Data
{
    public class HistoriaDAL
    {
        private readonly DatabaseConnectionService _connectionService;

        public HistoriaDAL(DatabaseConnectionService connectionService)
        {
            _connectionService = connectionService;
        }

        public void InserirHistoria(Historia historia)
        {
            using (var connection = _connectionService.CreateConnection())
            {
                connection.Open();

                using (var command = new SqlCommand("InserirHistoria", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                   
                    command.Parameters.AddWithValue("@DSHISTORIA", historia.DsHistoria);
                    command.Parameters.AddWithValue("@IDUSUARIO", historia.Usuario.UsuarioID);
                    command.Parameters.AddWithValue("@DTHISTORIA", historia.DtHistoria);
                    command.Parameters.AddWithValue("@IDSESSAO", historia.Sessao.IdSessao);
                    command.Parameters.AddWithValue("@CMMSG", historia.CmMsg);
                    command.Parameters.AddWithValue("@IDSTATUS", historia.Status.StatusID);

                    command.ExecuteNonQuery();
                }
            }
        }

            public Historia ObterHistoriaPorUsuarioEStatus(int usuarioId, int statusId)
            {
                using (var connection = _connectionService.CreateConnection())
                {
                    connection.Open();

                    using (var command = new SqlCommand("SELECT TOP 1 * FROM vw_historia WHERE UsuarioID = @UsuarioID AND IDSTATUS = @IDSTATUS ORDER BY DTHISTORIA DESC", connection))
                    {
                        command.Parameters.AddWithValue("@UsuarioID", usuarioId);
                        command.Parameters.AddWithValue("@IDSTATUS", statusId);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Historia
                                {
                                    IdHistoria = reader.GetInt32(reader.GetOrdinal("IDHISTORIA")),
                                    DsHistoria = reader.GetString(reader.GetOrdinal("DSHISTORIA")),
                                    DtHistoria = reader.GetDateTime(reader.GetOrdinal("DTHISTORIA")),
                                    CmMsg = reader.GetString(reader.GetOrdinal("CMMSG")),
                                    Status = new Status
                                    {
                                        StatusID = reader.GetInt32(reader.GetOrdinal("IDSTATUS")),
                                        Descricao = reader.GetString(reader.GetOrdinal("STATUS"))
                                    },
                                     Sessao = new Sessao
                                     {
                                         IdSessao = reader.GetInt32(reader.GetOrdinal("IDSESSAO")),
                                         DsSessao = reader.GetString(reader.GetOrdinal("DSSESSAO"))
                                     }
                                };
                            }
                        }
                    }

                    return null;
            }
        }

        public async Task<string> CallTelegramService(long chatId, string cmmsg, string username)
        {
            using (var connection = _connectionService.CreateConnection())
            {
                await connection.OpenAsync();


                using (var command = new SqlCommand("SP_TELEGRAMSERVICE", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CHATID", chatId);
                    command.Parameters.AddWithValue("@CMMSG", cmmsg); 
                    command.Parameters.AddWithValue("@NOME", username); 

                    // Parâmetro de saída
                    var retornoParameter = command.Parameters.Add("@RETORNO", SqlDbType.VarChar, 4000);
                    retornoParameter.Direction = ParameterDirection.Output;

                    await command.ExecuteNonQueryAsync();

                    return retornoParameter.Value.ToString();
                }
            }
        }


    }
}
