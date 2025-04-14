using AutoNest.Data.Common.Repositories;
using AutoNest.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace AutoNest.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected ApplicationDbContext _context;
        protected DbSet<TEntity> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }



        public Task AddAsync(TEntity entity)
        {
            return _dbSet.AddAsync(entity).AsTask();
        }

        public virtual IQueryable<TEntity> All()
        {
            return _dbSet;
        }

        public virtual IQueryable<TEntity> AllAsNoTracking()
        {
            return _dbSet.AsNoTracking();
        }

        public virtual void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }


        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Update(TEntity entity)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context?.Dispose();
            }
        }

        public TEntity GetById(string id)
        {
            return _dbSet.Find(id);
        }
    }
}