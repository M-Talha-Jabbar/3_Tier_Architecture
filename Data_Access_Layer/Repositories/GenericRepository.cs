using Microsoft.EntityFrameworkCore;
using Repository.Contracts;
using Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public readonly SchoolDBContext _context;
        public readonly DbSet<T> entity;

        public GenericRepository(SchoolDBContext context)
        {
            _context = context;
            entity = _context.Set<T>();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await entity.ToListAsync();
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await entity.FindAsync(id);
        }

        public void Insert(T obj)
        {
            entity.Add(obj);
        }

        public void Update(T obj)
        {
            entity.Update(obj);
        }

        public async Task DeleteAsync(object id)
        {
            // T existing = await entity.FindAsync(id);
            T existing = await GetByIdAsync(id);
            entity.Remove(existing);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
