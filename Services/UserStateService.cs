using MestreDigital.Data;
using MestreDigital.Model;

namespace MestreDigital.Services
{
    public class UserStateService
    {
        public UserState GetState(long chatId)
        {
            // Lógica para obter o estado do usuário.
            return InMemoryStateStore.GetState(chatId);
        }

        public void SetState(long chatId, UserState state)
        {
            // Lógica para definir o estado do usuário.
            InMemoryStateStore.SetState(chatId, state);
        }

     
    }

}
