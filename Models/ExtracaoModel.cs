using System;
using System.Collections.Generic;
using System.Linq;
using NFCE.API.Attributes;
using NFCE.API.Enums;

namespace NFCE.API.Models
{
    [ExtracaoAttribute(HtmlTag = "body")]
    public class ExtracaoModel
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        // [Required]
        public string URL { get; set; }
        [ExtracaoAttribute(HtmlTag = "span", HtmlClass = "chave", Pattern = ExtracaoPatternsModel.Int, PrecedingTag = "br")]
        public string ChaveAcesso { get; set; }
        [ExtracaoAttribute(HtmlTag = "strong", HtmlText = "Emissão:", Index = -1, RegexIndex = 1, Pattern = ExtracaoPatternsModel.DataHora)]
        public DateTime Emissao { get; set; }
        public ExtracaoPagamentoModel Pagamento { get; set; }
        public ExtracaoEmissorModel Emissor { get; set; }
        public List<ExtracaoItemModel> Items { get; set; }
        public StatusExtracaoEnum Status { get; set; }
        public bool Valido
        {
            get
            {
                return (
                    Pagamento != null && Pagamento.Valido &&
                    Items != null && Items.Count(x => x.Valido) == Items.Count() &&
                    Emissor != null && Emissor.Valido
                );
            }
        }
        public int Tentativas { get; set; }

        public ExtracaoModel()
        {
            IdUsuario = 1;
            Status = StatusExtracaoEnum.Erro;
        }
    }
}