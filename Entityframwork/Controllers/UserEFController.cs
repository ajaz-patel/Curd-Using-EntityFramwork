using AutoMapper;
using Entityframwork.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webapi_dotnet_core.Dont_tansfer_Objects;
using webapi_dotnet_core.Models;

namespace webapi_dotnet_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserEFController : ControllerBase
    {
        EntityContex _entityframework;

        IMapper _mapper;
        public UserEFController(IConfiguration config) {
            _entityframework = new EntityContex(config);
            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DtosTOadduser, Usermodel>();
            }));
        }
        [HttpGet("/user")]

        public IEnumerable<Usermodel> GetUser() {
            IEnumerable<Usermodel> user = _entityframework.Usertab.ToList<Usermodel>();
            return user;  
        }

        [HttpGet("/singleuser/{userid}")]
        public Usermodel GetUsersingle(int userid)
        {

            Usermodel? user = _entityframework.Usertab.Where(u => u.UserId == userid).FirstOrDefault<Usermodel>();
            if(user!= null)
            {
                return user;
            }
            throw new Exception("Failed to get users");
        }
        [HttpPut("/Updateuser/{userid}")]
        public IActionResult Updateuser(int userid,Usermodel user)
        {
             Usermodel? userup = _entityframework.Usertab.Where(u => u.UserId == userid).FirstOrDefault<Usermodel>();
            if(userup!= null)
            {
                userup.Active=user.Active;
                userup.Username=user.Username;
                userup.Age=user.Age;
                userup.Email=user.Email;
                if (_entityframework.SaveChanges() > 0)
                {
                    return Ok();
                }
            }
          
            throw new Exception("failed to update user");
           
        }
        [HttpPost("/Adduser")]
        public IActionResult Adduser(DtosTOadduser user)
        {
            Usermodel useradd = _mapper.Map<Usermodel>(user);

           /* Usermodel? useradd = new Usermodel();

            useradd.Active = user.Active;
            useradd.Username = user.Username;
            useradd.Age = user.Age;
            useradd.Email = user.Email;*/
            _entityframework.Add(useradd);
                if (_entityframework.SaveChanges() > 0)
                {
                    return Ok();
                }
            
            throw new Exception("failed to add user");

        }
        [HttpDelete("/Deleteuser/{userid}")]

        public IActionResult DeleteData(int userid)
        {
            Usermodel? userdlt = _entityframework.Usertab.Where(u => u.UserId == userid).FirstOrDefault<Usermodel>();
            if (userdlt != null)
            {
                _entityframework.Remove(userdlt);
                if(_entityframework.SaveChanges() > 0)
                {
                    return Ok();
                }
            }
            
            throw new Exception("failed to delete user");
        }



    }
}
