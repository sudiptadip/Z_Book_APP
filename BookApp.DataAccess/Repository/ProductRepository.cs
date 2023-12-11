using BookApp.DataAccess.Repository.IRepository;
using BookApp.DataAccess.Repository;
using BookApp.Model;

namespace BookApp.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Product entity)
        {
            _db.Products.Update(entity);
        }
    }
}