using NFCE.API.Models;

namespace NFCE.API.Interfaces
{
    public interface IExtracaoRepository
    {
        void Salvar(ExtracaoModel model);
    }
}