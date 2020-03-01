using System;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NFCE.API.Extensions;
using NFCE.API.Interfaces;
using NFCE.API.Models;

namespace NFCE.API.Controllers
{
    /// <summary>
    /// Autentificação de usuários
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        #region Parametros
        // private readonly IAuthService _authService;
        #endregion

        #region Construtor
        // public UsuarioController(IAuthService authService)
        // {
        //     _authService = authService;
        // }
        #endregion

        #region Metodos
        /// <summary>
        /// Login no sistema
        /// </summary>
        /// <returns>Token para o login no sistema</returns>
        [AllowAnonymous]
        [HttpPost("[action]")]
        public IActionResult Login([FromBody] AuthModel auth, [FromServices] IAuthService _authService)
        {
            ResponseModel response = new ResponseModel { Sucesso = true, HttpStatus = HttpStatusCode.OK };
            try
            {
                response.Objeto = _authService.Login(auth);
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
        /// Login no sistema
        /// </summary>
        /// <returns>Token para o login no sistema</returns>
        [Authorize]
        [HttpGet("[action]")]
        public IActionResult Listar([FromServices] IUsuarioService _usuarioService)
        {
            ResponseModel response = new ResponseModel { Sucesso = true, HttpStatus = HttpStatusCode.OK };
            try
            {
                response.Objeto = _usuarioService.Listar();
            }
            catch (Exception ex)
            {
                response.HttpStatus = HttpStatusCode.InternalServerError;
                response.Mensagem = ex.ToString();
                response.Sucesso = false;
            }
            return StatusCode((int)response.HttpStatus, response);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public IActionResult Novo([FromBody] UsuarioModel usuario, [FromServices] IUsuarioService _usuarioService)
        {
            ResponseModel response = new ResponseModel { Sucesso = true, HttpStatus = HttpStatusCode.Created };
            try
            {
                response.Mensagem = _usuarioService.Novo(usuario);
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