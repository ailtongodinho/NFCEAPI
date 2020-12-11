using System.Collections.Generic;
using System.Linq;
using NFCE.API.Models.Saldos;

namespace NFCE.API.Models.Response.ComprasProduto
{
    public class ComprasProdutoCompararResponse
    {
        public ProdutoModel Produto { get; set; }
        public SaldosModel Saldo { get; set; }
    }
}