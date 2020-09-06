using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using NFCE.API.Interfaces.Repositories;
using NFCE.API.Models;
using NFCE.API.Models.Request;
using NFCE.API.Models.Response.Produto;
using NFCE.API.Models.Saldos;

namespace NFCE.API.Repositories
{
    public class ProdutoRepository : RepositoryBase<ProdutoModel>, IProdutoRepository
    {
        public ProdutoRepository(IConfiguration config) : base(config) { }
        public IEnumerable<ProdutoResponse> Listar(ProdutoListarRequest produtoListarRequest)
        {
            List<string> where = new List<string>();
            where.Add("1 = 1");

            if (!string.IsNullOrEmpty(produtoListarRequest.NomeProduto))
            {
                where.Add($"UPPER(COALESCE(p.Apelido, p.Nome)) LIKE UPPER('%{produtoListarRequest.NomeProduto}%')");
            }
            if (produtoListarRequest.IdCompra.HasValue)
            {
                //  Adiciona empresas perto
                where.Add($"e.id in (select e.id from nfcet_emissor e join nfcet_compras c on e.id_localidade = c.id_localidade where c.id = {produtoListarRequest.IdCompra} group by e.id)");
            }
            if (produtoListarRequest.IdProdutos != null && produtoListarRequest.IdProdutos.Count > 0)
            {
                where.Add($"p.id IN ({string.Join(",", produtoListarRequest.IdProdutos)})");
            }

            string sql = $@"
                SELECT
                    --  Produto
                    p.*,
                    1 as divisor,
                    --  Saldos
                    s.*,
                    1 as divisor,
                    --  Emissores
                    e.*
                FROM nfcet_produtos p
                JOIN nfcet_saldos s on s.id_produto = p.id
                JOIN (SELECT id_produto, id_emissor, MAX(data_insercao) as data_insercao FROM nfcet_saldos GROUP BY id_produto, id_emissor) _s on s.id_produto = _s.id_produto AND s.data_insercao = _s.data_insercao AND s.id_emissor = _s.id_emissor
                JOIN nfcet_emissor e on e.id = s.id_emissor
                WHERE
                    {string.Join(" AND ", where)}
            ";
            var produtoDictionary = new Dictionary<int, ProdutoResponse>();
            using (var con = GetConnection())
            {
                DynamicParameters parameters = new DynamicParameters();
                return con.Query<ProdutoResponse, SaldosModel, EmissorModel, ProdutoResponse>(
                    sql,
                    (produto, saldo, emissor) =>
                    {
                        ProdutoResponse produtoEntry;

                        if (!produtoDictionary.TryGetValue(produto.Id, out produtoEntry))
                        {
                            produtoEntry = produto;
                            produtoEntry.Saldo = new SaldosModel();
                            produtoDictionary.Add(produtoEntry.Id, produtoEntry);
                        }

                        // saldo.Emissor = emissor;
                        produtoEntry.Emissor = emissor;
                        produtoEntry.Saldo = saldo;

                        return produtoEntry;
                    },
                    param: parameters,
                    splitOn: "divisor"
                )
                .Distinct();
            }
        }
    }
}