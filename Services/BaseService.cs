using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NFCE.API.Interfaces.Repositories;
using NFCE.API.Interfaces.Services;
using NFCE.API.Models;

namespace NFCE.API.Services
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
    {
        public UsuarioModel Usuario
        {
            get
            {
                try
                {
                    return _usuarioService.Consulta(int.Parse(_httpContextAccessor.HttpContext.User.Identity.Name));
                }
                catch
                {
                    return null;
                }
            }
        }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUsuarioService _usuarioService;
        public BaseService() { }
        public BaseService(IHttpContextAccessor httpContextAccessor, IUsuarioService usuarioService)
        {
            _httpContextAccessor = httpContextAccessor;
            _usuarioService = usuarioService;
        }
    }
}