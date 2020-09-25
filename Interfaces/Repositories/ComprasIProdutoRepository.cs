using System.Collections.Generic;
using NFCE.API.Models;
using NFCE.API.Models.Request;
using NFCE.API.Models.Response.ComprasProduto;

namespace NFCE.API.Interfaces.Repositories
{
    public interface IComprasProdutoRepository : IRepositoryBase<ComprasProdutoModel>
    {
        bool DeletarPorCompra(int IdCompra);
        IEnumerable<ComprasProdutoCompararResponse> CompararEmissores(int idCompra);
    }
}