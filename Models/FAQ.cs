namespace MestreDigital.Model
{
    public class FAQ
    {
        public int FAQID { get; set; }
        public string Pergunta { get; set; }
        public string Resposta { get; set; }
        public int? SubcategoriaID { get; set; } 
        public Subcategoria Subcategoria { get; set; }
    }
}
