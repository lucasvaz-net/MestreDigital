namespace MestreDigital.Models
{
    public class Historia
    {
        public int IdHistoria { get; set; }
        public string DsHistoria { get; set; }
        public Usuario Usuario { get; set; }
        public DateTime DtHistoria { get; set; }
        public Sessao Sessao { get; set; }
        public string CmMsg { get; set; }
        public Status Status { get; set; }
    }
}
