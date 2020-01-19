using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NFCE.API.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        TEntity GetById(int id);
        int Insert(TEntity entity);
        bool Update(TEntity entity);
        bool Delete(int id);
        IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate);
        bool UpdateGenerico(TEntity entity, object atualizarCampos = null, object filtro = null);
        IEnumerable<TEntity> GetListCustom(TEntity entity, object where = null, object groupBy = null, object orderBy = null, int? top = null, bool orderByDesc = false);
    }
}