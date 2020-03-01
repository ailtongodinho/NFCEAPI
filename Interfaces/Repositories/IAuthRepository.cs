using NFCE.API.Models;

namespace NFCE.API.Interfaces
{
    public interface IAuthRepository : IBaseRepository<AuthModel>
    {
        AuthModel Consulta(AuthModel model);
    }
}