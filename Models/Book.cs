namespace HomeLibrary.Models
{
    public class Book
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;
        public int PublicationYear { get; set; }

        public string? Content { get; set; }

        public List<Author> Authors { get; set; } = [];
    }
}
