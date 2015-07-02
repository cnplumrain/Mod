using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mod.IBLL;
using Mod.IDAL;

namespace Mod.BLL
{
    public abstract class BaseService<T> : INterfaceBaseService<T> where T : class
    {
        protected INterfaceBaseRepository<T> CurrentRepository { get; set; }

        protected BaseService(INterfaceBaseRepository<T> currentRepository) { CurrentRepository = currentRepository; }

        public IQueryable<T> Entities()
        {
            return CurrentRepository.Entities;
        }

        public bool Exist(Expression<Func<T, bool>> anyLambda)
        {
            return CurrentRepository.Exist(anyLambda);
        }

        public T Find(Expression<Func<T, bool>> whereLambda)
        {
            return CurrentRepository.Find(whereLambda);
        }
        public IQueryable<T> FindList(Expression<Func<T, bool>> whereLambda)
        {
            return CurrentRepository.FindList<T>(whereLambda);
        }
        public T Add(T entity) { return CurrentRepository.Add(entity); }
        public IEnumerable<T> AddList(List<T> entitys)
        {
            return CurrentRepository.AddList(entitys);
        } 

        public bool Update(T entity) { return CurrentRepository.Update(entity); }
        public bool UpdateList(List<T> entitys) { return CurrentRepository.UpdateList(entitys); }
        public bool Delete(T entity) { return CurrentRepository.Delete(entity); }

        public bool DeleteList(IEnumerable<T> entitys)
        {
            return CurrentRepository.DeleteList(entitys);
        }

        public IQueryable<T> PageList(IQueryable<T> entitys, int pageIndex, int pageSize)
        {
            return entitys.Skip((pageIndex - 1)*pageSize).Take(pageSize);
        }
       
    }

}
