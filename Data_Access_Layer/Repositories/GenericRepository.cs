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

        public DbSet<T> GetEntityOfTypeDbSet()
        {
            return entity;
        }

        public IQueryable<T> GetQuerable() 
        {
            return entity; // Since DbSet<T> class is derived from IQuerable<T> so .Net will implicitly converts/casts the 'entity' type from DbSet<T> to IQuerable<T> when returning the 'entity'.
        }

        public IQueryable<I> GetQueryable<I>() where I : class 
        {
            return _context.Set<I>();
        }

        // Now after adding these GetQuerable() & GetQueryable<I>() methods we don't need Specific Repository for an entity with Generic Repository.
        // Since we have not used this two methods for Student entity so thats why we needed and created a StudentRepository(i.e. Specific Repository for Student entity).
        // We have used these two methods in TeacherService which helps us to avoid creating a Specific Repository for Teacher entity.


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
