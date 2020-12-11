using System.Collections.Generic;
using System.Linq;

namespace NFCE.API.Models.Response.ComprasProduto
{
    public class ComprasProdutoAgregadoResponse
    {
        public List<ComprasProdutoModel> Dados { get; set; }
        public int Quantidade { get; set; }
        public decimal Total { get; set; }
        public ComprasProdutoAgregadoResponse(List<ComprasProdutoModel> dados)
        {
            Dados = dados;
            Total = dados.Sum(x => x.Produto.Saldo.ValorUnitario * x.Quantidade);
            Quantidade = dados.Count(x => x.Quantidade > 0);
        }
    }
}