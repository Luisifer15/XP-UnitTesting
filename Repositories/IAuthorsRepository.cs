using XP_UnitTesting.Models;

namespace XP_UnitTesting.Repositories

{
    public interface IAuthorsRepository
    {
        bool AddAuthor(Author author);
        List<Author> GetAuthors();
        bool RemoveAuthor(int authorId);
        bool UpdateAuthor(Author author);
    }
}
