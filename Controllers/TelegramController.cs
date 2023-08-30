using MestreDigital.Model;
using MestreDigital.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MestreDigital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelegramController : ControllerBase
    {
        private readonly ITelegramBotClient _botClient;
        private readonly UserStateService _userStateService;
        private readonly TelegramService _telegramService;
        private readonly ILogger<TelegramController> _logger;

        public TelegramController(
            ITelegramBotClient botClient,
            UserStateService userStateService,
            TelegramService telegramService,
            ILogger<TelegramController> logger)
        {
            _botClient = botClient;
            _userStateService = userStateService;
            _telegramService = telegramService;
            _logger = logger;
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
                    _logger.LogWarning("Update doesn't contain a message.");
                    return BadRequest("No message in the update.");
                }

                var currentState = _userStateService.GetState(update.Message.Chat.Id) ?? new UserState
                {
                    ChatId = update.Message.Chat.Id,
                    CurrentStage = ConversationStage.MainMenu
                };

                string responseMessage = await _telegramService.ProcessUpdateAsync(update, currentState);
                await _botClient.SendTextMessageAsync(update.Message.Chat.Id, responseMessage);

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
