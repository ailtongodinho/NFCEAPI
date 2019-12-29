using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using Dommel;
using Microsoft.Extensions.Configuration;
using NFCE.API.Interfaces;
using NFCE.API.Mapping;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
                    config.AddMap(new ExtracaoMapping());
                    config.AddMap(new ExtracaoEmissorMapping());
                    config.AddMap(new ExtracaoItemMapping());
                    config.AddMap(new ExtracaoPagamentoMapping());
                    config.ForDommel();
                });
            }
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
    }
}