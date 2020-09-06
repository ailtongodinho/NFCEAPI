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
    /// Configurações do APP
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ConfiguracoesController : BaseController
    {
        #region Parametros
        #endregion

        #region Construtor
        public ConfiguracoesController(IHttpContextAccessor httpContextAccessor, IUsuarioService usuarioService) : base(httpContextAccessor, usuarioService)
        {
        }
        #endregion
        #region Métodos
        [HttpGet]
        public IActionResult GetConfiguracoes([FromServices] ConfiguracaoModel configuracao)
        {
            return Ok(new
            {
                picker = new
                {
                    sexo = configuracao.Opcoes,
                    ordernar = configuracao.Ordenar
                }
            });
        }
        #endregion
    }
}