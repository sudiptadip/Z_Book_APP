using BookApp.DataAccess.Repository.IRepository;


namespace BookApp.DataAccess.Repository
{
    public class UniteOfwork : IUniteOfWork
    {
        private readonly ApplicationDbContext _db;
        public ICategoryRepository Category { get; private set; }

        public IProductRepository Product { get; private set; }

        public UniteOfwork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}