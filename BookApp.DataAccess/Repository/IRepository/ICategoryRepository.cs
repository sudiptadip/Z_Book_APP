using BookApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        public void Update(Category entity);
    }
}
