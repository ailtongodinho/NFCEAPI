using System.Collections.Generic;
using NFCE.API.Models;
using NFCE.API.Models.Request;
using NFCE.API.Models.Response.Produto;

namespace NFCE.API.Interfaces.Repositories
{
    public interface IProdutoRepository : IRepositoryBase<ProdutoModel>
    {
        IEnumerable<ProdutoResponse> Listar(ProdutoListarRequest produtoListarRequest);
    }
}