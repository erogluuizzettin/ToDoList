using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Entity;

namespace ToDoList.Core.DAL.EntityFramework
{
    public class EFRepositoryBase<TEntity, TContext> : IRepository<TEntity>
        where TEntity : BaseEntity, new()
        where TContext : DbContext, new()
    {
        TContext ctx;

        public EFRepositoryBase()
        {
            ctx = EFSingletonContext<TContext>.Instance;
        }

        public int Add(TEntity entity)
        {
            ctx.Entry(entity).State = EntityState.Added;
            return ctx.SaveChanges();
        }

        public int Delete(TEntity entity)
        {
            ctx.Entry(entity).State = EntityState.Deleted;
            return ctx.SaveChanges();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            return ctx.Set<TEntity>().Where(filter).SingleOrDefault();
        }

        public ICollection<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            if (filter == null)
            {
                return ctx.Set<TEntity>().ToList();
            }
            else
            {
                return ctx.Set<TEntity>().Where(filter).ToList();
            }
        }

        public int Update(TEntity entity)
        {
            ctx.Entry(entity).State = EntityState.Modified;
            return ctx.SaveChanges();
        }
    }
}
