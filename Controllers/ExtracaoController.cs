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
    public class ExtracaoController : BaseController
    {
        #region Parametros
        #endregion

        #region Construtor
        public ExtracaoController(IHttpContextAccessor httpContextAccessor, IUsuarioService usuarioService) : base(httpContextAccessor, usuarioService) { }
        #endregion

        #region Metodos
        /// <summary>
        /// Retorna informações da NFCE de acordo com a URL
        /// </summary>
        /// <param name="model">URL de acesso a NFCE. Mais informações em http://nfce.encat.org/desenvolvedor/</param>
        /// <param name="extracaoService">Serviço</param>
        /// <returns>Infomações extraídas da NFCE</returns>
        [HttpPost("[action]")]
        public IActionResult Extrair([FromBody] ExtracaoRequest model, [FromServices] IExtracaoService extracaoService)
        {
            return Ok(extracaoService.Processar(Usuario.Id, model));
        }
        #endregion
    }
}