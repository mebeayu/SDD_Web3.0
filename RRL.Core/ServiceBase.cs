using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace RRL.Core
{
    /// <summary>
    /// 实体服务对象基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceBase<T> : IDisposable, IServiceBase<T> where T : class
    {
        protected DbContext db;

        public int SaveChanges()
        {
            return db.SaveChanges();
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>

        public int ExeucteSqlNonQuery(string sql, params System.Data.SqlClient.SqlParameter[] paras)
        {
            return db.Database.ExecuteSqlCommand(sql, paras);
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public int ExeucteSPNonQuery(string spName, params System.Data.SqlClient.SqlParameter[] paras)
        {
            string sql = spName;

            for (int i = 0; i < paras.Count(); i++)
            {
                System.Data.SqlClient.SqlParameter param = paras[i];

                if (param.Direction == ParameterDirection.Input)
                {
                    sql += " " + param.ParameterName;
                }
                else if (param.Direction == ParameterDirection.Output)
                {
                    sql += " " + param.ParameterName + " out";
                }
                if (i < (paras.Count() - 1))
                {
                    sql += ",";
                }
            }

            return db.Database.ExecuteSqlCommand(sql, paras);
        }

        /// <summary>
        /// 执行sql，返回对象集合
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public IEnumerable<T> ExecuteSqlEntity(string sql, System.Data.SqlClient.SqlParameter[] paras)
        {
            return db.Database.SqlQuery<T>(sql, paras);
        }

        public ServiceBase(DContext currentContext)
        {
            db = DbContextFactory.GetDbContext();
        }

        public ServiceBase()
        {
            db = DbContextFactory.GetDbContext();
        }

        public T Get(params object[] key)
        {
            T result = db.Set<T>().Find(key);
            //T result = db.Set<T>().AsNoTracking<T>().Where
            return result;
        }

        public T Get(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda)
        {
            T result = db.Set<T>().AsNoTracking().Where(whereLambda).FirstOrDefault();
            //T result = db.Set<T>().AsNoTracking<T>().Where
            return result;
        }

        public IQueryable<T> SelectQueryable()
        {
            return db.Set<T>().AsQueryable();
        }

        public IQueryable<T> SelectQueryable(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda)
        {
            return db.Set<T>().Where(whereLambda);
        }

        public IEnumerable<T> GetAll()
        {
            return db.Set<T>().ToList();
        }

        public T GetOne(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda)
        {
            return db.Set<T>().Where(whereLambda).FirstOrDefault();
        }

        public IEnumerable<T> GetAll(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda)
        {
            IQueryable<T> result = db.Set<T>().AsNoTracking().Where(whereLambda);
            return result.ToList();
        }

        public IQueryable<T> GetAllEntity(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda)
        {
            IQueryable<T> result;
            if (whereLambda != null)
            {
                result = db.Set<T>().Where(whereLambda);
            }
            else
            {
                result = db.Set<T>();
            }
            return result;
        }

        public int Count(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return db.Set<T>().Where(predicate).Count();
        }

        public int Count()
        {
            return db.Set<T>().Count();
        }

        public T Add(T entity)
        {
            db.Set<T>().Add(entity);
            db.SaveChanges();

            return entity;
        }

        private Boolean RemoveHoldingEntityInContext(T entity)
        {
            var objContext = ((IObjectContextAdapter)db).ObjectContext;
            var objSet = objContext.CreateObjectSet<T>();
            var entityKey = objContext.CreateEntityKey(objSet.EntitySet.Name, entity);

            Object foundEntity;
            var exists = objContext.TryGetObjectByKey(entityKey, out foundEntity);

            if (exists)
            {
                objContext.Detach(foundEntity);
            }

            return (exists);
        }

        public void Update(T entity)
        {
            if (entity != null)
            {
                //db.Set<T>().Attach(entity);
                RemoveHoldingEntityInContext(entity);
                //db.Entry(entity).State = EntityState.Modified;
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();
                //db.Entry(entity).Reload();
            }
        }

        public void Delete(T entity)
        {
            if (entity != null)
            {
                db.Set<T>().Attach(entity);
                //db.Entry(entity).State = EntityState.Deleted;
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
            }
            db.SaveChanges();
        }

        public void Delete(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            db.Set<T>().Where(predicate).ToList().ForEach(item =>
            {
                db.Set<T>().Remove(item);
            });
            db.SaveChanges();
        }

        public T Delete(params object[] key)
        {
            T entity = db.Set<T>().Find(key);
            if (entity != null)
            {
                db.Set<T>().Remove(entity);
                db.SaveChanges();
            }
            return entity;
        }

        public void Dispose()
        {
            if (db != null)
            {
                db.Dispose();
            }
        }
    }
}