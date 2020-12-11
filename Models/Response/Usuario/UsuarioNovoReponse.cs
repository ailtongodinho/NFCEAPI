namespace NFCE.API.Models.Response.Usuario
{
    public class UsuarioNovoResponse
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public string SenhaTemporaria { get; set; }
    }
}