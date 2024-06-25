namespace Entityframwork.Don_t_tansfer_Objects
{
    public class PostEditDtos
    {
        public int PostId { get; set; }

        public string PostTitle { get; set; }

        public string PostContent { get; set; }


        public PostEditDtos()
        {
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
