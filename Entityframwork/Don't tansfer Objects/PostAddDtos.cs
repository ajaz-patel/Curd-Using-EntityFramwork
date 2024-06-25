namespace Entityframwork.Don_t_tansfer_Objects
{
    public class PostAddDtos
    {
        public string PostTitle { get; set; }

        public string PostContent { get; set; }


        public PostAddDtos()
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
