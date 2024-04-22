using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using XP_UnitTesting.Models;
using XP_UnitTesting.Repositories;

namespace XP_UnitTesting.UnitTesting
{
    [TestFixture]
    public class BlogsRepositoryTest 
    {
        private DbContextOptions<BlogsContext>? _options;
        
        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<BlogsContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;
        }


        [Test]
        public void GetBlogs()
        {
            //ARRANGE
            var author = new Author
            {
                Email = "Jong@gmail.com",
                Name = "Test",
                DateCreated = DateTime.Now,
            };

            using (var entities = new BlogsContext(_options!))
            {
                AuthorsRepository authorRepository = new(entities);
                authorRepository.AddAuthor(author);

                var blogs = new BlogPost
                {
                    BlogContent = "Test Content",
                    AuthorId = author.Id,
                    Slug = "Test-Slug",
                    Title = "Test Title",
                };

                BlogsRepository blogRepository = new(entities);
                blogRepository.AddBlog(blogs);

                //ACT
                var blogList = blogRepository.GetBlogs();

                //ASSERT
                Assert.That(1, Is.EqualTo(blogList.Count));
                ClassicAssert.IsTrue(blogList.Any(p => p.Title == "Test Title"));

            }
        }

        [Test]
        public void AddBlog()
        {
            //ARRANGE
            var author = new Author
            {
                Email = "test@example.com",
                Name = "Test Author",
                DateCreated = DateTime.Now
            };

            var blog = new BlogPost
            {
                Title = "Test Blog Post",
                BlogContent = "This is a test blog post content.",
                Slug = "test-blog-post",
                AuthorId = author.Id
            };

            using (var entities = new BlogsContext(_options!))
            {
                var authorRepo = new AuthorsRepository(entities);
                authorRepo.AddAuthor(author);

                var blogRepo = new BlogsRepository(entities);
                // Act
                blogRepo.AddBlog(blog);
                entities.SaveChanges();

                // Assert
                var addedBlog = entities.BlogPosts.FirstOrDefault(b => b.Id == blog.Id);
                Assert.That(blog.Title, Is.EqualTo(addedBlog.Title));
                Assert.That(blog.BlogContent, Is.EqualTo(addedBlog.BlogContent));
                Assert.That(blog.Id, Is.EqualTo(addedBlog.Id));
                Assert.That(blog.Slug, Is.EqualTo(addedBlog.Slug));
            }
        }

        [Test]
        public void RemoveBlog()
        {
            // Arrange
            var author = new Author
            {
                Email = "test@example.com",
                Name = "Test Author",
                DateCreated = DateTime.Now
            };

            var blog = new BlogPost
            {
                Title = "Test Blog Post",
                BlogContent = "This is a test blog post content.",
                Slug = "test-blog-post",
                AuthorId = author.Id
            };

            using (var entities = new BlogsContext(_options!))
            {
                var authorRepo = new AuthorsRepository(entities);
                authorRepo.AddAuthor(author);
                var blogRepo = new BlogsRepository(entities);
                blogRepo.AddBlog(blog);

                var existingBlog = entities.BlogPosts.FirstOrDefault(b => b.Id == blog.Id);
                Assert.That(existingBlog, Is.Not.Null);
                //ACT
                blogRepo.RemoveBlog(blog.Id);
                var blogList = blogRepo.GetBlogs();

                //ASSERT
                var removedBlog = entities.BlogPosts.FirstOrDefault(b => b.Id == blog.Id);
                Assert.That(removedBlog, Is.Null);
            }
        }
    }
}
