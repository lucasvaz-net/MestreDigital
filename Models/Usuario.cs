namespace MestreDigital.Models
{
    public class Usuario
    {
        public int UsuarioID { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; } 
        public int TipoID { get; set; }
        public  TipoDeUsuario TipoDeUsuario { get; set; }
        public Status Status { get; set; }
        public string ChatId { get; set; }
    }
}
