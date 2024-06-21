using Entityframwork.Database;
using Entityframwork.Don_t_tansfer_Objects;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace Entityframwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly Dappercontext _dapper;

        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _dapper = new Dappercontext(config);   
            _config = config;   
        }
        [HttpPost("/Register")]

        public IActionResult Register(UerRegistrationDtos useregister)
        {
            if(useregister.Password==useregister.PasswordConfirm)
            {
                String sqlexistsuser = "SELECT Email FROM Dotnet.Auth WHERE Email='" + useregister.Email + "'";
                IEnumerable<string> exisituser=_dapper.LoadData<string>(sqlexistsuser);
                if (exisituser.Count() == 0)
                {
                    byte[] passwordSalt = new byte[128 / 8];
                    using(RandomNumberGenerator rng=RandomNumberGenerator.Create()) { 
                        rng.GetBytes(passwordSalt);
                    }
                    string passwordSaltPlusString = _config.GetSection("Appsettings:PassswordKey").Value + Convert.ToBase64String(passwordSalt);

                    Byte[] passwordHash = KeyDerivation.Pbkdf2(
                        password: useregister.Password,
                        salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
                        prf: KeyDerivationPrf.HMACSHA256,
                        iterationCount: 10000000,
                        numBytesRequested: 256 / 8
                        );
                    string sqlauth = @"INSERT INTO Dotnet.Auth 
                                        (Email,PasswordHash,PasswordSalt) 
                                        VALUES('" + useregister.Email + "',@PasswordHash, @PasswordSalt)";

                    List<SqlParameter> sqlParameters = new List<SqlParameter>();

                    SqlParameter passwordSaltParameter = new SqlParameter("@PasswordSalt", SqlDbType.VarBinary);
                    passwordSaltParameter.Value = passwordSalt;

                    SqlParameter passwordHashParameter = new SqlParameter("@PasswordHash", SqlDbType.VarBinary);
                    passwordHashParameter.Value = passwordHash;

                    sqlParameters.Add(passwordSaltParameter);
                    sqlParameters.Add(passwordHashParameter);

                    if (_dapper.ExecuteSqlWithParameters(sqlauth, sqlParameters))
                    {
                     return Ok(); 
                    }
                    throw new Exception("Failed to register");

                }
                throw new Exception("email alrady exisit");
            }
            throw new Exception("password not match");
        }
        [HttpPost("/Login")]
        public IActionResult Login(UserLoginDto userlogind)
        {
            return Ok();
        } 


    }
}
