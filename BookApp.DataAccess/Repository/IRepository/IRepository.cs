using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        public void Add(T entity);
        public T Get(Expression<Func<T, bool>> filter);
        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null);
        public void Delete(T entity);
        public void RemoveRange(IEnumerable<T> entities);
    }
}
