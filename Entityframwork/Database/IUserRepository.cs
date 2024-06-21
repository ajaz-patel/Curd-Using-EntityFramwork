using webapi_dotnet_core.Models;

namespace webapi_dotnet_core.Database
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
