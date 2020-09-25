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
    // [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ComprasController : BaseController
    {
        #region Parametros
        private readonly IComprasService _ComprasService;
        private readonly IComprasProdutoService _ComprasProdutoService;
        #endregion

        #region Construtor
        /// <summary>
        /// CRUD e outros métodos para Compras
        /// </summary>
        /// <param name="ComprasService">Serviço de Compras</param>
        /// <param name="ComprasProdutoService">Serviço de Produtos</param>
        /// <param name="httpContextAccessor">Acesso de contexto</param>
        /// <param name="usuarioService">Serviço de Usuário</param>
        /// <returns></returns>
        public ComprasController(IComprasService ComprasService, IComprasProdutoService ComprasProdutoService, IHttpContextAccessor httpContextAccessor, IUsuarioService usuarioService) : base(httpContextAccessor, usuarioService)
        {
            _ComprasService = ComprasService;
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
            return Ok(_ComprasService.Consultar(Id));
        }
        /// <summary>
        /// Deletar
        /// </summary>
        /// <returns>Verdadeiro ou Falso</returns>
        [HttpDelete("[action]/{Id}")]
        public ActionResult Deletar(int Id)
        {
            return Ok(_ComprasService.Deletar(Id));
        }
        /// <summary>
        /// Novo Registro
        /// </summary>
        /// <param name="modelo">Modelo de Emissor</param>
        /// <returns>Identificação do Registro adicionado</returns>
        [HttpPost]
        public ActionResult Novo(ComprasModel modelo)
        {
            modelo.IdUsuario = Usuario.Id;
            return Ok(_ComprasService.Novo(modelo));
        }
        /// <summary>
        /// Comparar Compras
        /// </summary>
        /// <returns>Lista de Emissores por Produto</returns>
        [HttpGet("{Id}/Comparar")]
        public ActionResult Comparar(int Id)
        {
            return Ok(_ComprasProdutoService.Comparar(Id));
        }
        #endregion
        #region CRUD Produtos
        /// <summary>
        /// Consultar Produto
        /// </summary>
        /// <returns>Modelo de Consulta</returns>
        [HttpGet("Produto/{Id}")]
        public ActionResult ConsultarProduto(int Id)
        {
            return Ok(_ComprasProdutoService.Consultar(Id));
        }
        /// <summary>
        /// Deletar Produto
        /// </summary>
        /// <returns>Verdadeiro ou Falso</returns>
        [HttpDelete("Produto/Deletar/{Id}")]
        public ActionResult DeletarProduto(int Id)
        {
            return Ok(_ComprasProdutoService.Deletar(Id));
        }
        /// <summary>
        /// Novo Produto
        /// </summary>
        /// <param name="modelo">Modelo de Emissor</param>
        /// <returns>Identificação do Registro adicionado</returns>
        [HttpPost("Produto")]
        public ActionResult NovoProduto(ComprasProdutoModel modelo)
        {
            return Ok(_ComprasProdutoService.Novo(modelo));
        }
        /// <summary>
        /// Atualizar Produto
        /// </summary>
        /// <returns>Verdadeiro ou Falso</returns>
        [HttpPost("Produto/Atualizar")]
        public ActionResult AtualizarProduto(ComprasProdutoModel modelo)
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
        public ActionResult Listar()
        {
            return Ok(_ComprasService.Listar(Usuario.Id));
        }
        #endregion
        #region Metodos Produtos
        /// <summary>
        /// Listar
        /// </summary>
        /// <returns>Listagem das listas de comptas</returns>
        [HttpPost("Produto/Listar")]
        public ActionResult Listar(ComprasProdutoListarRequest comprasProdutosListarRequest)
        {
            return Ok(_ComprasProdutoService.Listar(comprasProdutosListarRequest));
        }
        /// <summary>
        /// Listar
        /// </summary>
        /// <returns>Listagem das listas de comptas</returns>
        [HttpPost("Produto/ListarProdutos")]
        public ActionResult ListarProdutos(ProdutoListarRequest produtoListarRequest)
        {
            return Ok(_ComprasProdutoService.Listar(produtoListarRequest));
        }
        #endregion
    }
}