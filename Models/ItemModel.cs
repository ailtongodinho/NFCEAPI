using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using NFCE.API.Attributes;
using NFCE.API.Enums;
using NFCE.API.Models.Extracao;

namespace NFCE.API.Models
{
    [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalBasico, HtmlTag = "tr", HtmlId = "Item")]
    [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "table", HtmlClass = "toggle box", AncestorTag = "div", AncestorId = "Prod", AncestorClass = "GeralXslt")]
    public class ItemModel
    {
        public int Id { get; set; }
        public int IdControle { get; set; }
        public int IdProduto { get; set; }
        [Column("Descrição")]
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", AncestorTag = "td", AncestorClass = "fixo-prod-serv-descricao")]
        public string Nome { get; set; }
        [Column("Cód. Produto")]
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalBasico, HtmlTag = "span", HtmlClass = "RCod", Pattern = ExtracaoPatternsModel.AposDoisPontosInt)]
        public string Codigo { get; set; }
        [Column("Qtd. Comercial")]
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalBasico, HtmlTag = "span", HtmlClass = "Rqtd", Pattern = ExtracaoPatternsModel.AposDoisPontosFloat)]
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", AncestorTag = "td", AncestorClass = "fixo-prod-serv-qtd")]
        public decimal Quantidade { get; set; }
        [Column("Unid. Comercial")]
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalBasico, HtmlTag = "span", HtmlClass = "RUN", Pattern = ExtracaoPatternsModel.AposDoisPontosString)]
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", AncestorTag = "td", AncestorClass = "fixo-prod-serv-uc")]
        public string Unidade { get; set; }
        [Column("Valor Unit.")]
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalBasico, HtmlTag = "span", HtmlClass = "RvlUnit", Pattern = ExtracaoPatternsModel.AposDoisPontosFloat)]
        public decimal ValorUnitario { get; set; }
        [Column("Valor Líquido do Item")]
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalBasico, HtmlTag = "span", HtmlClass = "valor", Pattern = ExtracaoPatternsModel.AposDoisPontosFloat)]
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", AncestorTag = "td", AncestorClass = "fixo-prod-serv-vb")]
        public decimal ValorTotal { get; set; }
        [Column("Valor Líquido do Item")]
        public string CEST { get; set; }
        [Column("Cód. NCM")]
        public string NCM { get; set; }
        [Column("CFOP")]
        public string CFOP { get; set; }
        public string EAN { get; set; }
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

        public ItemModel() { }
        public ItemModel(ItemModel item, ExtracaoAvancadoItemModel extracaoAvancadoItem)
        {
            // Item
            Nome = item.Nome;
            Quantidade = item.Quantidade;
            Unidade = item.Unidade;
            ValorTotal = item.ValorTotal;
            //  Avançado
            Codigo = item.Codigo ?? extracaoAvancadoItem.Codigo;
            ValorUnitario = extracaoAvancadoItem.ValorUnitarioComercializacao;
            CEST = extracaoAvancadoItem.CEST;
            NCM = extracaoAvancadoItem.NCM;
            CFOP = extracaoAvancadoItem.CFOP;
            EAN = extracaoAvancadoItem.CodigoEANComercial;
        }
    }
}