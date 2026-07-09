using System;
using System.Collections.Generic;
using System.Text;

namespace TeamTaskManager.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetById(Guid id);
        Task<IEnumerable<T>> GetAll();
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}
