using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using NFCE.API.Interfaces.Repositories;
using NFCE.API.Models;
using NFCE.API.Models.Handler;
using NFCE.API.Models.Request;
using NFCE.API.Models.Response.Pagamento;

namespace NFCE.API.Repositories
{
    public class PagamentoRepository : RepositoryBase<PagamentoModel>, IPagamentoRepository
    {
        public PagamentoRepository(IConfiguration config) : base(config)
        {
        }
        public PagamentoModel Consultar(int PagamentoId)
        {
            //  Verifica se o Pagamento existe
            PagamentoModel Pagamento = GetById(PagamentoId);
            if (Pagamento == null) throw new HttpExceptionHandler("Pagamento não existe");
            return Pagamento;
        }
        public PagamentoAgregadoResponse Agregado(int IdUsuario, int? IdNota = null)
        {
            string sql = @"
            SELECT 
                SUM(TRIBUTOS) AS TotalTributos, 
                SUM(VLR_PAGO) AS Total, 
                SUM(TROCO) AS TotalTroco,
                COUNT(*) AS Quantidade
            FROM NFCET_PAG P
            JOIN NFCET_CONTROLE C ON C.ID_USR = @ID_USR AND P.ID_CTRL = C.ID
            WHERE
                C.ID = COALESCE(@ID, C.ID)
            ";

            using (var con = GetConnection())
            {
                return con.QuerySingle<PagamentoAgregadoResponse>(sql, new { @ID_USR = IdUsuario, @ID = IdNota ?? null });
            }
        }
    }
}