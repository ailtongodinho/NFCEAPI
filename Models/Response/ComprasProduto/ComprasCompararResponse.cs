using System.Collections.Generic;
using System.Linq;

namespace NFCE.API.Models.Response.ComprasProduto
{
    public class ComprasCompararResponse
    {
        public EmissorModel Emissor { get; set; }
        public IEnumerable<ComprasProdutoCompararResponse> Dados { get; set; }
        public decimal Total => Dados.Sum(x => x.Saldo.ValorUnitario);
        public int Quantidade => Dados.Count();
    }
}