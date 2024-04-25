using XP_UnitTesting.Models;

namespace XP_UnitTesting.Repositories
{
    public interface IBlogsRepository
    {
        bool AddBlog(BlogPost blog);
        List<BlogPost> GetBlogs();
        bool RemoveBlog(int blogId);
        bool UpdateBlog(BlogPost blog);
        BlogPost FindById(int id);
    }
}
