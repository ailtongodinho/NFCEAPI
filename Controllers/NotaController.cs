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
    public class NotaController : BaseController
    {
        #region Parametros
        private readonly INotaService _notaService;
        #endregion

        #region Construtor
        public NotaController(INotaService notaService, IHttpContextAccessor httpContextAccessor, IUsuarioService usuarioService) : base(httpContextAccessor, usuarioService)
        {
            _notaService = notaService;
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
            return Ok(_notaService.Consultar(Id));
        }
        /// <summary>
        /// Consultar
        /// </summary>
        /// <returns>Modelo de Consulta</returns>
        [HttpGet("{Id}/Itens")]
        public ActionResult ConsultarItem(int Id, [FromServices] IItemService itemService)
        {
            return Ok(itemService.ConsultarPelaNota(Id));
        }
        /// <summary>
        /// Deletar
        /// </summary>
        /// <returns>Verdadeiro ou Falso</returns>
        [HttpDelete("[action]/{Id}")]
        public ActionResult Deletar(int Id)
        {
            return Ok(_notaService.Deletar(Id));
        }
        /// <summary>
        /// Novo Registro
        /// </summary>
        /// <param name="modelo">Modelo de nota</param>
        /// <returns>Identificação do Registro adicionado</returns>
        [HttpPost]
        public ActionResult Novo(NotaModel modelo)
        {
            return Ok(_notaService.Novo(modelo));
        }
        #endregion
        #region Metodos
        /// <summary>
        /// Favorita a nota
        /// </summary>
        /// <param name="modelo">URL de acesso a NFCE. Mais informações em http://nfce.encat.org/desenvolvedor/</param>
        /// <returns>Infomações extraídas da NFCE</returns>
        [HttpPost("[action]")]
        public IActionResult Favoritar([FromBody] NotaRequest modelo)
        {
            return Ok(_notaService.Favoritar(modelo));
        }
        /// <summary>
        /// Listar informações sobre as notas fiscais
        /// </summary>
        /// <returns>Informações extraídas da NFCE</returns>
        [HttpPost("[action]")]
        public IActionResult Listar(NotaListarRequest extracaoListarRequest)
        {
            return Ok(_notaService.ListarTotais(Usuario.Id, extracaoListarRequest));
        }
        /// <summary>
        /// Ordenar informações sobre as notas fiscais
        /// </summary>
        /// <returns>Informações extraídas da NFCE</returns>
        [HttpPost("[action]")]
        public IActionResult Ordenar(NotaOrdenarRequest notaOrdenarRequest)
        {
            return Ok(_notaService.Ordenar(notaOrdenarRequest));
        }
        [Authorize]
        [HttpGet("[action]")]
        public IActionResult Agregado([FromServices] INotaService notaService)
        {
            return Ok(_notaService.Agregado());
        }
        #endregion
    }
}

