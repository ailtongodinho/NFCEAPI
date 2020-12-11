using NFCE.API.Attributes;
using NFCE.API.Enums;

namespace NFCE.API.Models.Extracao
{
    [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "table", HtmlClass = "toggable box", AncestorTag = "div", AncestorId = "Cobranca", AncestorClass = "GeralXslt")]
    public class ExtracaoAvancadoPagamentoModel
    {
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", AncestorTag = "td", Index = 1)]
        public string TipoIntegracaoPagamento { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", AncestorTag = "td", Index = 2)]
        public string CNPJCredenciadora  { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", AncestorTag = "td", Index = 3)]
        public string BandeiraOperadora  { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", AncestorTag = "td", Index = 4)]
        public string NumeroAutorização { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", AncestorTag = "td", Index = 5)]
        public decimal Troco { get; set; }
    }
}