using System;
using System.Collections.Generic;
using System.Linq;
using NFCE.API.Helpers;
using NFCE.API.Interfaces.Repositories;
using NFCE.API.Interfaces.Services;
using NFCE.API.Models;
using NFCE.API.Models.Handler;
using NFCE.API.Models.Request;
using NFCE.API.Models.Response.ComprasProduto;

namespace NFCE.API.Services
{
    public class ComprasProdutoService : IComprasProdutoService
    {
        private readonly IComprasProdutoRepository _ComprasProdutoRepository;
        private readonly IProdutoService _produtoService;
        private readonly IEmissorService _emissorService;
        public ComprasProdutoService(IComprasProdutoRepository ComprasProdutoRepository, IProdutoService produtoService, IEmissorService emissorService)
        {
            _ComprasProdutoRepository = ComprasProdutoRepository;
            _produtoService = produtoService;
            _emissorService = emissorService;
        }
        #region CRUD
        /// <summary>
        /// Consultar
        /// </summary>
        /// <returns>Modelo de Consulta</returns>
        public ComprasProdutoModel Consultar(int Id)
        {
            return _ComprasProdutoRepository.GetById(Id);
        }
        /// <summary>
        /// Consultar
        /// </summary>
        /// <returns>Modelo de Consulta</returns>
        public IEnumerable<ComprasProdutoModel> ConsultarPorCompra(int IdCompra)
        {
            return _ComprasProdutoRepository.GetList(x => x.IdCompra == IdCompra);
        }
        /// <summary>
        /// Deletar
        /// </summary>
        /// <returns>Verdadeiro ou Falso</returns>
        public bool Deletar(int Id)
        {
            return _ComprasProdutoRepository.Delete(Id);
        }
        /// <summary>
        /// Deletar todos os registros da compra
        /// </summary>
        /// <returns>Verdadeiro ou Falso</returns>
        public bool DeletarPorCompra(int IdCompra)
        {
            return _ComprasProdutoRepository.DeletarPorCompra(IdCompra);
        }
        /// <summary>
        /// Novo Registro
        /// </summary>
        /// <param name="modelo">Modelo de ComprasProduto</param>
        /// <returns>Identificação do Registro adicionado</returns>
        public int Novo(ComprasProdutoModel modelo)
        {
            int retorno = -1;
            if (modelo.Quantidade <= 0) throw new HttpExceptionHandler("Adicione uma quantidade!");
            modelo.DataCriacao = DateTime.Now;
            // Verifica se existe na lista
            var produto = modelo.IdProduto > 0 ? Consultar(modelo) : null;
            modelo.Unidade = produto?.Unidade ?? modelo.Unidade;
            // if (modelo.Unidade == "KG")
            //     modelo.Quantidade = modelo.Quantidade / 1000;
            if (produto == null)
            {
                retorno = _ComprasProdutoRepository.Insert(modelo);
            }
            else
            {
                produto.Quantidade += modelo.Quantidade;
                if (produto.Quantidade <= 0)
                {
                    _ComprasProdutoRepository.Delete(produto.Id);
                }
                else
                {
                    _ComprasProdutoRepository.UpdateGenerico(produto, new { produto.DataCriacao, produto.Quantidade });
                }
                // retorno = produto.Id;
            }

            return retorno;
        }
        /// <summary>
        /// Atualizar Registro
        /// </summary>
        /// <param name="modelo">Modelo de ComprasProduto</param>
        /// <returns>Sucesso da atualização</returns>
        public bool Atualizar(ComprasProdutoModel modelo)
        {
            return _ComprasProdutoRepository.UpdateGenerico(modelo, new { modelo.Quantidade, modelo.Unidade });
        }
        #endregion
        #region Metodos
        /// <summary>
        /// Listar
        /// </summary>
        /// <returns>Modelo de Listagem</returns>
        // public IEnumerable<ComprasProdutoModel> Listar(ComprasProdutoListarRequest comprasProdutosListarRequest)
        public ComprasProdutoAgregadoResponse Listar(ComprasProdutoListarRequest comprasProdutosListarRequest)
        {
            var lista = _ComprasProdutoRepository.GetList(x => x.IdCompra == comprasProdutosListarRequest.IdCompra).ToList();
            var produtos = _produtoService.Listar(new ProdutoListarRequest { IdProdutos = lista.Select(x => x.IdProduto).Distinct().ToList() });

            var retorno = (
                from a in lista
                join p in produtos on a.IdProduto equals p.Id
                select new ComprasProdutoModel(a) { Produto = p }
            ).OrderBy(x => x.DataCriacao);

            return new ComprasProdutoAgregadoResponse(retorno.ToList());
        }
        /// <summary>
        /// Listar
        /// </summary>
        /// <returns>Modelo de Listagem</returns>
        public IEnumerable<ProdutoModel> Listar(ProdutoListarRequest produtoListarRequest)
        {
            return _produtoService.Listar(produtoListarRequest);
            // return _ComprasProdutoRepository.Listar(produtoListarRequest);
        }
        /// <summary>
        /// Consultar por produto
        /// </summary>
        /// <returns>Modelo de Consulta</returns>
        public ComprasProdutoModel Consultar(ComprasProdutoModel produto)
        {
            ComprasProdutoModel retorno = null;
            if (produto.Id > 0)
            {
                retorno = Consultar(produto.Id);
            }
            else
            {
                if (produto.IdProduto > 0)
                {
                    retorno = _ComprasProdutoRepository.GetList(x => x.IdCompra == produto.IdCompra && x.IdProduto == produto.IdProduto).FirstOrDefault();
                }
            }
            return retorno;
        }
        public IEnumerable<ComprasCompararResponse> Comparar(int idCompra)
        {
            var produtos = _ComprasProdutoRepository.CompararEmissores(idCompra);
            var emissores = produtos
                .GroupBy(x => x.Saldo.IdEmissor)
                .Select(x =>
                {
                    return new ComprasCompararResponse {
                        Emissor = _emissorService.Consultar(x.Key),
                        Dados = x
                    };
                });
            return emissores;
        }
        #endregion
    }
}