using System.Collections.Generic;
using System.Linq;
using NFCE.API.Attributes;

namespace NFCE.API.Models
{
    [ExtracaoAttribute(HtmlTag = "div", HtmlClass = "txtCenter", PrecedingTag = "div", PrecedingId = "avisos", Index = 1)]
    public class ExtracaoEmissorModel
    {
        public int Id { get; set; }
        public int IdControle { get; set; }
        [ExtracaoAttribute(HtmlTag = "div", HtmlClass = "txtTopo", Index = 1)]
        public string RazaoSocial { get; set; }
        [ExtracaoAttribute(HtmlTag = "div", HtmlClass = "text", Index = 1, Pattern = ExtracaoPatternsModel.AposDoisPontosString)]
        public string CNPJ { get; set; }
        public string InscricaoEstadual { get; set; }
        [ExtracaoAttribute(HtmlTag = "div", HtmlClass = "text", Index = 2)]
        public string Endereco { get; set; }
        public string UF { get; set; }
        public string Municipio { get; set; }
        public string Distrito { get; set; }
        public string CEP { get; set; }
        public string Numero { get; set; }
        public string Logradouro { get; set; }
        public bool Valido
        {
            get
            {
                CompletaValores();
                return (
                    !string.IsNullOrEmpty(RazaoSocial) &&
                    !string.IsNullOrEmpty(CNPJ) &&
                    !string.IsNullOrEmpty(Endereco)
                );
            }
        }

        public void CompletaValores()
        {
            List<string> SplitEndereco = Endereco.Split(",").Select(x => { return x.Trim(); }).ToList();
            UF = SplitEndereco[SplitEndereco.Count - 1];
            Municipio = SplitEndereco[SplitEndereco.Count - 2];
            Distrito = SplitEndereco[SplitEndereco.Count - 3];
            Numero = SplitEndereco[SplitEndereco.Count - 5];
            Logradouro = SplitEndereco[SplitEndereco.Count - 6];
        }
    }
}