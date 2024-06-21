namespace Entityframwork.Dont_tansfer_Objects
{
    public class DtosTOadduser
    {
        public string Username { get; set; }

        public string Email { get; set; }
        public int Age { get; set; }

        public bool Active { get; set; }

        public DtosTOadduser()
        {
            if (Username == null)
            {
                Username = "";
            }
            if (Email == null)
            {
                Email = "";
            }
        }
    }
}
