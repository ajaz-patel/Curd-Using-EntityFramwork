namespace Entityframwork.Don_t_tansfer_Objects
{
    public class UerRegistrationDtos
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }

        public UerRegistrationDtos() {
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

        }
    }
}
