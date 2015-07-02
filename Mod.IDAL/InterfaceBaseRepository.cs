using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Mod.IDAL
{
    public interface INterfaceBaseRepository<T>
    {

        /// <summary>
        /// 数据实体列表
        /// </summary>
        IQueryable<T> Entities { get; }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>添加后的数据实体</returns>
        T Add(T entity);

        IEnumerable<T> AddList(List<T> entitys);
        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>记录数</returns>
        int Count(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>是否成功</returns>
        bool Update(T entity);

        bool UpdateList(List<T> entitys);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>是否成功</returns>
        bool Delete(T entity);

        bool DeleteList(IEnumerable<T> entitys);

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="anyLambda">查询表达式</param>
        /// <returns>布尔值</returns>
        bool Exist(Expression<Func<T, bool>> anyLambda);

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="whereLambda">查询表达式</param>
        /// <returns>实体</returns>
        T Find(Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 查找数据列表
        /// </summary>
        /// <typeparam name="TS">排序</typeparam>
        ///
        /// <param name="whereLamdba">查询表达式</param>
        /// <param name="orderName"></param>
        /// <param name="isAsc">是否升序</param>
        /// <returns></returns>
        IQueryable<T> FindList<TS>(Expression<Func<T, bool>> whereLamdba);

        IQueryable<T> OrderBy(IQueryable<T> source, string propertyName, bool isAsc);

        /// <summary>
        /// 查找分页数据列表
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalRecord">总记录数</param>
        /// <param name="whereLamdba">查询表达式</param>
        /// <param name="orderName"></param>
        /// <param name="isAsc">是否升序</param>
        /// <returns></returns>
        IQueryable<T> FindPageList<TS>(int pageIndex, int pageSize, out int totalRecord, Expression<Func<T, bool>> whereLamdba,string orderName, bool isAsc);

        List<T> GetRecordsByProperty<T, TOrder, TInclude>(Expression<Func<T, bool>> predicate,
            Expression<Func<T, TOrder>> orderField, int pageIndex, int pageSize, bool desc,
            params Expression<Func<T, TInclude>>[] include) where T : class;

      
    }


}
