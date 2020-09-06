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
    public class ComprasController : BaseController
    {
        #region Parametros
        private readonly IComprasService _ComprasService;
        #endregion

        #region Construtor
        public ComprasController(IComprasService ComprasService, IHttpContextAccessor httpContextAccessor, IUsuarioService usuarioService) : base(httpContextAccessor, usuarioService)
        {
            _ComprasService = ComprasService;
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
    }
}