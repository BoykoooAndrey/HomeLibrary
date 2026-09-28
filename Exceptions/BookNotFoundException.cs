namespace HomeLibrary.Exceptions
{
    public class BookNotFoundException : NotFoundException
    {
        public int BookId { get; }

        public BookNotFoundException(int bookId) : base($"Book with id {bookId} not found.")
        {
            BookId = bookId;
        }
    }
}