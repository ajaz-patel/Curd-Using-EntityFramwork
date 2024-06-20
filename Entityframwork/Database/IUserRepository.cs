namespace webapi_dotnet_core.Database
{
    public interface IUserRepository
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entity);
        public void RemoveEntity<T>(T entity);
    }
}
