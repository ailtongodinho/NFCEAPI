using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using Dapper.FluentMap.Dommel.Mapping;
using Dapper.FluentMap.Mapping;
using Dommel;
using Microsoft.Extensions.Configuration;
using NFCE.API.Extensions;
using NFCE.API.Interfaces;
using NFCE.API.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace NFCE.API.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private string GetConnectionString => _config.GetConnectionString(_config.GetValue<string>("Sql:Connection"));
        private readonly IConfiguration _config;

        public BaseRepository(IConfiguration config)
        {
            _config = config;
            if (FluentMapper.EntityMaps.IsEmpty)
            {
                FluentMapper.Initialize(config =>
                {
                    //  Extração
                    config.AddMap(new ExtracaoMapping());
                    config.AddMap(new ExtracaoEmissorMapping());
                    config.AddMap(new ExtracaoItemMapping());
                    config.AddMap(new ExtracaoPagamentoMapping());
                    //  Usuário
                    config.AddMap(new UsuarioMapping());
                    config.AddMap(new AuthMapping());
                    config.ForDommel();
                });
            }
        }

        public bool UpdateGenerico(TEntity entity, object atualizarCampos = null, object filtro = null)
        {
            bool retorno = false;

            var entityMap = FluentMapper.EntityMaps.First(x => x.Key == entity.GetType());
            var dommelEntityMap = (IDommelEntityMap)entityMap.Value;
            Dictionary<string, DommelPropertyMap> dicionario = new Dictionary<string, DommelPropertyMap>();
            dommelEntityMap.PropertyMaps
                .ToList().ForEach(x => dicionario.Add(x.ColumnName, ((DommelPropertyMap)x)));
            //  Lista para SET
            List<string> sqlSet = new List<string>();
            //  Lista para WHERE
            List<string> sqlWhere = new List<string>();
            //  Lista de Campos
            List<string> campos;
            //  Parâmetros do SQL
            DynamicParameters parameters = new DynamicParameters();
            //  Populando SET
            var linq = dicionario.Where(x => !x.Value.Key && !x.Value.Identity);
            //  Somente atualizar os seguintes campos
            if (atualizarCampos != null)
            {
                campos = PropertyExtension.RetornaNomePropriedade(atualizarCampos);
                linq = linq.Where(x => campos.Contains(x.Value.PropertyInfo.Name));
            }

            if (linq.Count() > 0)
            {
                foreach (var item in linq)
                {
                    var info = item.Value.PropertyInfo;
                    var dbType = GetDbType(info.PropertyType);
                    parameters.Add(item.Key, info.GetValue(entity), dbType);
                    sqlSet.Add($"{item.Key} = @{item.Key}");
                }
                //  Processar somente se conter WHERE
                linq = dicionario.Where(x => x.Value.Key || x.Value.Identity);
                //  Somente filtrar pelos seguintes campos
                if (filtro != null)
                {
                    campos = PropertyExtension.RetornaNomePropriedade(filtro);
                    linq = linq.Where(x => campos.Contains(x.Value.PropertyInfo.Name));
                }

                if (linq.Count() > 0)
                {
                    foreach (var item in linq)
                    {
                        var info = item.Value.PropertyInfo;
                        var dbType = GetDbType(info.PropertyType);
                        parameters.Add(item.Key, info.GetValue(entity), dbType);
                        sqlWhere.Add($"{item.Key} = @{item.Key}");
                    }
                    //  Cria SQL
                    string set = string.Join(",", sqlSet);
                    string where = string.Join(" AND ", sqlWhere);
                    string sql = $"UPDATE X SET {set} FROM {dommelEntityMap.TableName} X WHERE {where}";
                    //  Executa
                    using (SqlConnection con = new SqlConnection(GetConnectionString))
                    {
                        retorno = con.Execute(sql, parameters) > 0;
                    }
                }
            }
            return retorno;
        }

        public IEnumerable<TEntity> GetListCustom(TEntity entity, object where = null, object groupBy = null, object orderBy = null, int? top = null, bool orderByDesc = false)
        {
            IEnumerable<TEntity> retorno = null;
            var entityMap = FluentMapper.EntityMaps.First(x => x.Key == entity.GetType());
            var dommelEntityMap = (IDommelEntityMap)entityMap.Value;
            Dictionary<string, DommelPropertyMap> dicionario = new Dictionary<string, DommelPropertyMap>();
            dommelEntityMap.PropertyMaps
                .ToList().ForEach(x => dicionario.Add(x.ColumnName, ((DommelPropertyMap)x)));
            //  Lista para WHERE
            List<string> sqlWhere = new List<string>();
            //  Lista para GROUP BY
            List<string> sqlGroup = new List<string>();
            //  Lista para ORDER BY
            List<string> sqlOrder = new List<string>();
            //  Lista de Campos
            List<string> campos;
            //  Parâmetros do SQL
            DynamicParameters parameters = new DynamicParameters();
            #region Where
            //  Monta lista do Where utilizando o mapeamento do Dapper
            if (where != null)
            {
                campos = PropertyExtension.RetornaNomePropriedade(where);
                foreach (var item in dicionario.Where(x => campos.Contains(x.Value.PropertyInfo.Name)))
                {
                    var info = item.Value.PropertyInfo;
                    var dbType = GetDbType(info.PropertyType);
                    parameters.Add(item.Key, info.GetValue(entity), dbType);
                    sqlWhere.Add($"{item.Key} = @{item.Key}");
                }
            }
            #endregion
            #region Group
            //  Monta lista do Group By utilizando o mapeamento do Dapper
            if (groupBy != null)
            {
                campos = PropertyExtension.RetornaNomePropriedade(groupBy);
                foreach (var item in dicionario.Where(x => campos.Contains(x.Value.PropertyInfo.Name)))
                {
                    sqlGroup.Add(item.Key);
                }
            }
            #endregion
            #region Order
            //  Monta lista do Order By utilizando o mapeamento do Dapper
            if (orderBy != null)
            {
                campos = PropertyExtension.RetornaNomePropriedade(orderBy);
                foreach (var item in dicionario.Where(x => campos.Contains(x.Value.PropertyInfo.Name)))
                {
                    sqlOrder.Add(item.Key);
                }
            }
            #endregion
            //  String para o Where
            string _where = sqlWhere.Count() > 0 ? $"WHERE {(string.Join(" AND ", sqlWhere))}" : string.Empty;
            //  String para o Group By
            string _group = sqlGroup.Count() > 0 ? $"GROUP BY {(string.Join(",", sqlGroup))}" : string.Empty;
            //  String para o Order By
            string _order = sqlOrder.Count() > 0 ? $"ORDER BY {(string.Join(",", sqlOrder))} {(orderByDesc ? "DESC" : "ASC")}" : string.Empty;
            //  String para o Top
            string _top = top.HasValue ? $"TOP {top.Value}" : string.Empty;
            //  String Final
            string sql = $"SELECT {_top} FROM {dommelEntityMap.TableName} WITH(NOLOCK) {_where} {_group} {_order}";

            using (SqlConnection con = new SqlConnection(GetConnectionString))
            {
                retorno = con.Query<TEntity>(sql, parameters);
            }

            return retorno;
        }

        public bool Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString))
            {
                return con.Delete(id);
            }
        }

        public TEntity GetById(int id)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString))
            {
                return con.Get<TEntity>(id);
            }
        }

        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString))
            {
                return con.Select<TEntity>(predicate);
            }
        }

        public int Insert(TEntity entity)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString))
            {
                return int.Parse(con.Insert<TEntity>(entity).ToString());
            }
        }

        public bool Update(TEntity entity)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString))
            {
                return con.Update(entity);
            }
        }

        public IEnumerable<TEntity> GetAll()
        {
            using (var con = new SqlConnection(GetConnectionString))
            {
                return con.GetAll<TEntity>();
            }
        }

        private static DbType GetDbType(Type runtimeType)
        {
            var nonNullableType = Nullable.GetUnderlyingType(runtimeType);
            if (nonNullableType != null)
            {
                runtimeType = nonNullableType;
            }
            var templateValue = (Object)null;
            if (!runtimeType.IsClass)
            {
                templateValue = Activator.CreateInstance(runtimeType);
            }

            var sqlParameter = new SqlParameter(parameterName: String.Empty, value: templateValue);

            return sqlParameter.DbType;
        }
    }
}