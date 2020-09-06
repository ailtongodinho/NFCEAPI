using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using NFCE.API.Interfaces.Repositories;
using NFCE.API.Models.Saldos;

namespace NFCE.API.Repositories
{
    public class SaldosRepository : RepositoryBase<SaldosModel>, ISaldosRepository
    {
        public SaldosRepository(IConfiguration config) : base(config)
        {

        }
        public bool AtualizarSaldos(int idNota)
        {
            string sql = @"
                CREATE TEMP TABLE itens AS
                SELECT
                    i.id,
                    i.id_produto,
                    c.id_emissor,
                    i.vlr_un,
                    c.data_insercao
                FROM nfcet_controle c
                JOIN nfcet_item i on i.id_ctrl = c.id
                WHERE
                    c.id = @id_ctrl
                ;
                INSERT INTO nfcet_saldos (id_produto, id_emissor, vlr_un, data_insercao)
                SELECT id_produto, id_emissor, vlr_un, data_insercao FROM itens;
            ";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@id_ctrl", idNota, DbType.Int32);
            return GetConnection().Execute(sql, parameters) > 0;
        }
    }
}