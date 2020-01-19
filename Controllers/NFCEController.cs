using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFCE.API.Interfaces;
using NFCE.API.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;

namespace NFCE.API.Controllers
{
    /// <summary>
    /// Autentificação de usuários
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class NFCEController : ControllerBase
    {
        #region Parametros
        private readonly IAuthService authService;
        private readonly IExtracaoService ExtracaoService;
        #endregion

        #region Construtor
        public NFCEController(IAuthService _authService, IExtracaoService _ExtracaoService)
        {
            authService = _authService;
            ExtracaoService = _ExtracaoService;
        }
        #endregion

        #region Metodos
        /// <summary>
        /// Retorna informações da NFCE de acordo com a URL
        /// </summary>
        /// <param name="model">URL de acesso a NFCE. Mais informações em http://nfce.encat.org/desenvolvedor/</param>
        /// <returns>Infomações extraídas da NFCE</returns>
        [Authorize("Bearer")]
        [HttpPost("[action]")]
        public IActionResult GetInfo([FromBody] ExtracaoRequestModel model)
        {
            if (ModelState.IsValid)
                return Ok(ExtracaoService.ProcessarNFCE(model.Url));
            else
                return BadRequest(ModelState);
        }
        #endregion

    }
}