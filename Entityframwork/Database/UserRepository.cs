

using System.Linq;
using System.Security.Cryptography;
using Entityframwork.Models;
using Microsoft.EntityFrameworkCore;

namespace Entityframwork.Database
{
    public class UserRepository: IUserRepository
    {
        EntityContex _entityframework;

      
        public UserRepository(IConfiguration config)
        {
            _entityframework = new EntityContex(config);     
        }
        public async Task<bool> SaveChanges()
        {
            return await _entityframework.SaveChangesAsync() > 0;
        }
        public  async Task AddEntity<T>(T entity)
        {
            if(entity != null)
            {
                await _entityframework.AddAsync(entity);
            }
        }
        public async Task RemoveEntity<T>(T entity)
        {
            if (entity != null)
            {
                 _entityframework.Remove(entity);
            }
        }
        public async Task<IEnumerable<Usermodel>> GetUser()
        {
            IEnumerable<Usermodel> user = await _entityframework.Usertab.ToListAsync<Usermodel>();
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
