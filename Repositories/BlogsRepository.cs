using Microsoft.EntityFrameworkCore;
using XP_UnitTesting.Models;

namespace XP_UnitTesting.Repositories
{
    public class BlogsRepository(BlogsContext _context) : IBlogsRepository
    {
        private readonly BlogsContext context = _context;

        public bool AddBlog(BlogPost blog)
        {
            try
            {
                // Log blog details before adding
                Console.WriteLine($"Adding Blog: Title - {blog.Title}, AuthorId - {blog.AuthorId}, BlogID - {blog.Id}");

                context.BlogPosts.Add(blog);
                context.SaveChanges();

                // Log successful addition
                Console.WriteLine($"Blog Added Successfully: Title - {blog.Title}");
                return true;
            }
            catch (Exception ex)
            {
                // Log error message during exception
                Console.WriteLine($"Error adding blog: {ex.Message}");

                return false;
            }
        }

        public List<BlogPost> GetBlogs()
        {
            //var blogs = context.BlogPosts.ToList();
            //return blogs;
            return [.. context.BlogPosts];
        }

        public bool RemoveBlog(int blogId)
        {
            try
            {
                var blog = context.BlogPosts.Where(x => x.Id == blogId).FirstOrDefault();

                //best practice
                if (blog == null)
                {
                    return false;
                }

                //remove author
                context.BlogPosts.Remove(blog);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateBlog(BlogPost blog)
        {
            try
            {
                context.Update(blog);
                context.SaveChanges();
                return true;

            }
            catch (DbUpdateConcurrencyException)
            {
                if (context.Authors.Any(x => x.Id == blog.Id))
                {
                    throw;
                }
                return false;
            }
        }
        public BlogPost FindById(int id)
        {
            return context.BlogPosts.FirstOrDefault(b => b.Id == id);         
        }
    }

}
