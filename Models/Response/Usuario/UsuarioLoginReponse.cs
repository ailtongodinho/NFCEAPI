using System;

namespace NFCE.API.Models.Response.Usuario
{
    public class UsuarioLoginResponse
    {
        public bool Authenticated { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Expiration { get; set; }
        public string AccessToken { get; set; }
        public string Message { get; set; }
    }
}