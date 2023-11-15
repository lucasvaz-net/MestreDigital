using Microsoft.AspNetCore.Mvc;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using MestreDigital.Data;

namespace MestreDigital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelegramController : ControllerBase
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<TelegramController> _logger;
        private readonly HistoriaDAL _historiaDAL;

        public TelegramController(
            ITelegramBotClient botClient,
            ILogger<TelegramController> logger,
            HistoriaDAL historiaDAL)
        {
            _botClient = botClient;
            _logger = logger;
            _historiaDAL = historiaDAL;
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update()
        {
            try
            {
                string body;
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    body = await reader.ReadToEndAsync();
                }

                var update = Newtonsoft.Json.JsonConvert.DeserializeObject<Update>(body);
                if (update?.Message == null)
                {
                    return BadRequest("No message in the update.");
                }

                
                string userMessage = update.Message.Text;
                string userName = update.Message.From.FirstName;

      
                var responseMessage = await _historiaDAL.CallTelegramService(
                    update.Message.Chat.Id,
                    userMessage,
                    userName
                );

                await _botClient.SendTextMessageAsync(update.Message.Chat.Id, responseMessage.Replace("/n/n", "\n"));

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
