using Entityframwork.Models;


namespace Entityframwork.Database
{
    public interface IUserRepository
    {
        public  Task<bool> SaveChanges();
        public  Task AddEntity<T>(T entity);
        public  Task RemoveEntity<T>(T entity);
        public Task<IEnumerable<Usermodel>> GetUser();
        public Usermodel GetUsersingle(int userid);
    }
}
