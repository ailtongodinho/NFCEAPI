using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using NFCE.API.Interfaces.Repositories;
using NFCE.API.Models;
using NFCE.API.Models.Request;
using NFCE.API.Models.Saldos;

namespace NFCE.API.Repositories
{
    public class ComprasProdutoRepository : RepositoryBase<ComprasProdutoModel>, IComprasProdutoRepository
    {
        public ComprasProdutoRepository(IConfiguration config) : base(config) { }
        public bool DeletarPorCompra(int IdCompra)
        {
            string sql = "DELETE FROM X.{TABLE_NAME} X WHERE X.{IdCompra} = @IdCompra";
            sql = Formatar<ComprasProdutoModel>(sql, "X");
            using (var con = GetConnection())
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@IdCompra", IdCompra);
                return con.Execute(sql, parameters) > 0;
            }
        }
    }
}