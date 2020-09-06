using NFCE.API.Models;
using NFCE.API.Models.Response.Pagamento;

namespace NFCE.API.Interfaces.Services
{
    public interface IPagamentoService : IBaseService<PagamentoModel>
    {
        #region CRUD
        PagamentoModel Consultar(int Id);
        int Novo(PagamentoModel modelo);
        bool Deletar(int Id);
        #endregion
        #region Metodos
        PagamentoAgregadoResponse Agregado(int IdUsuario, int? IdNota = null);
        #endregion
    }
}