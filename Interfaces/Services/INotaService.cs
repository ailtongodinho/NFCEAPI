using System.Collections.Generic;
using NFCE.API.Models;
using NFCE.API.Models.Request;
using NFCE.API.Models.Response.Nota;

namespace NFCE.API.Interfaces.Services
{
    public interface INotaService : IBaseService<NotaModel>
    {
        #region CRUD
        NotaModel Consultar(int Id);
        int Novo(NotaModel modelo);
        object Deletar(int Id);
        #endregion
        #region Metodos
        bool Favoritar(NotaRequest NotaRequest);
        NotaListarAgregadoResponse ListarTotais(int idUsuario, NotaListarRequest extracaoListarRequest);
        IEnumerable<NotaListarResponse> Ordenar(NotaOrdenarRequest notaOrdenarRequest);
        NotaAgregadoResponse Agregado();
        #endregion
    }
}