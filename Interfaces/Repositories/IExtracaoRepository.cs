using System.Collections.Generic;
using NFCE.API.Models;
using NFCE.API.Models.Request;

namespace NFCE.API.Interfaces
{
    public interface IExtracaoRepository : IBaseRepository<ExtracaoModel>
    {
        void Salvar(ExtracaoModel model);
        IEnumerable<ExtracaoModel> Listar(int IdUsuario, ExtracaoListarRequest extracaoListarRequest);
    }
}