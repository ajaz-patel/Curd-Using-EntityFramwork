using Entityframwork.Database;
using Entityframwork.Don_t_tansfer_Objects;
using Entityframwork.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Entityframwork.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly Dappercontext _dapper;
        
        private readonly IConfiguration _config;
        public PostsController(IConfiguration config) { 
               _config = config;
               _dapper = new Dappercontext(config);
        }

        [HttpGet("/Getpost")]

        public IEnumerable<PostsModel> GetPost()
        {
            string sql = "select * from Dotnet.Posts";
            return _dapper.LoadData<PostsModel>(sql);
            
        }
        [HttpGet("/Singlepost/{postid}")]
        public IEnumerable<PostsModel> GetSinglePost(int postid)
        {
            string singlepostsql = "select * from Dotnet.Posts where PostId='" + postid + "'";
            return _dapper.LoadData<PostsModel>(singlepostsql);
        }
        [HttpGet("/Getpostbyuser/{userid}")]
        public IEnumerable<PostsModel> Getpostbyuser(int userid)
        {
            string singlepostsql = "select * from Dotnet.Posts where UserId='" + userid + "'";
            return _dapper.LoadData<PostsModel>(singlepostsql);
        }
        [HttpGet("/Myposts")]
        public IEnumerable<PostsModel> Mypost()
        {
            string singlepostsql = "select * from Dotnet.Posts where UserId=" + this.User.FindFirst("UserId")?.Value;
            return _dapper.LoadData<PostsModel>(singlepostsql);
        }
        [HttpPost("/Addpost")]
        public IActionResult AddPost(PostAddDtos postAddDtos)
        {
            string sql=@"Insert into Dotnet.Posts 
                        values("+ this.User.FindFirst("UserId")?.Value +"," +
                        "'"+postAddDtos.PostTitle+"','"+postAddDtos.PostContent+ "'," +
                        "GETDATE(),GETDATE()) ";
            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }
            throw new Exception("Failed to create new posts");
        }

        [HttpPost("/UpdatePost/{postid}")]
        public IActionResult Updatepost(PostEditDtos postEditDtos)
        {
            string sql = @"
            UPDATE Dotnet.Posts 
                SET PostContent = '" + postEditDtos.PostContent +
                "', PostTitle = '" + postEditDtos.PostTitle +
                @"', PostUpdate = GETDATE()
                    WHERE PostId = " + postEditDtos.PostId +
                    "AND UserId = " + this.User.FindFirst("UserId")?.Value;
            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }
            return StatusCode(401, "unauthorize access");
        }
        [HttpDelete("/DeletePost/{postid}")]

        public IActionResult Delete(int postid)
        {
            string sql = "delete from Dotnet.Posts where PostId='"+postid+ "' and UserId = " + this.User.FindFirst("UserId")?.Value;
            if (_dapper.ExecuteSql(sql))
            {
                return Ok();    
            }
            return StatusCode(401, "unauthorize access");
        }
        [HttpGet("/Search/{searchparam}")]

        public IEnumerable<PostsModel> Search(String searchparam)
        {
            string sql = "select * from Dotnet.Posts where PostTitle LIKE '%" + searchparam + "%' or PostContent LIKE '%" + searchparam + "%'";
            return _dapper.LoadData<PostsModel>(sql);
        }
    
}
}
