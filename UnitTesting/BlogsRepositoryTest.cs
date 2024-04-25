using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Reflection.Metadata;
using XP_UnitTesting.Models;
using XP_UnitTesting.Repositories;
using static System.Reflection.Metadata.BlobBuilder;

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
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Test]
        public void GetABlog()
        {
            //ARRANGE
            var author = new Author
            {
                Id = 1,
                Email = "Jong@gmail.com",
                Name = "Test",
                DateCreated = DateTime.Now,
            };

            var blog1 = new BlogPost
            {
                BlogContent = "Test Content 1",
                AuthorId = author.Id,
                Slug = "Test-Slug-1",
                Title = "Test Title 1",
            };

            var blog2 = new BlogPost
            {
                BlogContent = "Test Content 2",
                AuthorId = author.Id,
                Slug = "Test-Slug-2",
                Title = "Test Title 2",
            };

            using var entities = new BlogsContext(_options!);
            AuthorsRepository authorRepository = new(entities);
            authorRepository.AddAuthor(author);

            BlogsRepository blogRepository = new(entities);
            blogRepository.AddBlog(blog1);
            blogRepository.AddBlog(blog2);
            entities.SaveChanges();


            //ACT
            var specificBlog = blogRepository.FindById(blog2.Id);

            //ASSERT
            //FAILING
            Assert.That(specificBlog, Is.Not.Null);
            Assert.That(specificBlog.Title, Is.EqualTo("Test Title 2"));
        }

        [Test]
        public void GetAllBlogs()
        {
            // ARRANGE
            var author1 = new Author
            {
                Id = 1,
                Email = "author1@example.com",
                Name = "Author 1",
                DateCreated = DateTime.Now,
            };

            var author2 = new Author
            {
                Id = 2,
                Email = "author2@example.com",
                Name = "Author 2",
                DateCreated = DateTime.Now,
            };

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


            using var entities = new BlogsContext(_options!);
            AuthorsRepository authorRepository = new(entities);
            authorRepository.AddAuthor(author1);
            authorRepository.AddAuthor(author2);

            BlogsRepository blogRepository = new(entities);
            blogRepository.AddBlog(blog1);
            blogRepository.AddBlog(blog2);

            //ACT
            var blogList = blogRepository.GetBlogs();
            var authorList = authorRepository.GetAuthors();

            //ASSERT 
            Assert.That(blogList.Count, Is.EqualTo(2));
            Assert.That(authorList.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetBlogsSpecificAuthor()
        {
            // ARRANGE
            var author1 = new Author
            {
                Id = 1,
                Email = "author1@example.com",
                Name = "Author 1",
                DateCreated = DateTime.Now,
            };

            var author2 = new Author
            {
                Id = 2,
                Email = "author2@example.com",
                Name = "Author 2",
                DateCreated = DateTime.Now,
            };

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
                AuthorId = author2.Id,
                Slug = "Test-Slug-3",
                Title = "Test Title 3",
            };

            using var entities = new BlogsContext(_options!);
            AuthorsRepository authorRepository = new(entities);
            authorRepository.AddAuthor(author1);
            authorRepository.AddAuthor(author2);

            BlogsRepository blogRepository = new(entities);
            blogRepository.AddBlog(blog1);
            blogRepository.AddBlog(blog2);
            blogRepository.AddBlog(blog3);

            //ACT
            var blogList = blogRepository.GetBlogs();
            var author2Blogs = blogList.Where(b => b.AuthorId == 2).ToList();
            //ASSERT
            Assert.That(blogList.Count, Is.EqualTo(3));
            Assert.That(author2Blogs.Count, Is.EqualTo(2));
        }

        [Test]
        public void AddBlog()
        {
            // ARRANGE
            var author = new Author
            {
                Id = 1,
                Email = "author1@example.com",
                Name = "Author 1",
                DateCreated = DateTime.Now,
            };

            var blog = new BlogPost
            {
                Title = "Test Content",
                BlogContent = "This is a test blog post content.",
                Slug = "test-blog-post",
                AuthorId = author.Id
            };

            using var entities = new BlogsContext(_options!);
            var authorRepository = new AuthorsRepository(entities);
            authorRepository.AddAuthor(author);

            var blogRepository = new BlogsRepository(entities);
            blogRepository.AddBlog(blog);

            //ACT
            var addedBlog = blogRepository.FindById(blog.Id);

            //ASSERT
            Assert.That(blog.Title, Is.EqualTo(addedBlog.Title));
            Assert.That(blog.BlogContent, Is.EqualTo(addedBlog.BlogContent));
            Assert.That(blog.Id, Is.EqualTo(addedBlog.Id));
            Assert.That(blog.Slug, Is.EqualTo(addedBlog.Slug));
        }

        [Test]
        public void AddBlogWithoutContent()
        {
            //ARRANGE
            var author = new Author
            {
                Id = 1,
                Email = "author1@example.com",
                Name = "Author 1",
                DateCreated = DateTime.Now,
            };

            var blog = new BlogPost
            {
                AuthorId = author.Id,
                Slug = "Test-Slug-1",
                Title = "Test Title 1",
            };

            using (var entities = new BlogsContext(_options!))
            {
                AuthorsRepository authorRepository = new(entities);
                authorRepository.AddAuthor(author);

                BlogsRepository blogRepository = new(entities);
                blogRepository.AddBlog(blog);

                //ACT
                var addedBlog = blogRepository.FindById(blog.Id);

                //ASSERT
                Assert.That(addedBlog, Is.Null);
            }
        }

        [Test]
        public void AddBlogsSingleAuthor()
        {
            // ARRANGE
            var author = new Author
            {
                Id= 1,
                Email = "author1@example.com",
                Name = "Author 1",
                DateCreated = DateTime.Now,
            };

            var blog1 = new BlogPost
            {
                BlogContent = "Test Content 1",
                AuthorId = author.Id,
                Slug = "Test-Slug-1",
                Title = "Test Title 1",
            };

            var blog2 = new BlogPost
            {
                BlogContent = "Test Content 2",
                AuthorId = author.Id,
                Slug = "Test-Slug-2",
                Title = "Test Title 2",
            };

            var blog3 = new BlogPost
            {
                BlogContent = "Test Content 3",
                AuthorId = author.Id,
                Slug = "Test-Slug-3",
                Title = "Test Title 3",
            };

            using var entities = new BlogsContext(_options!);
            var authorRepository = new AuthorsRepository(entities);
            authorRepository.AddAuthor(author);
            entities.SaveChanges();

            var blogRepository = new BlogsRepository(entities);
            blogRepository.AddBlog(blog1);
            blogRepository.AddBlog(blog2);
            blogRepository.AddBlog(blog3);
            entities.SaveChanges();

            //ACT
            var blogList = blogRepository.GetBlogs();
            var authorBlogs = entities.BlogPosts.Where(b => b.AuthorId == author.Id).ToList();

            //ASSERT
            Assert.That(blogList.Count, Is.EqualTo(3));
            Assert.That(authorBlogs.Count, Is.EqualTo(3));
        }

        [Test]
        public void RemoveBlog()
        {
            // ARRANGE
            var author = new Author
            {
                Id = 1,
                Email = "author1@example.com",
                Name = "Author 1",
                DateCreated = DateTime.Now,
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
                var authorRepository = new AuthorsRepository(entities);
                authorRepository.AddAuthor(author);

                var blogRepository = new BlogsRepository(entities);
                blogRepository.AddBlog(blog);
                var addedBlog = blogRepository.FindById(blog.Id); ;
                Assert.That(addedBlog, Is.Not.Null);

                //ACT
                blogRepository.RemoveBlog(blog.Id);

                //ASSERT
                var removedBlog = blogRepository.FindById(blog.Id); ;
                Assert.That(removedBlog, Is.Null);
            }
        }

        [Test]
        public void RemoveNonExistingBlog()
        {
            // ARRANGE
            var author = new Author
            {
                Id = 1,
                Email = "author1@example.com",
                Name = "Author 1",
                DateCreated = DateTime.Now,
            };

            var blog = new BlogPost
            {
                Title = "Test Blog Post",
                BlogContent = "This is a test blog post content.",
                Slug = "test-blog-post",
                AuthorId = author.Id
            };

            using var entities = new BlogsContext(_options!);
            var authorRepository = new AuthorsRepository(entities);
            authorRepository.AddAuthor(author);

            var blogRepository = new BlogsRepository(entities);
            blogRepository.AddBlog(blog);
            var addedBlog = blogRepository.FindById(blog.Id);
            Assert.That(addedBlog, Is.Not.Null);

            // ACT
            var nonExistingBlogId = blog.Id + 1; //Assuming this ID doesn't exist
            blogRepository.RemoveBlog(nonExistingBlogId);

            // ASSERT
            var nonExistingBlog = blogRepository.FindById(nonExistingBlogId);
            Assert.That(nonExistingBlog, Is.Null);
        }

        [Test]
        public void UpdateExistingBlog()
        {
            //ARRANGE
            var author = new Author
            {
                Id = 1,
                Email = "author1@example.com",
                Name = "Author 1",
                DateCreated = DateTime.Now,
            };

            var blog = new BlogPost
            {
                Title = "Test Blog Post",
                BlogContent = "This is a test blog post content.",
                Slug = "test-blog-post",
                AuthorId = author.Id
            };

            using var entities = new BlogsContext(_options!);
            var authorRepository = new AuthorsRepository(entities);
            authorRepository.AddAuthor(author);

            var blogRepository = new BlogsRepository(entities);
            blogRepository.AddBlog(blog);
            var addedBlog = blogRepository.FindById(blog.Id);
            Assert.That(addedBlog, Is.Not.Null);

            //ACT
            var updatedBlogContent = "Updated content for the test blog post.";
            blog.BlogContent = updatedBlogContent;
            blogRepository.UpdateBlog(blog);

            //ASSERT
            var updatedBlog = blogRepository.FindById(blog.Id);
            Assert.That(updatedBlog, Is.Not.Null);
            Assert.That(updatedBlog.BlogContent, Is.EqualTo(updatedBlogContent));
        }

        [Test]
        public void UpdateNonExistingBlog()
        {
            // ARRANGE
            var nonExistingBlogId = 50; //Assuming this ID doesn't exist

            using var entities = new BlogsContext(_options!);
            var blogRepository = new BlogsRepository(entities);

            //ACT
            var updatedBlogContent = "Updated content for a non-existing blog.";
            var blog = new BlogPost
            {
                Id = nonExistingBlogId,
                BlogContent = updatedBlogContent,
                Title = "Non-existing Blog",
                Slug = "non-existing-blog"
            };
            var result = blogRepository.UpdateBlog(blog);

            //ASSERT
            Assert.That(result, Is.False);
            var updatedBlog = blogRepository.FindById(nonExistingBlogId);
            Assert.That(updatedBlog, Is.Null);
        }
    }  
}
