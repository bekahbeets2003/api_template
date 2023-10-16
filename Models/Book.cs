namespace api_template.Models
{
    public class Book
    {
        public int book_id { get; set; }
        public string book_name { get; set; }
        public string book_description { get; set; }
        public int author_id { get; set; }
        public int gutenberg_id { get; set; }
    }
}
