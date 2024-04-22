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

    }
}
