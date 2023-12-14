using BookApp.DataAccess.Repository.IRepository;
using BookApp.DataAccess.Repository;
using BookApp.Model;

namespace BookApp.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ApplicationDbContext _db;
        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(ShoppingCart entity)
        {
            _db.ShoppingCarts.Update(entity);
        }
    }
}