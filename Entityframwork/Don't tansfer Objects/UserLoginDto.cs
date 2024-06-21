namespace Entityframwork.Don_t_tansfer_Objects
{
    public class UserLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public UserLoginDto()
        {
            if (Email == null)
            {
                Email = "";
            }
            if (Password == null)
            {
                Password = "";
            }
        }
    }
}
