using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NFCE.API.Attributes;
using NFCE.API.Enums;

namespace NFCE.API.Models
{
    [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.CupomFiscalAvancado, HtmlTag = "fieldset", AncestorTag = "div", AncestorClass = "conteudo")]
    [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalBasico, HtmlTag = "div", HtmlClass = "txtCenter", PrecedingTag = "div", PrecedingId = "avisos", Index = 1)]
    [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "div", HtmlId = "Emitente", HtmlClass = "GeralXslt")]
    public class EmissorModel
    {
        public int Id { get; set; }
        public long IdLocalidade { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.CupomFiscalAvancado, HtmlTag = "span", HtmlId = "conteudo_lblEmitenteDadosNome")]
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalBasico, HtmlTag = "div", HtmlClass = "txtTopo", Index = 1)]
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Nome / Razão Social", PrecedingSibling = true, Index = 1)]
        public string RazaoSocial { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.CupomFiscalAvancado, HtmlTag = "span", HtmlId = "conteudo_lblEmitenteDadosNomeFantasia")]
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Nome Fantasia", PrecedingSibling = true, Index = 1)]
        public string NomeFantasia { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.CupomFiscalAvancado, HtmlTag = "span", HtmlId = "conteudo_lblEmitenteDadosEmitenteCnpj")]
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalBasico, HtmlTag = "div", HtmlClass = "text", Index = 1, Pattern = ExtracaoPatternsModel.AposDoisPontosString)]
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "CNPJ", PrecedingSibling = true, Index = 1)]
        public string CNPJ { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.CupomFiscalAvancado, HtmlTag = "span", HtmlId = "conteudo_lblEmitenteDadosInscricaoEstatual")]
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Inscrição Estadual", PrecedingSibling = true, Index = 1)]
        public string InscricaoEstadual { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalBasico, HtmlTag = "div", HtmlClass = "text", Index = 2)]
        public string Endereco { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.CupomFiscalAvancado, HtmlTag = "span", HtmlId = "conteudo_lblEmitenteDadosUf")]
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "UF", PrecedingSibling = true, Index = 1)]
        public string UF { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.CupomFiscalAvancado, HtmlTag = "span", HtmlId = "conteudo_lblEmitenteDadosMunicipio")]
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Município", PrecedingSibling = true, Index = 1)]
        public string Municipio { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.CupomFiscalAvancado, HtmlTag = "span", HtmlId = "conteudo_lblEmitenteDadosBairro")]
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Bairro / Distrito", PrecedingSibling = true, Index = 1)]
        public string Distrito { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.CupomFiscalAvancado, HtmlTag = "span", HtmlId = "conteudo_lblEmitenteDadosCep")]
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "CEP", PrecedingSibling = true, Index = 1)]
        public string CEP { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Telefone", PrecedingSibling = true, Index = 1)]
        public string Telefone { get; set; }
        public string Numero { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.CupomFiscalAvancado, HtmlTag = "span", HtmlId = "conteudo_lblEmitenteDadosEndereco")]
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Endereço", Index = 1)]
        public string Logradouro { get; set; }
        public DateTime DataInsercao { get; set; }
        public bool Valido
        {
            get
            {
                CompletaValores();
                return (
                    !string.IsNullOrEmpty(RazaoSocial) &&
                    !string.IsNullOrEmpty(CNPJ) &&
                    !string.IsNullOrEmpty(Logradouro) &&
                    !string.IsNullOrEmpty(Numero) &&
                    !string.IsNullOrEmpty(UF)
                );
            }
        }

        public void CompletaValores()
        {
            if (string.IsNullOrEmpty(Endereco))
            {
                var regex = Regex.Match(Logradouro, @"\d+");
                if (regex.Success)
                {
                    Numero = regex.Value;
                    Logradouro = Logradouro.Replace(Numero, "").Replace(".", "").Replace(",", "").Trim();
                    Numero = int.Parse(Numero.Trim()).ToString();
                }
                Distrito = Distrito?.Trim();
                InscricaoEstadual = InscricaoEstadual?.Replace(".", "").Trim();
                Telefone = Telefone?.Trim();
                CNPJ = CNPJ?.Replace("-", "").Replace(".", "").Replace("/", "");
                CEP = CEP?.Replace("-", "");
            }
            else
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
}