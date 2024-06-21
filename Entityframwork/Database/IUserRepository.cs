using Entityframwork.Models;


namespace Entityframwork.Database
{
    public interface IUserRepository
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entity);
        public void RemoveEntity<T>(T entity);
        public IEnumerable<Usermodel> GetUser();
        public Usermodel GetUsersingle(int userid);
    }
}
