namespace MestreDigital.Model
{
    public class Conteudo
    {
        public int ConteudoID { get; set; }
        public int SubcategoriaID { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string? Link { get; set; }
        public string? Texto { get; set; }
        public Subcategoria Subcategoria { get; set; }

    }
}
