using NFCE.API.Models.Saldos;

namespace NFCE.API.Interfaces.Repositories
{
    public interface ISaldosRepository : IRepositoryBase<SaldosModel>
    {
        bool AtualizarSaldos(int idNota);
    }
}