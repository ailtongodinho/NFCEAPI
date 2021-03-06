using System.ComponentModel.DataAnnotations;
using NFCE.API.Models.Extracao;

namespace NFCE.API.Models.Request
{
    public class ExtracaoRequest
    {
        [Required]
        public string Url { get; set; }
        public string HostKey { get; set; }
        public string Imagem { get; set; }
        public ExtracaoAvancadoFisicoModel ExtracaoFisica { get; set; }
    }
}