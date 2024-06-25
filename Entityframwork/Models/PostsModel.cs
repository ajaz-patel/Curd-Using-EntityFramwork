namespace Entityframwork.Models
{
    public class PostsModel
    {
        public int PostId { get; set; }

        public int UsertId { get; set; }

        public string PostTitle { get; set; }  

        public string PostContent { get; set; }
        
        public DateTime PostCreated { get; set; }

        public DateTime PostUpdate {  get; set; }   


        public PostsModel() {
            if (PostTitle == null)
            {
                PostTitle = "";
            }
            if (PostContent == null)
            {
                PostContent = "";
            }
        }
    }
}
