using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Mod.IDAL;

namespace Mod.DAL
{
   public class BaseRepository<T>: INterfaceBaseRepository<T> where T : class 
    {
        protected ModContext Db = ContextFactory.GetCurrentContext();

       
       
       public IQueryable<T> Entities { get { return Db.Set<T>(); } }

       public T Add(T entity)
        {
            Db.Entry<T>(entity).State = System.Data.Entity.EntityState.Added;
            Db.SaveChanges();
            return entity;
        }
       public IEnumerable<T> AddList(List<T> entitys)
       {
           foreach (var entity in entitys)
           {
               Db.Entry<T>(entity).State = System.Data.Entity.EntityState.Added;
           }
           Db.SaveChanges();
           return entitys;
       }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return Db.Set<T>().Count(predicate);
        }

        public bool Update(T entity)
        {
                Db.Set<T>().Attach(entity);
                Db.Entry<T>(entity).State = System.Data.Entity.EntityState.Modified;
                return Db.SaveChanges() > 0;
            
        }

       public bool UpdateList(List<T> entitys)
       {
               entitys.ForEach(m =>
               {
                   Db.Set<T>().Attach(m);
                   Db.Entry<T>(m).State = System.Data.Entity.EntityState.Modified;
               });
               return Db.SaveChanges() > 0;
           
       }
        public bool Delete(T entity)
        {
            Db.Set<T>().Attach(entity);
            Db.Entry<T>(entity).State = System.Data.Entity.EntityState.Deleted;
            return Db.SaveChanges() > 0;
        }

       public bool DeleteList(IEnumerable<T> entitys)
       {
           foreach (var entity in entitys)
           {
               Db.Set<T>().Attach(entity);
               Db.Entry<T>(entity).State = System.Data.Entity.EntityState.Deleted;
           }
           return Db.SaveChanges() > 0;
       }
        public bool Exist(Expression<Func<T, bool>> anyLambda)
        {
            return Db.Set<T>().Any(anyLambda);
        }

        public T Find(Expression<Func<T, bool>> whereLambda)
        {
                T _entity = Db.Set<T>().FirstOrDefault<T>(whereLambda);
                return _entity; 
        }

        public IQueryable<T> FindList<TS>(Expression<Func<T, bool>> whereLamdba)
        {
            var list = Db.Set<T>().Where<T>(whereLamdba);
            
            return list;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">原IQueryable</param>
        /// <param name="propertyName">排序属性名</param>
        /// <param name="isAsc">是否正序</param>
        /// <returns>排序后的IQueryable<T/></returns>
        public IQueryable<T> OrderBy(IQueryable<T> source, string propertyName, bool isAsc=true)
        {
            if (source == null) throw new ArgumentNullException("source", "不能为空");
            if (string.IsNullOrEmpty(propertyName)) return source;
            var parameter = Expression.Parameter(source.ElementType);
            var property = Expression.Property(parameter, propertyName);
            if (property == null) throw new ArgumentNullException("propertyName", "属性不存在");
            var lambda = Expression.Lambda(property, parameter);
            var methodName = isAsc ? "OrderBy" : "OrderByDescending";
            var resultExpression = Expression.Call(typeof(Queryable), methodName, new Type[] { source.ElementType, property.Type }, source.Expression, Expression.Quote(lambda));
            return source.Provider.CreateQuery<T>(resultExpression);
        }

       public IQueryable<T> FindPageList<TS>(int pageIndex, int pageSize, out int totalRecord, Expression<Func<T, bool>> whereLamdba,string orderName, bool isAsc=true)
        {
            var list = Db.Set<T>().Where<T>(whereLamdba);
           
            totalRecord = list.Count();
            list = OrderBy(list, orderName, isAsc).Skip<T>((pageIndex - 1)*pageSize).Take<T>(pageSize);
           
            return list;
        }


        /// <summary>
        /// get records with paging
        /// </summary>
        /// <typeparam name="T">the type of items of the entities list</typeparam>
        /// <typeparam name="TOrder">the type of order field by descending</typeparam>
        /// <typeparam name="TInclude">Type of the Include</typeparam>
        /// <param name="predicate"></param>
        /// <param name="orderField">Order filed</param>
        /// <param name="include">The param need to be select in the same time</param>
        /// <param name="pageIndex">page index started with 0. numbers that less than 0 will be treated as 0</param>
        /// <param name="pageSize">the number of return results. return all if less than or equal 0</param>
        /// <param name="desc">the order of orderFiled</param>
        /// <returns></returns>
        public List<T> GetRecordsByProperty<T, TOrder, TInclude>(Expression<Func<T, bool>> predicate, Expression<Func<T, TOrder>> orderField, int pageIndex, int pageSize, bool desc, params Expression<Func<T, TInclude>>[] include) where T : class
        {
            var table = GetTable<T>();
            List<T> result;
            if (pageIndex < 0)
            {
                pageIndex = 0;
            }
            if (pageSize > 0)
                if (desc)
                    result = GetTableQuery(table, include).Where(predicate).OrderByDescending(orderField).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                else
                    result = GetTableQuery(table, include).Where(predicate).OrderBy(orderField).Skip(pageIndex * pageSize).Take(pageSize).ToList();
            else
            {
                result = GetTableQuery(table, include).Where(predicate).ToList();
            }
            return result;
        }

        private IQueryable<T> GetTableQuery<T, TInclude>(DbSet<T> table, params Expression<Func<T, TInclude>>[] includes) where T : class
        {
            IQueryable<T> tableQuery = table;
            foreach (Expression<Func<T, TInclude>> include in includes)
            {
                tableQuery = tableQuery.Include(include);
            }
            return tableQuery;
        }
        public DbSet<T> GetTable<T>() where T : class
        {
            Type type = Db.GetType();
            Type tableType = typeof(T);
            DbSet<T> respon;
            var property = type.GetProperty(tableType.Name);
            //lock (lockObj)
            {
                respon = (DbSet<T>)Convert.ChangeType(property.GetValue(Db, null), typeof(DbSet<T>));
            }
           // log.DebugFormat("Get Table {0} .", tableType.Name);
            return respon;
        }
    }
}

