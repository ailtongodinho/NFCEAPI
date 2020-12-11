using System;

namespace NFCE.API.Models.Response.Extracao
{
    public class ExtracaoResponse
    {
        public string HostKey { get; set; }
        public string Mensagem { get; set; }
        public DateTime? Emissao { get; set; }
        public int? IdNota { get; set; }
        public string Imagem { get; set; }
        public string Url { get; set; }
        public bool Sucesso { get; set; }
    }
}