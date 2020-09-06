using NFCE.API.Models.Saldos;

namespace NFCE.API.Models.Response.Produto
{
    public class ProdutoResponse : ProdutoModel
    {
        public SaldosModel Saldo { get; set; }
        public EmissorModel Emissor { get; set; }
    }
}