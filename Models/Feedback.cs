namespace MestreDigital.Model
{
    public class Feedback
    {
        public int FeedbackID { get; set; }
        public int ConteudoID { get; set; }
        public DateTime Data { get; set; }
        public string Comentario { get; set; }
        public string Avaliacao { get; set; }
        public Conteudo Conteudo { get; set; }
    }
}
