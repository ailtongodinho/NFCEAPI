using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using Dapper.FluentMap.Dommel.Mapping;
using Dommel;
using Microsoft.Extensions.Configuration;
using NFCE.API.Extensions;
using NFCE.API.Interfaces.Repositories;
using NFCE.API.Mapping;
using System;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace NFCE.API.Repositories
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        public IConfiguration GetConfiguration { get; set; }
        private readonly ConnectionRepository _connectionRepository;
        private string WhichConnection => GetConfiguration.GetValue<string>("Sql:Connection");
        private string GetConnectionString => GetConfiguration.GetConnectionString(WhichConnection);
        public IDommelEntityMap EntityMap { get; set; }
        private NpgsqlConnection _connection { get; set; }
        public RepositoryBase(IConfiguration config)
        {
            GetConfiguration = config;
            // _connectionRepository = new ConnectionRepository<NpgsqlConnection>(config);
            _connectionRepository = new ConnectionRepository(config);
            if (FluentMapper.EntityMaps.IsEmpty)
            {
                FluentMapper.Initialize(config =>
                {
                    //  Extração
                    config.AddMap(new NotaMapping());
                    config.AddMap(new EmissorMapping());
                    config.AddMap(new ItemMapping());
                    config.AddMap(new PagamentoMapping());
                    //  Usuário
                    config.AddMap(new UsuarioMapping());
                    config.AddMap(new AuthMapping());
                    //  Compras
                    config.AddMap(new ComprasMapping());
                    config.AddMap(new ComprasProdutoMapping());
                    //  Produto
                    config.AddMap(new ProdutoMapping());
                    //  Localidade
                    config.AddMap(new LocalidadeMapping());
                    //  Saldos
                    config.AddMap(new SaldosMapping());
                    config.ForDommel();
                });
            }
            SetEntityMap();
        }
        #region Connection
        public NpgsqlConnection GetConnection(bool restaurar = false)
        {
            if (ConnectionRepository.Transaction != null)
            {
                _connection = ConnectionRepository.Connection;
                if (_connection.State != ConnectionState.Open) _connection.Open();
            }
            else
            {
                if (_connection == null || restaurar)
                {
                    _connection = _connectionRepository.GetConnection();
                }
                // if (_connection.State != ConnectionState.Open) _connection.Open();
            }
            return _connection;
        }
        public void SetConnection(NpgsqlConnection connection)
        {
            _connection = connection ?? GetConnection(restaurar: true);
        }
        public void OpenTransaction()
        {
            _connectionRepository.OpenTransaction();
        }
        public void CloseTransaction(bool commit)
        {
            _connectionRepository.CloseTransaction(commit);
        }
        #endregion
        public bool UpdateGenerico(TEntity entity, object atualizarCampos = null, object filtro = null)
        {
            bool retorno = false;

            Dictionary<string, DommelPropertyMap> dicionario = new Dictionary<string, DommelPropertyMap>();
            EntityMap.PropertyMaps
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
                    string sql = $"UPDATE {EntityMap.TableName} SET {set} WHERE {where}";
                    //  Executa
                    // using (var con = GetConnection(restaurar: true))
                    // {
                    retorno = GetConnection(restaurar: true).Execute(sql, parameters, transaction: ConnectionRepository.Transaction) > 0;
                    // }
                }
            }
            return retorno;
        }

        public IEnumerable<TEntity> GetListCustom(TEntity entity, Dictionary<object, string> where = null, Dictionary<object, List<object>> between = null, object groupBy = null, object orderBy = null, int? top = null, bool orderByDesc = false)
        {
            IEnumerable<TEntity> retorno = null;
            Dictionary<string, DommelPropertyMap> dicionario = new Dictionary<string, DommelPropertyMap>();
            EntityMap.PropertyMaps
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
                foreach (var campo in where)
                {
                    campos = PropertyExtension.RetornaNomePropriedade(campo.Key);
                    foreach (var item in dicionario.Where(x => campos.Contains(x.Value.PropertyInfo.Name)))
                    {
                        var info = item.Value.PropertyInfo;
                        var dbType = GetDbType(info.PropertyType);
                        parameters.Add(item.Key, info.GetValue(entity), dbType);
                        sqlWhere.Add($"{item.Key} {campo.Value} @{item.Key}");
                    }
                }
            }
            #endregion
            #region Between
            //  Monta lista do Between utilizando o mapeamento do Dapper
            if (between != null)
            {
                foreach (var campo in between)
                {
                    campos = PropertyExtension.RetornaNomePropriedade(campo.Key);
                    foreach (var item in dicionario.Where(x => campos.Contains(x.Value.PropertyInfo.Name)))
                    {
                        //  Primeiro Comparador
                        var dbType = GetDbType(campo.Value.First().GetType());
                        parameters.Add($"{item.Key}_BETWEEN_ONE", campo.Value.First(), dbType);
                        //  Segundo Comparador
                        dbType = GetDbType(campo.Value.Last().GetType());
                        parameters.Add($"{item.Key}_BETWEEN_TWO", campo.Value.Last(), dbType);
                        sqlWhere.Add($"{item.Key} BETWEEN @{item.Key}_BETWEEN_ONE AND @{item.Key}_BETWEEN_TWO");
                    }
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
            string _top = top.HasValue ? $"LIMIT {top.Value}" : string.Empty;
            //  String Final
            string sql = $"SELECT * FROM {EntityMap.TableName} {_where} {_group} {_order} {_top}";

            // using (var con = GetConnection(restaurar: true))
            // {
            retorno = GetConnection(restaurar: true).Query<TEntity>(sql, parameters, transaction: ConnectionRepository.Transaction);
            // }

            return retorno;
        }
        /// <summary>
        /// Formata o SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        public string Formatar<T>(string sql, string alias)
        {
            var entity = typeof(T);
            var entityMap = FluentMapper.EntityMaps.First(x => x.Key == entity);
            var dommelEntityMap = (IDommelEntityMap)entityMap.Value;
            Dictionary<string, string> dicionario = new Dictionary<string, string>();
            dommelEntityMap.PropertyMaps
                .ToList().ForEach(x => dicionario.Add(x.PropertyInfo.Name, x.ColumnName));
            //  Tabela
            string procurar = $"{alias}.{{TABLE_NAME}}";
            if (sql.Contains(procurar))
            {
                sql = sql.Replace(procurar, dommelEntityMap.TableName);
            }
            //  Colunas
            foreach (var item in dicionario)
            {
                procurar = $"{alias}.{{{item.Key}}}";
                if (sql.Contains(procurar))
                {
                    sql = sql.Replace(procurar, $"{alias}.{item.Value}");
                }
            }

            return sql;
        }

        public bool Delete(int id)
        {
            // using (var con = GetConnection(restaurar: true))
            // {
            return GetConnection(restaurar: true).Delete<TEntity>(GetById(id), transaction: ConnectionRepository.Transaction);
            // }
        }

        public TEntity GetById(int id)
        {
            // using (var con = GetConnection(restaurar: true))
            // {
            return GetConnection(restaurar: true).Get<TEntity>(id);
            // }
        }

        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate)
        {
            // using (var con = GetConnection(restaurar: true))
            // {
            return GetConnection(restaurar: true).Select<TEntity>(predicate);
            // }
        }

        public int Insert(TEntity entity)
        {
            // using (var con = GetConnection(restaurar: true))
            // {
            return int.Parse(GetConnection(restaurar: true).Insert<TEntity>(entity, transaction: ConnectionRepository.Transaction).ToString());
            // }
        }

        public bool Update(TEntity entity)
        {
            // using (var con = GetConnection(restaurar: true))
            // {
            return GetConnection(restaurar: true).Update(entity, transaction: ConnectionRepository.Transaction);
            // }
        }

        public IEnumerable<TEntity> GetAll()
        {
            // using (var con = GetConnection(restaurar: true))
            // {
            return GetConnection(restaurar: true).GetAll<TEntity>();
            // }
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
        private void SetEntityMap()
        {
            var entityMap = FluentMapper.EntityMaps.First(x => x.Key == typeof(TEntity));
            EntityMap = (IDommelEntityMap)entityMap.Value;
        }
    }
}