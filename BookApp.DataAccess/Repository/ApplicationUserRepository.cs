using BookApp.DataAccess.Repository.IRepository;
using BookApp.DataAccess.Repository;
using BookApp.Model;

namespace BookApp.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;
        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(ApplicationUser entity)
        {
            _db.ApplicationUsers.Update(entity);
        }
    }
}