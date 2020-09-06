using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Dapper.FluentMap.Dommel.Mapping;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace NFCE.API.Interfaces.Repositories
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        TEntity GetById(int id);
        int Insert(TEntity entity);
        bool Update(TEntity entity);
        bool Delete(int id);
        IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate);
        bool UpdateGenerico(TEntity entity, object atualizarCampos = null, object filtro = null);
        IEnumerable<TEntity> GetListCustom(TEntity entity, Dictionary<object, string> where = null, Dictionary<object, List<object>> between = null, object groupBy = null, object orderBy = null, int? top = null, bool orderByDesc = false);
        string Formatar<T>(string sql, string alias);
        IConfiguration GetConfiguration { get; set; }
        #region Connection
        NpgsqlConnection GetConnection(bool restaurar = false);
        void SetConnection(NpgsqlConnection connection);
        void OpenTransaction();
        void CloseTransaction(bool commit);
        IDommelEntityMap EntityMap { get; set; }
        #endregion
    }
}