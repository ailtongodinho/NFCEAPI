using System.Collections.Generic;
using NFCE.API.Models;
using NFCE.API.Models.Request;
using NFCE.API.Models.Response.Emissor;

namespace NFCE.API.Interfaces.Services
{
    public interface IEmissorService : IBaseService<EmissorModel>
    {
        #region CRUD
        EmissorModel Consultar(int Id);
        int Novo(EmissorModel modelo);
        bool Deletar(int Id);
        IEnumerable<EmissorAgregadoResponse> Agregado(int IdUsuario, EmissorAgregadoRequest emissorAgregadoRequest);
        #endregion
    }
}