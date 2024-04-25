using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using XP_UnitTesting.Models;

namespace XP_UnitTesting.Repositories
{
    public class AuthorsRepository(BlogsContext _context) : IAuthorsRepository
    {
        private readonly BlogsContext context = _context;

        public bool AddAuthor(Author author)
        {
            try
            {
                // Log blog details before adding
                author.DateCreated = DateTime.Now;
                context.Authors.Add(author);
                context.SaveChanges();
                Console.WriteLine($"Adding Author: Title - {author.Name}, Id - {author.Id}");

                // Log successful addition
                Console.WriteLine($"Author Added Successfully: Title - {author.Name}");
                return true;
            }
            catch (Exception ex)
            {
                // Log error message during exception
                Console.WriteLine($"Error adding Author: {ex.Message}");
                return false;
            }
        }

        public List<Author> GetAuthors()
        {
            //var authors = context.Authors.ToList();
            //return authors;
            return [.. context.Authors];
        }

        public bool RemoveAuthor(int authorId)
        {
            try
            {
                var author = context.Authors.Where(x => x.Id == authorId).FirstOrDefault();

                //best practice
                if (author == null)
                {
                    return false;
                }

                //remove author
                context.Authors.Remove(author);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateAuthor(Author author)
        {
            try
            {
                context.Update(author);
                context.SaveChanges();
                return true;

            }
            catch (DbUpdateConcurrencyException)
            {
                if (context.Authors.Any(x => x.Id == author.Id))
                {
                    throw;
                }
                return false;
            }
        }

        public Author FindById(int id)
        {
            return context.Authors.FirstOrDefault(b => b.Id == id);
        }
    }
}
