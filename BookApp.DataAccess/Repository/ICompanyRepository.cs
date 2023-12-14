using BookApp.DataAccess.Repository.IRepository;
using BookApp.DataAccess.Repository;
using BookApp.Model;

namespace BookApp.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _db;
        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Company entity)
        {
            _db.Companys.Update(entity);
        }
    }
}