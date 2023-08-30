namespace MestreDigital.Data
{
    using MestreDigital.Model;
    using System.Collections.Generic;

    public static class InMemoryStateStore
    {
        private static Dictionary<long, UserState> _userStates = new Dictionary<long, UserState>(); // Alterado int para long

        public static UserState GetState(long chatId)  // Alterado int para long
        {
            if (_userStates.TryGetValue(chatId, out var state))
            {
                return state;
            }
            return null;
        }

        public static void SetState(long chatId, UserState state)  // Alterado int para long
        {
            _userStates[chatId] = state;
        }
    }


}
