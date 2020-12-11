using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using NFCE.API.Interfaces.Repositories;
using NFCE.API.Models;
using NFCE.API.Models.IBGE;

namespace NFCE.API.Repositories
{
    public class LocalidadeRepository : RepositoryBase<LocalidadeModel>, ILocalidadeRepository
    {
        public LocalidadeRepository(IConfiguration config) : base(config)
        {

        }
        public List<IbgeEstado> ListarEstados()
        {
            List<IbgeEstado> estados = new List<IbgeEstado>();
            string sql = @"
                SELECT 
                    REGIAO_ID, REGIAO_SIGLA, REGIAO_NOME, 
                    UF_ID, UF_SIGLA, UF_NOME
                FROM NFCET_LOCALIDADE 
                GROUP BY
                    REGIAO_ID, REGIAO_SIGLA, REGIAO_NOME,
                    UF_ID, UF_SIGLA, UF_NOME
            ";

            using (var con = GetConnection())
            {
                var lista = con.Query<LocalidadeModel>(sql);
                foreach (var item in lista)
                {
                    estados.Add(new IbgeEstado
                    {
                        Id = item.UFId,
                        Nome = item.UFNome,
                        Sigla = item.UFSigla,
                        Regiao = new IbgeModel
                        {
                            Id = item.RegiaoId,
                            Nome = item.RegiaoNome,
                            Sigla = item.RegiaoSigla
                        }
                    });
                }
            }
            return estados;
        }
        public List<IbgeMunicipio> ListarMunicipios(int uFId)
        {
            List<IbgeMunicipio> estados = new List<IbgeMunicipio>();
            string sql = @"
                SELECT
                    MAX(ID) as ID,
                    MESORREGIAO_ID, MESORREGIAO_NOME,
                    MICROREGIAO_ID, MICROREGIAO_NOME,
                    MUNICIPIO_ID, MUNICIPIO_NOME
                FROM NFCET_LOCALIDADE 
                WHERE
                    UF_ID = @UF_ID
                GROUP BY 
                    MESORREGIAO_ID, MESORREGIAO_NOME,
                    MICROREGIAO_ID, MICROREGIAO_NOME,
                    MUNICIPIO_ID, MUNICIPIO_NOME
            ";

            using (var con = GetConnection())
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UF_ID", uFId, DbType.Int32);
                var lista = con.Query<LocalidadeModel>(sql, parameters);
                foreach (var item in lista)
                {
                    estados.Add(new IbgeMunicipio
                    {
                        Id = item.MunicipioId,
                        IdLocalidade = item.Id,
                        Nome = item.MunicipioNome,
                        Microrregiao = new IbgeMicrorregiao
                        {
                            Id = item.MicrorregiaoId,
                            Nome = item.MicrorregiaoNome,
                            Mesorregiao = new IbgeMesorregiao {
                                Id = item.MesorregiaoId,
                                Nome = item.MesorregiaoNome
                            }
                        }
                    });
                }
            }
            return estados;
        }
        public List<IbgeDistrito> ListarDistritos(int municipioId)
        {
            List<IbgeDistrito> distritos = new List<IbgeDistrito>();
            string sql = @"
                SELECT
                    MAX(ID) as ID,
                    MUNICIPIO_ID, MUNICIPIO_NOME, 
                    DISTRITO_ID, DISTRITO_NOME
                FROM NFCET_LOCALIDADE 
                WHERE
                    MUNICIPIO_ID = @MUNICIPIO_ID
                GROUP BY 
                    MUNICIPIO_ID, MUNICIPIO_NOME, 
                    DISTRITO_ID, DISTRITO_NOME
            ";

            using (var con = GetConnection())
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MUNICIPIO_ID", municipioId, DbType.Int32);
                var lista = con.Query<LocalidadeModel>(sql, parameters);
                foreach (var item in lista)
                {
                    distritos.Add(new IbgeDistrito
                    {
                        Id = item.DistritoId,
                        IdLocalidade = item.Id,
                        Nome = item.DistritoNome,
                        Municipio = new IbgeMunicipio
                        {
                            Id = item.MunicipioId,
                            Nome = item.MunicipioNome
                        }
                    });
                }
            }
            return distritos;
        }
        
    }
}