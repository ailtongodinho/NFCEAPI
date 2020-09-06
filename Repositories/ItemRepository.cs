using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using NFCE.API.Interfaces.Repositories;
using NFCE.API.Models;
using NFCE.API.Models.Handler;
using NFCE.API.Models.Response.Item;

namespace NFCE.API.Repositories
{
    public class ItemRepository : RepositoryBase<ItemModel>, IItemRepository
    {
        public ItemRepository(IConfiguration config) : base(config)
        {
        }
        public int ConsultaQuantidade(int idControle)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ID", idControle, DbType.Int32);
            return GetConnection().QuerySingle<int>(
                Formatar<ItemModel>("SELECT COUNT(*) FROM X.{TABLE_NAME} X WHERE X.{IdControle} = @ID", "X"),
                parameters
            );
        }
        public decimal ConsultaValorTotal(int idControle)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ID", idControle, DbType.Int32);
            return GetConnection().QuerySingle<decimal>(
                Formatar<ItemModel>("SELECT SUM(X.{ValorTotal}) FROM X.{TABLE_NAME} X WHERE X.{IdControle} = @ID", "X"),
                parameters
            );
        }
        public ItemModel Consultar(int itemId)
        {
            //  Verifica se o item existe
            ItemModel item = GetById(itemId);
            if (item == null) throw new HttpExceptionHandler(
                mensagemUsuario: "Item não existe",
                innerException: null
            );
            return item;
        }
        public ItemAgregadoResponse Agregado(int IdUsuario, int? IdNota = null)
        {
            string sql = @"
            SELECT
                SUM(VLR_TOT) AS Total,
                SUM(QTD) AS Quantidade
            FROM NFCET_ITEM I
            JOIN NFCET_CONTROLE C ON C.ID_USR = @ID_USR AND I.ID_CTRL = C.ID
            WHERE
                C.ID = COALESCE(@ID, C.ID)
            ";

            using (var con = GetConnection())
            {
                return con.QuerySingle<ItemAgregadoResponse>(sql, new { @ID_USR = IdUsuario, @ID = IdNota ?? null });
            }
        }
    }
}