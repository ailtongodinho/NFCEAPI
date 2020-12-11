using NFCE.API.Models;
using NFCE.API.Models.Response.Item;

namespace NFCE.API.Interfaces.Repositories
{
    public interface IItemRepository : IRepositoryBase<ItemModel>
    {
        int ConsultaQuantidade(int idControle);
        decimal ConsultaValorTotal(int idControle);
        ItemModel Consultar(int itemId);
        ItemAgregadoResponse Agregado(int IdUsuario, int? IdNota = null);
    }
}