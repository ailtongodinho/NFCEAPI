using System.Collections.Generic;
using NFCE.API.Models;
using NFCE.API.Models.Request;
using NFCE.API.Models.Response.ComprasProduto;

namespace NFCE.API.Interfaces.Services
{
    public interface IComprasProdutoService : IBaseService<ComprasProdutoModel>
    {
        #region CRUD
        ComprasProdutoModel Consultar(int Id);
        int Novo(ComprasProdutoModel modelo);
        bool Deletar(int Id);
        bool Atualizar(ComprasProdutoModel modelo);
        #endregion
        #region Metodos
        // IEnumerable<ComprasProdutoModel> Listar(ComprasProdutoListarRequest comprasProdutosListarRequest);
        ComprasProdutoAgregadoResponse Listar(ComprasProdutoListarRequest comprasProdutosListarRequest);
        ComprasProdutoModel Consultar(ComprasProdutoModel produto);
        IEnumerable<ComprasProdutoModel> ConsultarPorCompra(int IdCompra);
        IEnumerable<ProdutoModel> Listar(ProdutoListarRequest produtoListarRequest);
        bool DeletarPorCompra(int IdCompra);
        IEnumerable<ComprasCompararResponse> Comparar(int idCompra);
        #endregion
    }
}