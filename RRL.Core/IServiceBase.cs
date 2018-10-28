using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RRL.Core
{
    /// <summary>
    /// 实体服务类接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IServiceBase<T> where T : class
    {
        T Get(params object[] key);

        IEnumerable<T> GetAll();

        IEnumerable<T> GetAll(Expression<Func<T, bool>> whereLambda);

        IQueryable<T> GetAllEntity(Expression<Func<T, bool>> whereLambda);

        //查询总数量
        int Count(Expression<Func<T, bool>> predicate);

        T Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        T Delete(params object[] key);
    }
}