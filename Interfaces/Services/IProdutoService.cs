using System.Collections.Generic;
using NFCE.API.Models;
using NFCE.API.Models.Request;
using NFCE.API.Models.Response.Produto;

namespace NFCE.API.Interfaces.Services
{
    public interface IProdutoService : IBaseService<ProdutoModel>
    {
        #region CRUD
        ProdutoModel Consultar(int Id);
        int Novo(ProdutoModel modelo);
        bool Deletar(int Id);
        #endregion
        #region Metodos
        ProdutoModel Consultar(ProdutoModel produto);
        IEnumerable<ProdutoResponse> Listar(ProdutoListarRequest produtoListarRequest);
        #endregion
    }
}