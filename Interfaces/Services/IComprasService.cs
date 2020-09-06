using System.Collections.Generic;
using NFCE.API.Models;

namespace NFCE.API.Interfaces.Services
{
    public interface IComprasService : IBaseService<ComprasModel>
    {
        #region CRUD
        ComprasModel Consultar(int Id);
        int Novo(ComprasModel modelo);
        bool Deletar(int Id);
        #endregion
        #region Metodos
        IEnumerable<ComprasModel> Listar(int idUsuario);
        #endregion
    }
}