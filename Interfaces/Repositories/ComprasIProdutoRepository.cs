using System.Collections.Generic;
using NFCE.API.Models;
using NFCE.API.Models.Request;

namespace NFCE.API.Interfaces.Repositories
{
    public interface IComprasProdutoRepository : IRepositoryBase<ComprasProdutoModel>
    {
        bool DeletarPorCompra(int IdCompra);
    }
}