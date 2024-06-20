namespace webapi_dotnet_core.Models
{
    public class Usermodel
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }   
        public int Age { get; set; }

        public bool Active { get; set; }    

        public Usermodel()
        {
            if (Username==null) {
                Username = "";
            }
            if (Email == null)
            {
                Email = "";
            }
        }
    }
   
}
