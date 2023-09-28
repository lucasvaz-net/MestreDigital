using MestreDigital.DAL;
using MestreDigital.Data;
using MestreDigital.Model;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
namespace MestreDigital.Services
{
    public class TelegramService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly CategoriaDAL _categoriaDAL;
        private readonly SubcategoriaDal _subcategoriaDAL;
        private readonly ConteudoDAL _conteudoDAL;
        private readonly FAQDAL _faqDAL;
        private readonly ILogger<TelegramService> _logger;
        private readonly UserStateService _userStateService; 

        public TelegramService(
            ITelegramBotClient botClient,
            CategoriaDAL categoriaDAL,
            SubcategoriaDal subcategoriaDAL,
            ConteudoDAL conteudoDAL,
            FAQDAL faqDAL,
            ILogger<TelegramService> logger,
            UserStateService userStateService) 
        {
            _botClient = botClient;
            _categoriaDAL = categoriaDAL;
            _subcategoriaDAL = subcategoriaDAL;
            _conteudoDAL = conteudoDAL;
            _faqDAL = faqDAL;
            _logger = logger;
            _userStateService = userStateService; 
        }


        public async Task<string> ProcessUpdateAsync(Update update, UserState currentState)
        {
            List<string> responseMessages;

            switch (currentState.CurrentStage)
            {
                case ConversationStage.MainMenu:
                    return HandleMainMenu(update, currentState);

                case ConversationStage.CategorySelection:
                    return HandleCategorySelection(update, currentState);

                case ConversationStage.SubcategorySelection:
                    return HandleSubcategorySelection(update, currentState);

                case ConversationStage.ContentSelection:
                    return await HandleContentDetailAsync(update, currentState);

                case ConversationStage.FAQs:
                    return HandleFAQs(update, currentState);

                default:
                    currentState.CurrentStage = ConversationStage.MainMenu;
                    return "Desculpe, não entendi. Por favor, comece novamente.";
            }

            foreach (var message in responseMessages)
            {
                await _botClient.SendTextMessageAsync(update.Message.Chat.Id, message);
            }

        }


        private string HandleMainMenu(Update update, UserState currentState)
        {
            if (update.Message.Text == "0")
            {
                currentState.CurrentStage = ConversationStage.FAQs;
                return HandleFAQs(update, currentState);
            }

            var categorias = _categoriaDAL.GetCategorias();
            var message = new StringBuilder("Olá! 😊 Aqui estão as opções disponíveis:\n\n");

            message.AppendLine("0 - Perguntas frequentes");
            foreach (var categoria in categorias)
            {
                message.AppendLine($"{categoria.CategoriaID} - {categoria.Nome}");
            }

            message.AppendLine("\nPor favor, escolha a opção pelo número!");
            currentState.CurrentStage = ConversationStage.CategorySelection;
            _userStateService.SetState(update.Message.Chat.Id, currentState);

            return message.ToString();
        }

        private string HandleCategorySelection(Update update, UserState currentState)
        {
            if (!int.TryParse(update.Message.Text, out int selectedCategoryId))
            {
                return "Por favor, insira um número válido para escolher uma categoria.";
            }

            if (selectedCategoryId == 0)
            {
                currentState.CurrentStage = ConversationStage.FAQs;
                 _userStateService.SetState(update.Message.Chat.Id, currentState);
                return HandleFAQs(update, currentState);
            }

            var subcategorias = _subcategoriaDAL.GetSubcategoriasByCategoriaId(selectedCategoryId);
            var message = new StringBuilder();

            if (subcategorias != null && subcategorias.Any())
            {
                message.AppendLine("Agora selecione uma das opções abaixo:\n\n");
                foreach (var subcategoria in subcategorias)
                {
                    message.AppendLine($"{subcategoria.SubcategoriaID} - {subcategoria.Nome}");
                }

                message.AppendLine("\nPor favor, escolha a opção pelo número!");
                currentState.CurrentStage = ConversationStage.SubcategorySelection;
            }
            else
            {
                message.AppendLine("Desculpe, não há subcategorias disponíveis para a categoria escolhida. Por favor, escolha outra categoria.");
                currentState.CurrentStage = ConversationStage.MainMenu;
            }

            _userStateService.SetState(update.Message.Chat.Id, currentState);

            return message.ToString();
        }


        private string HandleSubcategorySelection(Update update, UserState currentState)
        {
            if (!int.TryParse(update.Message.Text, out int selectedSubcategoryId))
            {
                return "Por favor, insira um número válido para escolher uma opção.";
            }

            var conteudos = _conteudoDAL.GetConteudoBySubcategoriasId(selectedSubcategoryId);
            var message = new StringBuilder();

            if (conteudos != null && conteudos.Any())
            {
                message.AppendLine("Agora selecione um Conteúdo:\n\n");
                foreach (var conteudo in conteudos)
                {
                    message.AppendLine($"{conteudo.ConteudoID} - {conteudo.Titulo}");
                }

                message.AppendLine("\nPor favor, escolha o Conteúdo pelo número!");
                currentState.CurrentStage = ConversationStage.ContentSelection;
            }
            else
            {
                message.AppendLine("Desculpe, não há Conteúdos disponíveis para a opção escolhida. Por favor, escolha outra Categoria.\n\n");
                currentState.CurrentStage = ConversationStage.CategorySelection;
                message.AppendLine(DisplayMainMenu());

            }

            _userStateService.SetState(update.Message.Chat.Id, currentState);

            return message.ToString();
        }


        private async Task<string> HandleContentDetailAsync(Update update, UserState currentState)
        {
            if (!int.TryParse(update.Message.Text, out int selectedConteudoId))
            {
                return "Por favor, insira um número válido para escolher um conteúdo.";
            }

            var conteudo = _conteudoDAL.GetConteudoById(selectedConteudoId);
            if (conteudo == null)
            {
                return "Desculpe, o conteúdo selecionado não foi encontrado. Por favor, escolha outro conteúdo.";
            }

            var messagesToSend = new List<string>
    {
        $"Título: {conteudo.Titulo}",
        $"Descrição: {conteudo.Descricao}"
    };



            if (!string.IsNullOrEmpty(conteudo.Link))
            {
                messagesToSend.Add($"Link: {conteudo.Link}");
            }

            if (!string.IsNullOrEmpty(conteudo.Texto))
            {
                messagesToSend.Add($"Texto: {conteudo.Texto}");
            }

            foreach (var msg in messagesToSend)
            {
                await _botClient.SendTextMessageAsync(update.Message.Chat.Id, msg);
                await Task.Delay(500); // Adiciona delay de meio segundo entre as mensagens
            }



           currentState.CurrentStage = ConversationStage.MainMenu; 
           // currentState.SelectedCategoryId = null; // Resetamos para o estado padrão.
             _userStateService.SetState(update.Message.Chat.Id, currentState);

            return "";
        }


        private string HandleFAQs(Update update, UserState currentState)
        {
            var message = new StringBuilder();

            if (currentState.CurrentStage == ConversationStage.FAQs && (currentState.SelectedCategoryId == null && currentState.SelectedSubcategoryId == null))
            {

                var faqs = _faqDAL.GetFAQ();

                message.AppendLine("Aqui estão as perguntas frequentes disponíveis:\n\n");

                foreach (var faq in faqs)
                {
                    message.AppendLine($"{faq.FAQID} - {faq.Pergunta}");
                }

                message.AppendLine("\nPor favor, escolha a pergunta pelo número para ver a resposta.");
                currentState.SelectedCategoryId = -1; // Usamos este valor para indicar que uma FAQ foi selecionada.


            }
            // Se o usuário já escolheu uma FAQ específica:
            else if (currentState.CurrentStage == ConversationStage.FAQs && currentState.SelectedCategoryId == -1)
            {
                if (int.TryParse(update.Message.Text, out int selectedFAQId))
                {
                    var faq = _faqDAL.GetFAQById(selectedFAQId);

                    if (faq != null)
                    {
                        var messagetosend = new List<string>
                                {
                            $"Pergunta: {faq.Pergunta}",
                            $"Resposta: {faq.Resposta}"
                                };
                        //  messagetosend.Add(DisplayMainMenu());
                        foreach (var msg in messagetosend)
                        {
                            _botClient.SendTextMessageAsync(update.Message.Chat.Id, msg);
                        }
                        currentState.CurrentStage = ConversationStage.MainMenu; // Redirecionar o usuário para o menu principal após mostrar a resposta.
                        currentState.SelectedCategoryId = null; // Resetamos para o estado padrão.

                    }
                    else
                    {

                        message.AppendLine("Desculpe, não consegui encontrar essa pergunta. Por favor, selecione um número válido ou retorne ao menu principal.");
                        message.AppendLine(DisplayMainMenu());
                    }
                }
                else
                {
                    message.AppendLine("Por favor, insira um número válido para escolher uma pergunta.");
                }
            }

            // Atualize o estado do usuário no armazenamento.
             _userStateService.SetState(update.Message.Chat.Id, currentState);

            return message.ToString();
        }


        private string DisplayMainMenu()
        {
            var categorias = _categoriaDAL.GetCategorias();
            var message = new StringBuilder("Aqui estão as opções disponíveis:\n\n");

            // Adicione a opção de FAQs.
            message.AppendLine("0 - Perguntas Frequentes");

            foreach (var categoria in categorias)
            {
                message.AppendLine($"{categoria.CategoriaID} - {categoria.Nome}");
            }
            message.AppendLine("\nPor favor, escolha a opção pelo número!");

            return message.ToString();
        }
    }

}
