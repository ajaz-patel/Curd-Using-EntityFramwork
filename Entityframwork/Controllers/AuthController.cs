using Entityframwork.Database;
using Entityframwork.Don_t_tansfer_Objects;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

        public IActionResult Register(UserRegistrationDtos useregister)
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
                    byte[] passwordHash = GetPassHash(useregister.Password, passwordSalt);
                    string sqlauth = @"INSERT INTO Dotnet.Auth 
                                        (Email,PasswordHash,PasswordSalt,FirstName,LastName,ActiveUser) 
                                        VALUES('" + useregister.Email + "',@PasswordHash, @PasswordSalt,'" + useregister.FirstName + "','" + useregister.LastName + "',1)";

                    List<SqlParameter> sqlParameters = new List<SqlParameter>();

                    SqlParameter passwordSaltParameter = new SqlParameter("@PasswordSalt", SqlDbType.VarBinary);
                    passwordSaltParameter.Value = passwordSalt;

                    SqlParameter passwordHashParameter = new SqlParameter("@PasswordHash", SqlDbType.VarBinary);
                    passwordHashParameter.Value = passwordHash;

                    sqlParameters.Add(passwordSaltParameter);
                    sqlParameters.Add(passwordHashParameter);

                    if (_dapper.ExecuteSqlWithParameters(sqlauth, sqlParameters))
                    {
                        /*string sqladduser = @"INSERT INTO Dotnet.Auth 
                                          (FirstName,LastName,ActiveUser) 
                                          VALUES('" + useregister.FirstName + "','" + useregister.LastName + "',1)";
                          if (_dapper.ExecuteSql(sqladduser))
                          {
                              return Ok();
                          }*/
                        return Ok();
                        throw new Exception("Failed to add user");
                       
                    }
                    throw new Exception("Failed to register");

                }
                return StatusCode(409,"email alrady exisit");
            }
            return StatusCode (401,"password not match");
        }
        [HttpPost("/Login")]
        public IActionResult Login(UserLoginDto userlogind)
        {
            string passHashAndSalt = "SELECT Email,PasswordHash,PasswordSalt FROM Dotnet.Auth WHERE Email='" + userlogind.Email + "'";
           
            UserLoginConfirmationDto userLoginConfirmation = _dapper.LoadDataSingle<UserLoginConfirmationDto>(passHashAndSalt);
           
            byte[] passwordHash = GetPassHash(userlogind.Password, userLoginConfirmation.PasswordSalt);


            for (int i = 0; i < passwordHash.Length; i++)
            {
                if (passwordHash[i] != userLoginConfirmation.PasswordHash[i])
                {
                    return StatusCode(401,"Incorrect password");
                }
            }
            string useridsql = "select UserId from Dotnet.Auth where Email='"+userlogind.Email+"'";
            int userId = _dapper.LoadDataSingle<int>(useridsql);
            return Ok(new Dictionary<string, string>
            {
                {"token",createToken(userId) }
            });
        } 
        private byte[] GetPassHash(String Password, byte[] passwordSalt)
        {

            string passwordSaltPlusString = _config.GetSection("Appsettings:PassswordKey").Value + Convert.ToBase64String(passwordSalt);
            return KeyDerivation.Pbkdf2(
                password: Password,
                salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
                );
        }
        private string createToken(int userId)
        {
            Claim[] claim = new Claim[]
            {
                new Claim("userid",userId.ToString())
            };
            SymmetricSecurityKey tokenkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Appsettings:TokenKey").Value));
            SigningCredentials credentials=new SigningCredentials(tokenkey,SecurityAlgorithms.HmacSha512Signature);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claim),
                SigningCredentials = credentials,
                Expires = DateTime.Now.AddDays(1)
            };
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            SecurityToken token = handler.CreateToken(descriptor);
            return handler.WriteToken(token);
        }
           




    }
}
