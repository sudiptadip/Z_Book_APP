using BookApp.DataAccess.Repository.IRepository;
using BookApp.DataAccess.Repository;
using BookApp.Model;

namespace BookApp.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Category entity)
        {
            _db.Categories.Update(entity);
        }
    }
}