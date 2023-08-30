namespace MestreDigital.Model
{
    public class Subcategoria
    {
        public int SubcategoriaID { get; set; }
        public int CategoriaID { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public Categoria Categoria { get; set; }
    }
}
