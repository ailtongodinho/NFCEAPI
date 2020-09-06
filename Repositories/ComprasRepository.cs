using System.Collections.Generic;
using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using NFCE.API.Interfaces.Repositories;
using NFCE.API.Models;

namespace NFCE.API.Repositories
{
    public class ComprasRepository : RepositoryBase<ComprasModel>, IComprasRepository
    {
        public ComprasRepository(IConfiguration config) : base(config) { }

        public IEnumerable<ComprasModel> Listar(int idUsuario)
        {
            string sql = @"
                SELECT 
                    X.*
                    , SUM(Z.vlr_un) Total 
                --  COMPRAS
                FROM X.{TABLE_NAME} X
                --  COMPRAS e PRODUTOS
                JOIN Y.{TABLE_NAME} Y ON Y.{Id} = X.{Id}
                --  ULTIMOS SALDOS
                JOIN NFCEV_SALDOS Z ON Z.ID_PRODUTO = Y.{IdProduto}
            ";

            sql = Formatar<ComprasModel>(sql, "X");
            sql = Formatar<ComprasModel>(sql, "Y");

            using (var con = GetConnection())
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ID_USR", idUsuario, DbType.Int32);
                return con.Query<ComprasModel>(sql, parameters);
            }
        }
    }
}