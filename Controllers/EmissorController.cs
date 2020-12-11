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
    public class EmissorController : BaseController
    {
        #region Parametros
        private readonly IEmissorService _EmissorService;
        #endregion

        #region Construtor
        public EmissorController(IEmissorService EmissorService, IHttpContextAccessor httpContextAccessor, IUsuarioService usuarioService) : base(httpContextAccessor, usuarioService)
        {
            _EmissorService = EmissorService;
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
            return Ok(_EmissorService.Consultar(Id));
        }
        /// <summary>
        /// Deletar
        /// </summary>
        /// <returns>Verdadeiro ou Falso</returns>
        [HttpDelete("[action]/{Id}")]
        public ActionResult Deletar(int Id)
        {
            return Ok(_EmissorService.Deletar(Id));
        }
        /// <summary>
        /// Novo Registro
        /// </summary>
        /// <param name="modelo">Modelo de Emissor</param>
        /// <returns>Identificação do Registro adicionado</returns>
        [HttpPost]
        public ActionResult Novo(EmissorModel modelo)
        {
            return Ok(_EmissorService.Novo(modelo));
        }
        #endregion
        #region Metodos
        /// <summary>
        /// Agregado
        /// </summary>
        /// <param name="modelo">Modelo de Emissor Agregado</param>
        /// <param name="agregadoService">Serviço agregado</param>
        /// <returns>Modelo Agregado</returns>
        [HttpPost("[action]")]
        public ActionResult Agregado(EmissorAgregadoRequest modelo, [FromServices] IAgregadoService agregadoService)
        {
            return Ok(agregadoService.Emissor(Usuario.Id, modelo));
        }
        #endregion
    }
}