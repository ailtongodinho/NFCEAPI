using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using NFCE.API.Attributes;
using NFCE.API.Enums;

namespace NFCE.API.Models
{
    [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.CupomFiscalAvancado, HtmlTag = "body")]
    [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalBasico, HtmlTag = "body")]
    [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "div", HtmlId = "NFe", HtmlClass = "GeralXslt")]
    public class NotaModel
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        // [Required]
        public string URL { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.CupomFiscalAvancado, HtmlTag = "span", HtmlId = "conteudo_lblChaveAcesso")]
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalBasico, HtmlTag = "span", HtmlClass = "chave", Pattern = ExtracaoPatternsModel.Int, PrecedingTag = "br")]
        public string ChaveAcesso { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.CupomFiscalAvancado, HtmlTag = "span", HtmlId = "conteudo_lblDataEmissao")]
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Data de Emissão", PrecedingSibling = true, Index = 1)]
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalBasico, HtmlTag = "strong", HtmlText = "Emissão:", Index = -1, RegexIndex = 1, Pattern = ExtracaoPatternsModel.DataHora)]
        public DateTime Emissao { get; set; }
        public PagamentoModel Pagamento { get; set; }
        public EmissorModel Emissor { get; set; }
        public List<ItemModel> Items { get; set; }
        public StatusExtracaoEnum Status { get; set; }
        // public decimal ValorTotal { get { return Items.Sum(x => x.ValorTotal); } }
        public decimal ValorTotal { get; set; }
        public bool Favorito { get; set; }
        public int QuantidadeItens { get; set; }
        [JsonIgnore]
        public int IdEmissor { get; set; }
        public bool Valido
        {
            get
            {
                return (
                    Pagamento != null && Pagamento.Valido &&
                    Items != null && Items.Count(x => x.Valido) == Items.Count() &&
                    Emissor != null && Emissor.Valido &&
                    Emissao != null && Emissao > DateTime.MinValue
                );
            }
        }
        public int Tentativas { get; set; }

        public NotaModel()
        {
            IdUsuario = 1;
            Status = StatusExtracaoEnum.Erro;
        }
    }
}