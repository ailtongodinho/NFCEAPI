using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NFCE.API.Interfaces.Services;
using NFCE.API.Models;

namespace NFCE.API.Controllers
{
    public class BaseController : ControllerBase
    {
        public UsuarioModel Usuario { get { return _usuarioService.Consulta(int.Parse(_httpContextAccessor.HttpContext.User.Identity.Name)); } }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUsuarioService _usuarioService;
        public BaseController(IHttpContextAccessor httpContextAccessor, IUsuarioService usuarioService)
        {
            _httpContextAccessor = httpContextAccessor;
            _usuarioService = usuarioService;
        }
    }
}