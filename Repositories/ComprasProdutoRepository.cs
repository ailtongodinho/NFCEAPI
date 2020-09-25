using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using NFCE.API.Interfaces.Repositories;
using NFCE.API.Models;
using NFCE.API.Models.Request;
using NFCE.API.Models.Response.ComprasProduto;
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
        public IEnumerable<ComprasProdutoCompararResponse> CompararEmissores(int idCompra)
        {
            string sql = @"
                SELECT 
                    p.*,
                    1 as divisor,
                    s.*
                FROM nfcet_saldos s
                JOIN nfcet_compras_produto cp ON
                    cp.id_produto = s.id_produto
                JOIN nfcet_produtos p ON
                    p.id = s.id_produto
                WHERE
                    cp.id_compra = @IdCompra
                    and s.ultimo = 1
                order by s.id_produto
            ";
            var comprasDictionary = new Dictionary<int, ComprasProdutoCompararResponse>();
            using (var con = GetConnection())
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@IdCompra", idCompra);
                return con.Query<ProdutoModel, SaldosModel, ComprasProdutoCompararResponse>(sql,
                    (produto, saldo) =>
                    {
                        return new ComprasProdutoCompararResponse()
                        {
                            Produto = produto,
                            Saldo = saldo
                        };
                    },
                    param: parameters,
                    splitOn: "divisor"
                );
            }
        }
    }
}