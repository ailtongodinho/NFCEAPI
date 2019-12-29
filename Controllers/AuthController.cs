using Microsoft.AspNetCore.Mvc;
using NFCE.API.Interfaces;

namespace NFCE.API.Controllers
{
    /// <summary>
    /// Autentificação de usuários
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        #region Parametros
        private readonly IAuthService authService;
        #endregion

        #region Construtor
        public AuthController(IAuthService _authService)
        {
            authService = _authService;
        }
        #endregion

        #region Metodos
        /// <summary>
        /// Descrição da API
        /// </summary>
        /// <returns>Versão atual da API</returns>
        // [AllowAuth]
        
        [HttpGet]
        public IActionResult GetAction()
        {
            string teste = authService.Login(new Models.AuthModel { Login = "Godinho", Senha = "1234" });
            return Ok(teste);
        }
        #endregion

    }
}