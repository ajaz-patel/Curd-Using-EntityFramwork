using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Entityframwork.Database
{
    public class Dappercontext
    {

        private readonly IConfiguration _config;
        public Dappercontext(IConfiguration config) { 
                _config=config;
        }  

        public IEnumerable<T> LoadData<T>(String sql)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.Query<T>(sql);
        }
        public T LoadDataSingle<T>(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.QuerySingle<T>(sql);
        }
        public bool ExecuteSql(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return (dbConnection.Execute(sql) > 0);
        }

        public int ExecuteSqlWithRowCount(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.Execute(sql);
        }
        public bool ExecuteSqlWithParameters(string sql,List<SqlParameter> parameters)
        {
            SqlCommand cmd = new SqlCommand(sql);

            foreach (SqlParameter param in parameters)
            {
                cmd.Parameters.Add(param);
            }
            SqlConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            dbConnection.Open();

            cmd.Connection = dbConnection;

            int rowaffected=cmd.ExecuteNonQuery();

            dbConnection.Close();
            return rowaffected > 0;
        }

    }
}
