﻿namespace AutoNest.Data.Common.Repositories
{
    public interface IRepository<TEntity> : IDisposable
        where TEntity : class
    {
        IQueryable<TEntity> All();
        TEntity GetById(string id);
        IQueryable<TEntity> AllAsNoTracking();

        Task AddAsync(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        Task<int> SaveChangesAsync();
    }
}
