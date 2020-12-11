using NFCE.API.Attributes;
using NFCE.API.Enums;

namespace NFCE.API.Models.Extracao
{
    [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "table", HtmlClass = "toggable box", AncestorTag = "div", AncestorId = "Prod", AncestorClass = "GeralXslt")]
    public class ExtracaoAvancadoItemModel
    {
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Código do Produto", PrecedingSibling = true, Index = 1)]
        public string Codigo { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Código NCM", PrecedingSibling = true, Index = 1)]
        public string NCM { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Código CEST", PrecedingSibling = true, Index = 1)]
        public string CEST { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Indicador de Escala Relevante", PrecedingSibling = true, Index = 1)]
        public string IndicadorEscalaRelevante { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "CNPJ do Fabricante da Mercadoria", PrecedingSibling = true, Index = 1)]
        public string CNPJFabricanteMercadoria { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Código de Benefício Fiscal na UF", PrecedingSibling = true, Index = 1)]
        public string CodigoBeneficioFiscalUF { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Código EX da TIPI", PrecedingSibling = true, Index = 1)]
        public string CodigoEXTIPI { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "CFOP", PrecedingSibling = true, Index = 1)]
        public string CFOP { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Outras Despesas Acessórias", PrecedingSibling = true, Index = 1)]
        public string OutrasDespesasAcessorias { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Valor do Desconto", PrecedingSibling = true, Index = 1)]
        public string ValorDesconto { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Valor Total do Frete", PrecedingSibling = true, Index = 1)]
        public string ValorTotalFrete { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Valor do Seguro", PrecedingSibling = true, Index = 1)]
        public string ValorSeguro { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Indicador de Composição do Valor Total da NF-e", PrecedingSibling = true, Index = 1)]
        public string IndicadorComposicaoValorTotalNFe { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Código EAN Comercial", PrecedingSibling = true, Index = 1)]
        public string CodigoEANComercial { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Unidade Comercial", PrecedingSibling = true, Index = 1)]
        public string UnidadeComercial { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Quantidade Comercial", PrecedingSibling = true, Index = 1)]
        public decimal QuantidadeComercial { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Código EAN Tributável", PrecedingSibling = true, Index = 1)]
        public string CodigoEANTributavel { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Unidade Tributável", PrecedingSibling = true, Index = 1)]
        public string UnidadeTributavel { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Quantidade Tributável", PrecedingSibling = true, Index = 1)]
        public decimal QuantidadeTributavel { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Valor unitário de comercialização", PrecedingSibling = true, Index = 1)]
        public decimal ValorUnitarioComercializacao { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Valor unitário de tributação", PrecedingSibling = true, Index = 1)]
        public decimal ValorUnitarioTributacao { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Número do pedido de compra", PrecedingSibling = true, Index = 1)]
        public string NumeroPedidoCompra { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Item do pedido de compra", PrecedingSibling = true, Index = 1)]
        public string ItemPedidoCompra { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Valor Aproximado dos Tributos", PrecedingSibling = true, Index = 1)]
        public decimal ValorAproximadoTributos { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Número da FCI", PrecedingSibling = true, Index = 1)]
        public string NumeroFCI { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Origem da Mercadoria", PrecedingSibling = true, Index = 1)]
        public string OrigemMercadoria { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Tributação do ICMS", PrecedingSibling = true, Index = 1)]
        public string TributacaoICMS { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Valor da BC do ICMS ST retido", PrecedingSibling = true, Index = 1)]
        public decimal ValorBC_ICMS_ST_Retido { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Alíquota suportada pelo Consumidor Final", PrecedingSibling = true, Index = 1)]
        public decimal AliquotaSuportadaConsumidorFinal { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Valor do ICMS ST retido", PrecedingSibling = true, Index = 1)]
        public decimal ValorICMS_ST_Retido { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Valor da Base de Cálculo do FCP retido anteriormente por ST", PrecedingSibling = true, Index = 1)]
        public string ValorBaseCalculoFCPRetidoAnteriormenteST { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Percentual do FCP retido anteriormente por Substituição Tributária", PrecedingSibling = true, Index = 1)]
        public string PercFCPRetidoAnteriormenteSubstituicaoTributaria { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Valor do FCP retido por Substituição Tributária", PrecedingSibling = true, Index = 1)]
        public string ValorFCPRetidoSubstituicaoTributaria { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Percentual de redução da base de cálculo efetiva", PrecedingSibling = true, Index = 1)]
        public string PercReduçãoBaseCalculoEfetiva { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Valor da base de cálculo efetiva", PrecedingSibling = true, Index = 1)]
        public string ValorBaseCalculoEfetiva { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Alíquota do ICMS efetiva", PrecedingSibling = true, Index = 1)]
        public string AliquotaICMSEfetiva { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Valor do ICMS efetivo", PrecedingSibling = true, Index = 1)]
        public string ValorICMSEfetivo { get; set; }
        #region PIS
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "CST", PrecedingSibling = true, AncestorTag = "table", AncestorClass = "box", Index = 1, StartsWith = true)]
        public string PIS_CST { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Base de Cálculo", PrecedingSibling = true, AncestorTag = "table", AncestorClass = "box", Index = 1, StartsWith = true)]
        public decimal PIS_BaseCalculo { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Alíquota", PrecedingSibling = true, AncestorTag = "table", AncestorClass = "box", Index = 1, StartsWith = true)]
        public decimal PIS_Aliquota { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Valor", PrecedingSibling = true, AncestorTag = "table", AncestorClass = "box", Index = 1, StartsWith = true)]
        public decimal PIS_Valor { get; set; }
        #endregion
        #region COFINS
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Valor", PrecedingSibling = true, AncestorTag = "table", AncestorClass = "box", Index = 2, StartsWith = true)]
        public string COFINS_CST { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Base de Cálculo", PrecedingSibling = true, AncestorTag = "table", AncestorClass = "box", Index = 2, StartsWith = true)]
        public decimal COFINS_BaseCalculo { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Alíquota", PrecedingSibling = true, AncestorTag = "table", AncestorClass = "box", Index = 2, StartsWith = true)]
        public decimal COFINS_Aliquota { get; set; }
        [ExtracaoAttribute(Tipo = ExtracaoProcessamentoEnum.NotaFiscalAvancado, HtmlTag = "span", PrecedingTag = "label", PrecedingText = "Valor", PrecedingSibling = true, AncestorTag = "table", AncestorClass = "box", Index = 2, StartsWith = true)]
        public decimal COFINS_Valor { get; set; }
        #endregion
    }
}