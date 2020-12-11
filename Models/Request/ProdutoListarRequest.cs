using System.Collections.Generic;

namespace NFCE.API.Models.Request
{
    public class ProdutoListarRequest
    {
        public List<int?> IdProdutos { get; set; }
        public int? IdCompra { get; set; }
        public string NomeProduto { get; set; }
    }
}