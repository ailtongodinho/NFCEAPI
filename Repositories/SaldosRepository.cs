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
                --  Itens a serem incluidos
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
                    c.id = 37
                ;
                --  Deletar itens do mesmo emissor com o valor igual
                DELETE FROM itens i USING nfcet_saldos s
                WHERE
                    s.id_produto = i.id_produto
                    and s.id_emissor = i.id_emissor
                    and s.vlr_un = i.vlr_un;
                --  Atualizar campo 'ultimo'
                UPDATE nfcet_saldos s SET ultimo = 0
                FROM itens i
                WHERE
                    s.id_produto = i.id_produto
                    and s.id_emissor = i.id_emissor;
                --  Inserir itens
                INSERT INTO nfcet_saldos (id_produto, id_emissor, vlr_un, data_insercao, ultimo)
                SELECT id_produto, id_emissor, vlr_un, data_insercao, 1 FROM itens;
            ";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@id_ctrl", idNota, DbType.Int32);
            return GetConnection().Execute(sql, parameters) > 0;
        }
    }
}