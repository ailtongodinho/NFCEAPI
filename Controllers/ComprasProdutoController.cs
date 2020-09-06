using Microsoft.AspNetCore.Mvc;
using NFCE.API.Interfaces;
using NFCE.API.Models;
using Microsoft.AspNetCore.Authorization;
using NFCE.API.Helpers;
using System.Net;
using System;
using NFCE.API.Models.Request;
using NFCE.API.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace NFCE.API.Controllers
{
    /// <summary>
    /// Autentificação de usuários
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ComprasProdutoController : BaseController
    {
        #region Parametros
        private readonly IComprasProdutoService _ComprasProdutoService;
        #endregion

        #region Construtor
        public ComprasProdutoController(IComprasProdutoService ComprasProdutoService, IHttpContextAccessor httpContextAccessor, IUsuarioService usuarioService) : base(httpContextAccessor, usuarioService)
        {
            _ComprasProdutoService = ComprasProdutoService;
        }
        #endregion
        #region CRUD
        /// <summary>
        /// Consultar
        /// </summary>
        /// <returns>Modelo de Consulta</returns>
        [HttpGet("{Id}")]
        public ActionResult Consultar(int Id)
        {
            return Ok(_ComprasProdutoService.Consultar(Id));
        }
        /// <summary>
        /// Deletar
        /// </summary>
        /// <returns>Verdadeiro ou Falso</returns>
        [HttpDelete("[action]/{Id}")]
        public ActionResult Deletar(int Id)
        {
            return Ok(_ComprasProdutoService.Deletar(Id));
        }
        /// <summary>
        /// Novo Registro
        /// </summary>
        /// <param name="modelo">Modelo de Emissor</param>
        /// <returns>Identificação do Registro adicionado</returns>
        [HttpPost]
        public ActionResult Novo(ComprasProdutoModel modelo)
        {
            return Ok(_ComprasProdutoService.Novo(modelo));
        }
        /// <summary>
        /// Atualizar
        /// </summary>
        /// <returns>Verdadeiro ou Falso</returns>
        [HttpPost("[action]")]
        public ActionResult Atualizar(ComprasProdutoModel modelo)
        {
            return Ok(_ComprasProdutoService.Atualizar(modelo));
        }
        #endregion
        #region Metodos
        /// <summary>
        /// Listar
        /// </summary>
        /// <returns>Listagem das listas de comptas</returns>
        [HttpPost("[action]")]
        public ActionResult Listar(ComprasProdutoListarRequest comprasProdutosListarRequest)
        {
            return Ok(_ComprasProdutoService.Listar(comprasProdutosListarRequest));
        }
        /// <summary>
        /// Listar
        /// </summary>
        /// <returns>Listagem das listas de comptas</returns>
        [HttpPost("[action]")]
        public ActionResult ListarProdutos(ProdutoListarRequest produtoListarRequest)
        {
            return Ok(_ComprasProdutoService.Listar(produtoListarRequest));
        }
        #endregion
    }
}