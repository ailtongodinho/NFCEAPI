using System.Collections.Generic;
using NFCE.API.Models;
using NFCE.API.Models.Request;

namespace NFCE.API.Interfaces.Repositories
{
    public interface INotaRepository : IRepositoryBase<NotaModel>
    {
        int Novo(NotaModel modelo);
        NotaModel Consultar(int Id);
        IEnumerable<NotaModel> Listar(int IdUsuario, NotaListarRequest extracaoListarRequest);
    }
}