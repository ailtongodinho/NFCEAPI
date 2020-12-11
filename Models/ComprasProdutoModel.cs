using System;
using NFCE.API.Models.Response.Produto;

namespace NFCE.API.Models
{
    public class ComprasProdutoModel
    {
        public string Apelido { get; set; }
        public int Id { get; set; }
        public int IdCompra { get; set; }
        public int? IdProduto { get; set; }
        public ProdutoResponse Produto { get; set; }
        public decimal Quantidade { get; set; }
        public string Unidade { get; set; }
        public decimal Total { get; set; }
        public DateTime DataCriacao { get; set; }
        public ComprasProdutoModel()
        {
            IdProduto = IdProduto > 0 ? IdProduto : null;
        }
        public ComprasProdutoModel(ComprasProdutoModel model)
        {
            Apelido = model.Apelido;
            Id = model.Id;
            IdCompra = model.IdCompra;
            IdProduto = model.IdProduto;
            Produto = model.Produto;
            Quantidade = model.Quantidade;
            Unidade = model.Unidade;
            Total = model.Total;
            DataCriacao = model.DataCriacao;
        }
    }
}