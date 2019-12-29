using NFCE.API.Attributes;

namespace NFCE.API.Models
{
    [ExtracaoAttribute(HtmlTag = "div", HtmlId = "totalNota")]
    public class ExtracaoPagamentoModel
    {
        public int Id { get; set; }
        public int IdControle { get; set; }
        [ExtracaoAttribute(HtmlTag = "label", PrecedingTag = "label", PrecedingText = "Forma de pagamento", Index = 1)]
        public string FormaPagamento { get; set; }
        [ExtracaoAttribute(HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Forma de pagamento", Index = 2)]
        public decimal ValorPago { get; set; }
        [ExtracaoAttribute(HtmlTag = "span", HtmlClass = "totalNumb", PrecedingTag = "label", PrecedingText = "Troco", Index = 1, Pattern = ExtracaoPatternsModel.Float)]
        public decimal Troco { get; set; }
        [ExtracaoAttribute(HtmlTag = "span", HtmlClass = "totalNumb txtObs", PrecedingTag = "label", PrecedingClass = "txtObs", Index = 1)]
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

    }
}