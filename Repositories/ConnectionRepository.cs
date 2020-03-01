using System;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace NFCE.API.Repositories
{
    public class ConnectionRepository<TEntity> where TEntity : class
    {
        private readonly IConfiguration _config;
        private string WhichConnection => _config.GetValue<string>("Sql:Connection");
        private string GetConnectionString => _config.GetConnectionString(WhichConnection);
        public ConnectionRepository(IConfiguration config)
        {
            _config = config;
        }
        public TEntity GetConnection()
        {
            TEntity con = null;

            var databaseUri = new Uri(GetConnectionString);
            var userInfo = databaseUri.UserInfo.Split(':');

            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/'),
                SslMode = SslMode.Require,
                TrustServerCertificate = true
            };
            con = (TEntity)typeof(TEntity).GetConstructor(new[] { typeof(string) }).Invoke(new object[] { builder.ToString() });
            return con;
        }
    }
}