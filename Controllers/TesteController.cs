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
using NFCE.API.Services;

namespace NFCE.API.Controllers
{
    /// <summary>
    /// Autentificação de usuários
    /// </summary>
    // [Authorize]
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class TesteController : BaseController
    {
        #region Parametros
        #endregion

        #region Construtor
        public TesteController(IHttpContextAccessor httpContextAccessor, IUsuarioService usuarioService) : base(httpContextAccessor, usuarioService) { }
        #endregion

        #region Metodos
        [HttpPost("[action]")]
        public IActionResult Localidade([FromBody] EmissorModel model, [FromServices] IBGEService service)
        {
            return Ok(service.GetLocalidade(model));
        }
        #endregion
    }
}