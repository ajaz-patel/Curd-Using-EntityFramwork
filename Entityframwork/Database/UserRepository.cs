

using System.Linq;
using System.Security.Cryptography;
using webapi_dotnet_core.Models;

namespace webapi_dotnet_core.Database
{
    public class UserRepository: IUserRepository
    {
        EntityContex _entityframework;

      
        public UserRepository(IConfiguration config)
        {
            _entityframework = new EntityContex(config);     
        }
        public bool SaveChanges()
        {
            return _entityframework.SaveChanges() > 0;
        }
        public void AddEntity<T>(T entity)
        {
            if(entity != null)
            {
                _entityframework.Add(entity);
            }
        }
        public void RemoveEntity<T>(T entity)
        {
            if (entity != null)
            {
                _entityframework.Remove(entity);
            }
        }
        public IEnumerable<Usermodel> GetUser()
        {
            IEnumerable<Usermodel> user = _entityframework.Usertab.ToList<Usermodel>();
            return user;
        }
        public  Usermodel GetUsersingle(int userid)
        {
            Usermodel? user = _entityframework.Usertab.Where(u => u.UserId == userid).FirstOrDefault<Usermodel>();
            if (user != null)
            {
                return user;
            }
            throw new Exception("Failed to get users");
        }
    }
}
