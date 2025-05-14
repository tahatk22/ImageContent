using ImageContent.Common.Interfaces.IRepository;
using ImageContent.Domain.Models;
using ImageContent.Infrastracture.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ImageContent.DAL.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext context;
        internal DbSet<T> Set;

        public Repository(AppDbContext context)
        {
            this.context = context;
            Set = context.Set<T>();
        }
        public async Task Add(T obj)
        {
            await Set.AddAsync(obj);
        }

        public void Delete(T obj)
        {
            Set.Remove(obj);
        }

        public void DeleteRange(IEnumerable<T> objs)
        {
            Set.RemoveRange(objs);
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, string? props = null)
        {
            IQueryable<T> Data = Set;
            if (filter is not null)
            {
                Data = Data.Where(filter);
            }
            if (props is not null)
            {
                foreach (var item in props.Split(',' , StringSplitOptions.RemoveEmptyEntries))
                {
                    Data.Include(item.Trim());
                }
            }
            return await Data.ToListAsync();
        }

        public async Task<T> GetOne(Expression<Func<T, bool>> filter, string? props = null)
        {
            var Data = Set.Where(filter);
            if (!string.IsNullOrWhiteSpace(props))
            {
                foreach (var item in props.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    Data = Data.Include(item.Trim());
                }
            }
            return await Data.FirstOrDefaultAsync();
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public void Update(T obj)
        {
            Set.Update(obj);
        }
    }
}
