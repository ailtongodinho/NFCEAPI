using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NFCE.API.Interfaces.Services;
using NFCE.API.Models;
using NFCE.API.Models.Request;
using NFCE.API.Models.Request.Usuario;
using NFCE.API.Services;

namespace NFCE.API.Controllers
{
    /// <summary>
    /// Autentificação de usuários
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : BaseController
    {
        #region Parametros
        // private readonly IAuthService _authService;
        #endregion

        #region Construtor
        public UsuarioController(IHttpContextAccessor httpContextAccessor, IUsuarioService usuarioService) : base(httpContextAccessor, usuarioService) { }
        #endregion

        #region Metodos
        /// <summary>
        /// Login no sistema
        /// </summary>
        /// <returns>Token para o login no sistema</returns>
        [Authorize]
        [HttpGet]
        public IActionResult Get([FromServices] IUsuarioService _usuarioService)
        {
            return Ok(Usuario);
        }
        /// <summary>
        /// Login no sistema
        /// </summary>
        /// <returns>Token para o login no sistema</returns>
        [AllowAnonymous]
        [HttpPost("[action]")]
        public IActionResult Login(AuthModel auth, [FromServices] IAuthService _authService)
        {
            return Ok(_authService.Login(auth));
        }

        /// <summary>
        /// Login no sistema
        /// </summary>
        /// <returns>Token para o login no sistema</returns>
        [Authorize]
        [HttpGet("[action]")]
        public IActionResult Listar([FromServices] IUsuarioService _usuarioService)
        {

            return Ok(_usuarioService.Listar());
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public IActionResult Novo([FromBody] UsuarioModel usuario, [FromServices] IUsuarioService _usuarioService)
        {
            return Ok(_usuarioService.Novo(usuario));
        }
        [Authorize]
        [HttpPost("[action]")]
        public IActionResult Atualizar([FromBody] UsuarioModel usuario, [FromServices] IUsuarioService _usuarioService)
        {
            return Ok(_usuarioService.Atualizar(usuario));
        }
        #endregion
        #region Metodos
        [Authorize]
        [HttpPost("[action]")]
        public IActionResult AlterarSenha([FromBody] AuthAlterarSenha authAlterarSenha, [FromServices] IAuthService _authService)
        {
            return Ok(_authService.AlterarSenha(Usuario.Id, authAlterarSenha));
        }
        #endregion
    }
}