namespace Entityframwork.Don_t_tansfer_Objects
{
    public class UserRegistrationDtos
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public string FirstName {  get; set; }  
        public string LastName { get; set; }    


        

        public UserRegistrationDtos() {
            if (Email == null)
            {
                Email = "";
            }
            if(Password == null)
            {
                Password = "";
            }
            if (PasswordConfirm == null) {
                PasswordConfirm = "";
            }
            if (FirstName == null)
            {
                FirstName = "";
            }
            if (LastName == null)
            {
                LastName = "";
            }


        }
    }
}
