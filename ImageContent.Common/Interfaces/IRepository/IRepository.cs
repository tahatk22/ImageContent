using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ImageContent.Common.Interfaces.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetOne(Expression<Func<T, bool>> filter, string? props = null);
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, string? props = null);
        Task Add(T obj);
        void Update(T obj);
        void Delete(T obj);
        void DeleteRange(IEnumerable<T> objs);
        Task Save();
    }
}
