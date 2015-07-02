using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Mod.IBLL
{
    public interface INterfaceBaseService<T> where T : class
    {
        IQueryable<T> Entities();

        bool Exist(Expression<Func<T, bool>> anyLambda);

        T Find(Expression<Func<T, bool>> whereLambda);

        IQueryable<T> FindList(Expression<Func<T, bool>> whereLambda);
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>添加后的数据实体</returns>
        T Add(T entity);

        IEnumerable<T> AddList(List<T> entitys);
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

        IQueryable<T> PageList(IQueryable<T> entitys, int pageIndex, int pageSize);
       // IQueryable<T> FindPageList(out int totalRecord,int pageIndex, int pageSize, string modelName, string  columnName, string onternt,DateTime? startTime, DateTime?endTime,int order);
    
    }

}
