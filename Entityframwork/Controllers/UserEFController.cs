using AutoMapper;
using Entityframwork.Database;
using Entityframwork.Dont_tansfer_Objects;
using Entityframwork.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;



namespace Entityframwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserEFController : ControllerBase
    {
       // EntityContex _entityframework;

        IUserRepository _iuserRepository;

        IMapper _mapper;
        public UserEFController(IConfiguration config)
        {
           // _entityframework = new EntityContex(config);
            _iuserRepository = new UserRepository(config);
            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DtosTOadduser, Usermodel>();
            }));
        }
        [HttpGet("/user")]

        public async Task<IEnumerable<Usermodel>> GetUser() {
          /*  IEnumerable<Usermodel> user = _entityframework.Usertab.ToList<Usermodel>();
            return user; */ 
           return await _iuserRepository.GetUser();
        }

        [HttpGet("/singleuser/{userid}")]
        public Usermodel GetUsersingle(int userid)
        {

           /* Usermodel? user = _entityframework.Usertab.Where(u => u.UserId == userid).FirstOrDefault<Usermodel>();
            if(user!= null)
            {
                return user;
            }
            throw new Exception("Failed to get users");*/
           return _iuserRepository.GetUsersingle(userid);
        }
        [HttpPut("/Updateuser/{userid}")]
        public async Task<IActionResult> Updateuser(int userid, Usermodel user)
        {
            Usermodel? userup =  _iuserRepository.GetUsersingle(userid);
            if (userup != null)
            {
                userup.Active = user.Active;
                userup.Username = user.Username;
                userup.Age = user.Age;
                userup.Email = user.Email;
                if (await _iuserRepository.SaveChanges())
                {
                    return Ok();
                }
            }

            throw new Exception("failed to update user");

        }
        [HttpPost("/Adduser")]
        public async Task<IActionResult> Adduser(DtosTOadduser user)
        {
             Usermodel useradd = _mapper.Map<Usermodel>(user);

           // Usermodel? useradd = new Usermodel();

           /* useradd.Active = user.Active;
            useradd.Username = user.Username;
            useradd.Age = user.Age;
            useradd.Email = user.Email;*/
            await _iuserRepository.AddEntity(useradd);
                if (await _iuserRepository.SaveChanges())
                {
                    return Ok();
                }
            
            throw new Exception("failed to add user");

        }
        [HttpDelete("/Deleteuser/{userid}")]

        public async Task< IActionResult >DeleteData(int userid)
        {
            Usermodel? userdlt = _iuserRepository.GetUsersingle(userid);    
            if (userdlt != null)
            {
                await _iuserRepository.RemoveEntity(userdlt);
                if(await _iuserRepository.SaveChanges())
                {
                    return Ok();
                }
            }
            
            throw new Exception("failed to delete user");
        }



    }
}
