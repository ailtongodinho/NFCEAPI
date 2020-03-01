using Microsoft.AspNetCore.Mvc;
using NFCE.API.Interfaces;
using NFCE.API.Models;
using Microsoft.AspNetCore.Authorization;
using NFCE.API.Helpers;
using System.Net;
using System;
using NFCE.API.Models.Request;

namespace NFCE.API.Controllers
{
    /// <summary>
    /// Autentificação de usuários
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ExtracaoController : ControllerBase
    {
        #region Parametros
        private readonly IAuthService authService;
        private readonly IExtracaoService ExtracaoService;
        #endregion

        #region Construtor
        public ExtracaoController(IAuthService _authService, IExtracaoService _ExtracaoService)
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
        [HttpPost("[action]")]
        public IActionResult Extrair([FromBody] ExtracaoRequestModel model)
        {
            ResponseModel response = new ResponseModel { Sucesso = true, HttpStatus = HttpStatusCode.OK };
            try
            {
                response.Objeto = ExtracaoService.ProcessarNFCE(model);
            }
            catch (Exception ex)
            {
                response.HttpStatus = HttpStatusCode.InternalServerError;
                response.Mensagem = ex.ToString();
                response.Sucesso = false;
            }
            return StatusCode((int)response.HttpStatus, response);
        }
        /// <summary>
        /// Listar informações sobre as notas fiscais
        /// </summary>
        /// <returns>Informações extraídas da NFCE</returns>
        [HttpGet("[action]")]    
        public IActionResult Listar(ExtracaoListarRequest extracaoListarRequest)
        {
            ResponseModel response = new ResponseModel { Sucesso = true, HttpStatus = HttpStatusCode.OK };
            try
            {
                response.Objeto = ExtracaoService.Listar(extracaoListarRequest);
            }
            catch (Exception ex)
            {
                response.HttpStatus = HttpStatusCode.InternalServerError;
                response.Mensagem = ex.ToString();
                response.Sucesso = false;
            }
            return StatusCode((int)response.HttpStatus, response);
        }
        #endregion

    }
}