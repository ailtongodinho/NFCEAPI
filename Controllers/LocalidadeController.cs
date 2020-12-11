using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NFCE.API.Interfaces.Services;
using NFCE.API.Models;
using NFCE.API.Models.Request;
using NFCE.API.Services;

namespace NFCE.API.Controllers
{
    /// <summary>
    /// Localidades do Sistema
    /// </summary>
    // [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class LocalidadeController : BaseController
    {
        #region Parametros
        // private readonly IAuthService _authService;
        #endregion

        #region Construtor
        public LocalidadeController(IHttpContextAccessor httpContextAccessor, IUsuarioService usuarioService) : base(httpContextAccessor, usuarioService) { }
        #endregion

        #region Metodos
        /// <summary>
        /// Retorna a Localidade registrada no sistema
        /// </summary>
        /// <returns>Localidades registradas no sistema</returns>
        [HttpGet("{Id}")]
        public IActionResult Consultar(int Id, [FromServices] ILocalidadeService _localidadeService)
        {
            return Ok(_localidadeService.Consulta(Id));
        }
        /// <summary>
        /// Listar as localidades do sistema
        /// </summary>
        /// <returns>Localidades registradas no sistema</returns>
        [HttpGet("[action]")]
        public IActionResult Listar([FromServices] ILocalidadeService _localidadeService)
        {
            return Ok(_localidadeService.Listar());
        }
        /// <summary>
        /// Listar estados do sistema
        /// </summary>
        /// <returns>Localidades registradas no sistema</returns>
        [HttpGet("[action]")]
        public IActionResult ListarEstados([FromServices] ILocalidadeService _localidadeService)
        {
            return Ok(_localidadeService.ListarEstados());
        }
        /// <summary>
        /// Listar distritos do sistema
        /// </summary>
        /// <returns>Localidades registradas no sistema</returns>
        [HttpGet("[action]/{UFId}")]
        public IActionResult ListarMunicipios(int UFId, [FromServices] ILocalidadeService _localidadeService)
        {
            return Ok(_localidadeService.ListarMunicipios(UFId));
        }
        /// <summary>
        /// Listar distritos do sistema
        /// </summary>
        /// <returns>Localidades registradas no sistema</returns>
        [HttpGet("[action]/{municipioId}")]
        public IActionResult ListarDistritos(int municipioId, [FromServices] ILocalidadeService _localidadeService)
        {
            return Ok(_localidadeService.ListarDistritos(municipioId));
        }
        /// <summary>
        /// Nova Localidade
        /// </summary>
        /// <returns>Id da Localidade</returns>
        [HttpPost("[action]")]
        public IActionResult Novo([FromBody] LocalidadeModel localidade, [FromServices] ILocalidadeService _localidadeService)
        {
            return Ok(_localidadeService.Novo(localidade));
        }
        /// <summary>
        /// Atualizar Localidade
        /// </summary>
        /// <returns>Sucesso da operação</returns>
        [HttpPost("[action]")]
        public IActionResult Atualizar([FromBody] LocalidadeModel localidade, [FromServices] ILocalidadeService _localidadeService)
        {
            return Ok(_localidadeService.Atualizar(localidade));
        }
        #endregion
        #region Metodos
        /// <summary>
        /// Retorna a Localidade registrada no sistema
        /// </summary>
        /// <returns>Localidades registradas no sistema</returns>
        [HttpPost]
        public IActionResult Consultar(LocalidadeModel localidadeModel, [FromServices] ILocalidadeService _localidadeService)
        {
            return Ok(_localidadeService.Consulta(localidadeModel));
        }
        #endregion
    }
}