using NFCE.API.Attributes;
using NFCE.API.Enums;

namespace NFCE.API.Models.Extracao
{
    public class ExtracaoAvancadoFisicoModel
    {
        public string Nota { get; set; }
        public string Emissor { get; set; }
        public string Pagamento { get; set; }
        public string Itens { get; set; }
    }
}