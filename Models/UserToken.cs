namespace MestreDigital.Models
{
    public class UserToken
    {
        public int TokenID { get; set; }
        public int UserID { get; set; }
        public string TokenValue { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string SessionInfo { get; set; } 
    }

}
