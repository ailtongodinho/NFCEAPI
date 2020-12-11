using System.Collections.Generic;
using NFCE.API.Models;

namespace NFCE.API.Interfaces.Repositories
{
    public interface IComprasRepository : IRepositoryBase<ComprasModel>
    {
        IEnumerable<ComprasModel> Listar(int idUsuario);
    }
}