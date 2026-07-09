using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Application.Interfaces.Repositories;
using TeamTaskManager.Infrastructure.Data;

namespace TeamTaskManager.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _db;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(AppDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }
        public async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetById(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
