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
            using var context = new BlogsContext(_options!);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [Test]
        public void GetABlog()
        {
            // ARRANGE
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

                var blog = new BlogPost
                {
                    BlogContent = "Test Content",
                    AuthorId = author.Id,
                    Slug = "Test-Slug",
                    Title = "Test Title",
                };

                BlogsRepository blogRepository = new(entities);
                blogRepository.AddBlog(blog);
                entities.SaveChanges();


                // ACT
                var specificBlog = entities.BlogPosts.FirstOrDefault(b => b.Id == blog.Id);

                // ASSERT
                Assert.That(specificBlog, Is.Not.Null);
                Assert.That(specificBlog.Title, Is.EqualTo("Test Title"));
            }
        }

        [Test]
        public void GetAllBlogs()
        {
            // ARRANGE
            var author1 = new Author
            {
                Email = "author1@example.com",
                Name = "Author 1",
                DateCreated = DateTime.Now,
            };

            var author2 = new Author
            {
                Email = "author2@example.com",
                Name = "Author 2",
                DateCreated = DateTime.Now,
            };

            var author3 = new Author
            {
                Email = "author3@example.com",
                Name = "Author 3",
                DateCreated = DateTime.Now,
            };

            using (var entities = new BlogsContext(_options!))
            {
                //authors
                AuthorsRepository authorRepository = new(entities);
                authorRepository.AddAuthor(author1);
                authorRepository.AddAuthor(author2);
                authorRepository.AddAuthor(author3);

                var blog1 = new BlogPost
                {
                    BlogContent = "Test Content 1",
                    AuthorId = author1.Id,
                    Slug = "Test-Slug-1",
                    Title = "Test Title 1",
                };

                var blog2 = new BlogPost
                {
                    BlogContent = "Test Content 2",
                    AuthorId = author2.Id,
                    Slug = "Test-Slug-2",
                    Title = "Test Title 2",
                };

                var blog3 = new BlogPost
                {
                    BlogContent = "Test Content 3",
                    AuthorId = author3.Id,
                    Slug = "Test-Slug-3",
                    Title = "Test Title 3",
                };

                //blogs
                BlogsRepository blogRepository = new(entities);
                blogRepository.AddBlog(blog1);
                blogRepository.AddBlog(blog2);
                blogRepository.AddBlog(blog3);

                //ACT
                var blogList = blogRepository.GetBlogs();
                var authorList = authorRepository.GetAuthors();


                //ASSERT 
                //@LUIS
                //For some reason it only sees 1 blog
                Assert.That(blogList.Count, Is.EqualTo(3));
                //This one works
                Assert.That(authorList.Count, Is.EqualTo(3));
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
                entities.SaveChanges();

                //ACT
                blogRepo.RemoveBlog(blog.Id);
                entities.SaveChanges();

                //ASSERT
                var removedBlog = entities.BlogPosts.FirstOrDefault(b => b.Id == blog.Id);
                Assert.That(removedBlog, Is.Null);
            }
        }
    }
}
