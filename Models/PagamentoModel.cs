using NFCE.API.Attributes;
using NFCE.API.Enums;
using NFCE.API.Models.Extracao;

namespace NFCE.API.Models
{
    [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.CupomFiscalAvancado, HtmlTag = "div", HtmlClass = "conteudo")]
    [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalBasico, HtmlTag = "div", HtmlId = "totalNota")]
    [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "table", HtmlClass = "toggle box", AncestorTag = "div", AncestorId = "Cobranca", AncestorClass = "GeralXslt")]
    public class PagamentoModel
    {
        public int Id { get; set; }
        public int IdControle { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.CupomFiscalAvancado, HtmlTag = "span", HtmlId = "conteudo_grvMeiosPagamento_lblMeiosPagamentoCodigoMeioPagamento_0")]
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalBasico, HtmlTag = "label", PrecedingTag = "label", PrecedingText = "Forma de pagamento", Index = 1)]
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", AncestorTag = "td", Index = 1)]
        public string FormaPagamento { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", AncestorTag = "td", Index = 2)]
        public string MeioPagamento { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.CupomFiscalAvancado, HtmlTag = "span", HtmlId = "conteudo_grvMeiosPagamento_lblMeiosPagamentoValorMeioPagamento_0")]
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalBasico, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Forma de pagamento", Index = 2)]
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", AncestorTag = "td", Index = 3)]
        public decimal ValorPago { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.CupomFiscalAvancado, HtmlTag = "span", HtmlId = "conteudo_lblValorTroco")]
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalBasico, HtmlTag = "span", HtmlClass = "totalNumb", PrecedingTag = "label", PrecedingText = "Troco", Index = 1, Pattern = ExtracaoPatternsModel.Float)]
        public decimal Troco { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.CupomFiscalAvancado, HtmlTag = "span", HtmlId = "conteudo_lblvalorCFeLei12741")]
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalBasico, HtmlTag = "span", HtmlClass = "totalNumb txtObs", PrecedingTag = "label", PrecedingClass = "txtObs", Index = 1)]
        public decimal TributosTotaisIncidentes { get; set; }
        public bool Valido
        {
            get
            {
                return (
                    !string.IsNullOrEmpty(FormaPagamento) &&
                    ValorPago > 0
                );
            }
        }
        public PagamentoModel() { }
        public PagamentoModel(PagamentoModel pagamento, ExtracaoAvancadoPagamentoModel pagamentoAvancado)
        {
            this.FormaPagamento = pagamento.FormaPagamento;
            this.MeioPagamento = pagamento.MeioPagamento;
            this.ValorPago = pagamento.ValorPago;
            this.Troco = pagamentoAvancado.Troco;
            this.Troco = pagamentoAvancado.Troco;
        }
    }
}