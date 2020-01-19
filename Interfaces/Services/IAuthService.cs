using NFCE.API.Models;

namespace NFCE.API.Interfaces
{
    public interface IAuthService : IBaseService<int>
    {
        object Login(AuthModel usuario);
    }
}