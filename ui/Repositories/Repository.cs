using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;

namespace ui.Repositories
{
    public class Repository<T> : IRepository<T>
    {
        protected readonly string _tableName;
        protected readonly MySqlConnection _dbConnection;

        public Repository(string tableName)
        {
            _tableName = tableName;
            _dbConnection = new Database().GetConnection;
            _dbConnection.Open();
        }

        public IEnumerable<T> GetAll()
        {
            var sql = "SELECT * FROM " + _tableName;
            return _dbConnection.Query<T>(sql).ToList();
        }

        //public bool Put(T model)
        //{
        //    return true;
        //}

        public void Dispose()
        {
            _dbConnection.Close();
        }
    }

    public interface IRepository<T> : IDisposable
    {
        IEnumerable<T> GetAll();
        //bool Put(T model);
    }
}