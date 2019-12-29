using NFCE.API.Attributes;

namespace NFCE.API.Models
{
    [ExtracaoAttribute(HtmlTag = "tr", HtmlId = "Item")]
    public class ExtracaoItemModel
    {
        public int Id { get; set; }
        public int IdControle { get; set; }
        [ExtracaoAttribute(HtmlTag = "span", HtmlClass = "txtTit")]
        public string Nome { get; set; }
        [ExtracaoAttribute(HtmlTag = "span", HtmlClass = "RCod", Pattern = ExtracaoPatternsModel.AposDoisPontosInt)]
        public int Codigo { get; set; }
        [ExtracaoAttribute(HtmlTag = "span", HtmlClass = "Rqtd", Pattern = ExtracaoPatternsModel.AposDoisPontosFloat)]
        public decimal Quantidade { get; set; }
        [ExtracaoAttribute(HtmlTag = "span", HtmlClass = "RUN", Pattern = ExtracaoPatternsModel.AposDoisPontosString)]
        public string Unidade { get; set; }
        [ExtracaoAttribute(HtmlTag = "span", HtmlClass = "RvlUnit", Pattern = ExtracaoPatternsModel.AposDoisPontosFloat)]
        public decimal ValorUnitario { get; set; }
        [ExtracaoAttribute(HtmlTag = "span", HtmlClass = "valor", Pattern = ExtracaoPatternsModel.AposDoisPontosFloat)]
        public decimal ValorTotal { get; set; }
        public bool Valido
        {
            get
            {
                return (
                    !string.IsNullOrEmpty(Nome) &&
                    ValorUnitario > 0
                );
            }
        }

    }
}