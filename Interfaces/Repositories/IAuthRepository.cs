using NFCE.API.Models;

namespace NFCE.API.Interfaces.Repositories
{
    public interface IAuthRepository : IRepositoryBase<AuthModel>
    {
        AuthModel Consulta(AuthModel model);
    }
}