using NFCE.API.Models;
using NFCE.API.Models.Request;
using NFCE.API.Models.Request.Usuario;
using NFCE.API.Models.Response.Usuario;

namespace NFCE.API.Interfaces.Services
{
    public interface IAuthService : IBaseService<int>
    {
        UsuarioLoginResponse Login(AuthModel usuario);
        UsuarioLoginResponse AlterarSenha(int idUsuario, AuthAlterarSenha authAlterarSenha);
    }
}