using System;
using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace NFCE.API.Repositories
{
    public class ConnectionRepository
    {
        private readonly IConfiguration _config;
        private string WhichConnection => _config.GetValue<string>("Sql:Connection");
        private string GetConnectionString => _config.GetConnectionString(WhichConnection);
        public static NpgsqlConnection Connection { get; set; }
        public static NpgsqlTransaction Transaction { get; set; }
        public static bool UseTransaction { get; set; }
        public ConnectionRepository(IConfiguration config)
        {
            _config = config;
        }
        ~ConnectionRepository()
        {
            if (Connection != null)
            {
                Transaction.Dispose();
                Connection.Dispose();
                Connection.Close();
                UseTransaction = false;
                Connection = null;
                Transaction = null;
            }
        }
        public void OpenTransaction()
        {
            UseTransaction = true;
            Connection = Connection ?? GetConnection();
            Connection.Open();
            Transaction = Connection.BeginTransaction();
        }
        public void CloseTransaction(bool commit)
        {
            if (
                Connection != null
                && Transaction != null
                && Connection.State == ConnectionState.Open
            )
            {
                if (commit) Transaction.Commit();
                else Transaction.Rollback();
            }
            UseTransaction = false;
            Connection = null;
            Transaction = null;
        }
        public NpgsqlConnection GetConnection()
        {
            // NpgsqlConnection con = null;

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
            // con = (TEntity)typeof(TEntity).GetConstructor(new[] { typeof(string) }).Invoke(new object[] { builder.ToString() });
            // return con;
            return new NpgsqlConnection(builder.ToString());
        }
    }
}