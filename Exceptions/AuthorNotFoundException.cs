using HomeLibrary.Exceptions;

public class AuthorNotFoundException : NotFoundException
{
    public IReadOnlyCollection<int> AuthorIds { get; }

    public AuthorNotFoundException(IEnumerable<int> authorIds) : base($"Authors not found: {string.Join(", ", authorIds)}")
    {
        AuthorIds = authorIds.ToList();
    }
}