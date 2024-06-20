

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
    }
}
