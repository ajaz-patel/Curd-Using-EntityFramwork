namespace Entityframwork.Don_t_tansfer_Objects
{
    public class UserLoginConfirmationDto
    {
        public Byte[] PasswordHash {  get; set; }  
        public Byte[] PasswordSalt { get; set;}

        public UserLoginConfirmationDto() {

            if (PasswordHash == null)
            {
                PasswordHash = new Byte[0];
            }
            if (PasswordSalt == null)
            {
                PasswordSalt = new Byte[0]; 
            }
        }
    }
}
