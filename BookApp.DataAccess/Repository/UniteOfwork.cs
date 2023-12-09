using BookApp.DataAccess.Repository.IRepository;


namespace BookApp.DataAccess.Repository
{
    public class UniteOfwork : IUniteOfWork
    {
        private readonly ApplicationDbContext _db;
        public ICategoryRepository Category { get; private set; }
        public UniteOfwork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}