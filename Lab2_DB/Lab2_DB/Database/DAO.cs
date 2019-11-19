using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2_DB.Database
{
    abstract class DAO<T>
    {
        protected DBConnection dbconnection;

        public DAO(DBConnection db){
            dbconnection = db;
        }

        public abstract void Create(T entity);
        public abstract T Get(long id);
        public abstract List<T> Get(int page);
        public abstract void Update(T entity);
        public abstract void Delete(long id);
    }
}
