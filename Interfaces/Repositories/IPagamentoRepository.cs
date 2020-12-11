using NFCE.API.Models;
using NFCE.API.Models.Response.Pagamento;

namespace NFCE.API.Interfaces.Repositories
{
    public interface IPagamentoRepository : IRepositoryBase<PagamentoModel>
    {
        PagamentoModel Consultar(int PagamentoId);
        PagamentoAgregadoResponse Agregado(int IdUsuario, int? IdNota = null);
    }
}