using System.Collections.Generic;
using NFCE.API.Models;
using NFCE.API.Models.Request;
using NFCE.API.Models.Response.Item;

namespace NFCE.API.Interfaces.Services
{
    public interface IItemService : IBaseService<ItemModel>
    {
        #region CRUD
        ItemModel Consultar(int Id);
        IEnumerable<ItemModel> ConsultarPelaNota(int idNota);
        int Novo(ItemModel modelo);
        bool Deletar(int Id);
        #endregion
        #region Metodos
        ItemAgregadoResponse Agregado(int IdUsuario, int? IdNota = null);
        #endregion
    }
}