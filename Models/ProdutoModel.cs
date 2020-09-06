using System;
using System.Collections.Generic;
using NFCE.API.Models.Saldos;

namespace NFCE.API.Models
{
    public class ProdutoModel
    {
        public string Apelido { get; set; }
        public int Id { get; set; }
        public int IdEmissor { get; set; }
        public string Codigo { get; set; }
        public string Unidade { get; set; }
        public string Nome { get; set; }
        public string CEST { get; set; }
        public string NCM { get; set; }
        public string CFOP { get; set; }
        public string EAN { get; set; }
        // public decimal ValorUnitario { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataSaldos { get; set; }
        // public SaldosModel Saldo { get; set; }
        // public EmissorModel Emissor { get; set; }
        public ProdutoModel() { }
        // public ProdutoModel(string codigo)
        // {
        //     Codigo = codigo;
        // }
        public ProdutoModel(ItemModel itemModel)
        {
            Unidade = itemModel.Unidade;
            Codigo = itemModel.Codigo;
            Nome = itemModel.Nome;
            CEST = itemModel.CEST;
            CFOP = itemModel.CFOP;
            NCM = itemModel.NCM;
            EAN = itemModel.EAN;
            DataCriacao = DateTime.Now;
        }
    }
}