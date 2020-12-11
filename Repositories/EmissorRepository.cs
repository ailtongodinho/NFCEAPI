using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using NFCE.API.Interfaces.Repositories;
using NFCE.API.Models;
using NFCE.API.Models.Handler;
using NFCE.API.Models.Request;
using NFCE.API.Models.Response.Emissor;

namespace NFCE.API.Repositories
{
    public class EmissorRepository : RepositoryBase<EmissorModel>, IEmissorRepository
    {
        public EmissorRepository(IConfiguration config) : base(config)
        {
        }
        public int Adicionar(EmissorModel emissorModel)
        {
            int? id = Consultar(emissorModel)?.Id;
            if (!id.HasValue)
            {
                id = Insert(emissorModel);
            }
            return id.Value;
        }
        public EmissorModel Consultar(int EmissorId)
        {
            //  Verifica se o Emissor existe
            EmissorModel Emissor = GetById(EmissorId);
            if (Emissor == null) throw new HttpExceptionHandler("Emissor não existe");
            return Emissor;
        }
        public EmissorModel Consultar(EmissorModel emissorModel)
        {
            //  Verifica se o Emissor existe
            var Emissor = GetList(x =>
                x.CNPJ == emissorModel.CNPJ &&
                x.Distrito == emissorModel.Distrito &&
                x.Logradouro == emissorModel.Logradouro &&
                x.Municipio == emissorModel.Municipio &&
                x.Numero == emissorModel.Numero &&
                x.RazaoSocial == emissorModel.RazaoSocial &&
                x.UF == emissorModel.UF
            );
            return Emissor.Count() > 0 ? Emissor.First() : null;
        }
        public IEnumerable<EmissorAgregadoResponse> Agregado(int IdUsuario, EmissorAgregadoRequest emissorAgregadoRequest)
        {
            string sql = @"
            SELECT
                E.razao_social AS razaoSocial,
                MAX(E.nome_fantasia) AS nomeFantasia,
                E.cnpj,
                L.uf_nome as uf,
                E.municipio,
                E.distrito,
                E.cep,
                E.numero,
                E.logradouro,
                E.id,
                SUM(P.vlr_pago) AS SomatoriaTotal,
                SUM(P.troco) AS SomatoriaTroco,
                SUM(P.tributos) AS SomatoriaTributos,
                --SUM(I.qtd) AS Quantidade,
                COUNT(*) AS QuantidadeNotas,
                MAX(P.id) AS UltimoPagamento,
                MAX(C.id) AS UltimaNota
            FROM NFCET_CONTROLE C
            JOIN NFCET_EMISSOR E ON E.ID = C.ID_EMISSOR
            JOIN NFCET_PAG P ON C.ID = P.ID_CTRL
            JOIN NFCET_LOCALIDADE L ON L.ID = E.ID_LOCALIDADE
            --JOIN (SELECT _I.id_ctrl, COUNT(DISTINCT _I.codigo) AS QTD FROM NFCET_ITEM _I GROUP BY _I.id_ctrl) I ON C.ID = I.ID_CTRL
            WHERE
                C.ID_USR = @ID_USR
                AND C.emissao BETWEEN @DT_INI AND @DT_FIM
            GROUP BY
                E.razao_social,
                E.cnpj,
                L.uf_nome,
                E.municipio,
                E.distrito,
                E.cep,
                E.numero,
                E.logradouro,
                E.id
            ";

            using (var con = GetConnection())
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("DT_INI", emissorAgregadoRequest.DataInicial, DbType.DateTime);
                parameters.Add("DT_FIM", emissorAgregadoRequest.DataFinal, DbType.DateTime);
                parameters.Add("ID_USR", IdUsuario, DbType.Int32);
                return con.Query<EmissorAgregadoResponse>(sql, parameters);
            }
        }
    }
}