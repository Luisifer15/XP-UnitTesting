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

        private void InitArrange(BlogsContext context)
        {
            var author1 = new Author
            {
                Id = 1,
                Email = "FHerbert@gmail.com",
                Name = "Frank Herbert",
                DateCreated = DateTime.Now,
            };

            var author2 = new Author
            {
                Id = 2,
                Email = "ASap@gmail.com",
                Name = "Andrzej Sapkowski",
                DateCreated = DateTime.Now,
            };

            var author3 = new Author
            {
                Id = 3,
                Email = "WGibson@gmail.com",
                Name = "William Gibson",
                DateCreated = DateTime.Now,
            };

            var author4 = new Author
            {
                Id = 4,
                Email = "TClancy@gmail.com",
                Name = "Tom Clancy",
                DateCreated = DateTime.Now,
            };

            AuthorsRepository authorRepository = new(context);
            authorRepository.AddAuthor(author1);
            authorRepository.AddAuthor(author2);
            authorRepository.AddAuthor(author3);
            authorRepository.AddAuthor(author4);
            context.SaveChanges();

            var blog1 = new BlogPost
            {
                BlogContent = "Dune Content",
                AuthorId = author1.Id,
                Slug = "Dune",
                Title = "Dune",
            };

            var blog2 = new BlogPost
            {
                BlogContent = "Dune Content",
                AuthorId = author1.Id,
                Slug = "Dune",
                Title = "Dune Messiah",
            };

            var blog3 = new BlogPost
            {
                BlogContent = "Dune Content",
                AuthorId = author1.Id,
                Slug = "Dune",
                Title = "Children of Dune",
            };

            var blog4 = new BlogPost
            {
                BlogContent = "Witcher Content",
                AuthorId = author2.Id,
                Slug = "Witcher",
                Title = "Blood of Elves",
            };

            var blog5 = new BlogPost
            {
                BlogContent = "Witcher Content",
                AuthorId = author2.Id,
                Slug = "Witcher",
                Title = "Time of Contempt",
            };

            var blog6 = new BlogPost
            {
                BlogContent = "Witcher Content",
                AuthorId = author2.Id,
                Slug = "Witcher",
                Title = "Baptism of fire",
            };

            var blog7 = new BlogPost
            {
                BlogContent = "Cyberpunk Content",
                AuthorId = author3.Id,
                Slug = "Sprawl",
                Title = "Neuromancer",
            };

            var blog8 = new BlogPost
            {
                BlogContent = "Cyberpunk Content",
                AuthorId = author3.Id,
                Slug = "Sprawl",
                Title = "Count Zero",
            };

            var blog9 = new BlogPost
            {
                BlogContent = "Cyberpunk Content",
                AuthorId = author3.Id,
                Slug = "Sprawl",
                Title = "Mona Lisa Overdrive",
            };

            var blog10 = new BlogPost
            {
                BlogContent = "Jack Ryan Content",
                AuthorId = author4.Id,
                Slug = "Jack Ryan",
                Title = "Rainbow 6",
            };

            BlogsRepository blogRepository = new(context);
            blogRepository.AddBlog(blog1);
            blogRepository.AddBlog(blog2);
            blogRepository.AddBlog(blog3);
            blogRepository.AddBlog(blog4);
            blogRepository.AddBlog(blog5);
            blogRepository.AddBlog(blog6);
            blogRepository.AddBlog(blog7);
            blogRepository.AddBlog(blog8);
            blogRepository.AddBlog(blog9);
            blogRepository.AddBlog(blog10);
            context.SaveChanges();
        }

        [Test]
        public void GetABlog()
        {
            //ARRANGE
            using var context = new BlogsContext(_options!);
            InitArrange(context);
            BlogsRepository blogRepository = new(context);


            //ACT
            var specificBlog = blogRepository.FindById(2);

            //ASSERT
            //FAILING
            Assert.That(specificBlog, Is.Not.Null);
            Assert.That(specificBlog.Title, Is.EqualTo("Dune Messiah")); ;
        }

        [Test]
        public void GetAllBlogs()
        {
            //ARRANGE
            using var context = new BlogsContext(_options!);
            InitArrange(context);
            BlogsRepository blogRepository = new(context);

            //ACT
            var blogList = blogRepository.GetBlogs();

            //ASSERT 
            Assert.That(blogList.Count, Is.EqualTo(10));
        }

        [Test]
        public void GetBlogsSpecificAuthor()
        {
            //ARRANGE
            using var context = new BlogsContext(_options!);
            InitArrange(context);
            BlogsRepository blogRepository = new(context);

            //ACT
            var blogList = blogRepository.GetBlogs();
            var author2Blogs = blogList.Where(b => b.AuthorId == 2).ToList();
            //ASSERT
            Assert.That(blogList.Count, Is.EqualTo(10));
            Assert.That(author2Blogs.Count, Is.EqualTo(3));
        }

        [Test]
        public void AddBlog()
        {
            //ARRANGE
            using var context = new BlogsContext(_options!);
            InitArrange(context);
            BlogsRepository blogRepository = new(context);

            //ACT
            var addedBlog = blogRepository.FindById(1);

            //ASSERT
            Assert.That(addedBlog.Title, Is.EqualTo("Dune"));
        }

        [Test]
        public void AddBlogWithoutContent()
        {
            //ARRANGE
            var author = new Author
            {
                Email = "author1@example.com",
                Name = "Author 1",
                DateCreated = DateTime.Now,
            };

            using var entities = new BlogsContext(_options!);
            AuthorsRepository authorRepository = new(entities);
            authorRepository.AddAuthor(author);

            var blog = new BlogPost
            {
                AuthorId = author.Id,
                Slug = "Test-Slug-1",
                Title = "Test Title 1",
            };

            BlogsRepository blogRepository = new(entities);
            blogRepository.AddBlog(blog);

            //ACT
            var addedBlog = blogRepository.FindById(blog.Id);

            //ASSERT
            Assert.That(addedBlog, Is.Null);
        }

        [Test]
        public void AddBlogsSingleAuthor()
        {
            //ARRANGE
            using var context = new BlogsContext(_options!);
            InitArrange(context);
            BlogsRepository blogRepository = new(context);

            //ACT
            var blogList = blogRepository.GetBlogs();
            var authorBlogs = context.BlogPosts.Where(b => b.AuthorId == 2).ToList();

            //ASSERT
            Assert.That(authorBlogs.Count, Is.EqualTo(3));
        }

        [Test]
        public void RemoveBlog()
        {
            //ARRANGE
            using var context = new BlogsContext(_options!);
            InitArrange(context);
            BlogsRepository blogRepository = new(context);

            //ACT
            blogRepository.RemoveBlog(2);

            //ASSERT
            var removedBlog = blogRepository.FindById(2); ;
            Assert.That(removedBlog, Is.Null);
        }

        [Test]
        public void RemoveNonExistingBlog()
        {
            //ARRANGE
            using var context = new BlogsContext(_options!);
            InitArrange(context);
            BlogsRepository blogRepository = new(context);

            // ACT
            var nonExistingBlogId = 11; //this ID doesn't exist
            blogRepository.RemoveBlog(nonExistingBlogId);

            // ASSERT
            var nonExistingBlog = blogRepository.FindById(nonExistingBlogId);
            Assert.That(nonExistingBlog, Is.Null);
        }

        [Test]
        public void UpdateExistingBlog()
        {
            //ARRANGE
            using var context = new BlogsContext(_options!);
            InitArrange(context);
            BlogsRepository blogRepository = new(context);

            //ACT
            var addedBlog = blogRepository.FindById(5);
            Assert.That(addedBlog, Is.Not.Null);
            var updatedBlogContent = "Updated content for the test blog post.";
            addedBlog.BlogContent = updatedBlogContent;
            blogRepository.UpdateBlog(addedBlog);

            //ASSERT
            var updatedBlog = blogRepository.FindById(5);
            Assert.That(updatedBlog, Is.Not.Null);
            Assert.That(updatedBlog.BlogContent, Is.EqualTo(updatedBlogContent));
        }

        [Test]
        public void UpdateNonExistingBlog()
        {
            // ARRANGE
            using var context = new BlogsContext(_options!);
            InitArrange(context);
            BlogsRepository blogRepository = new(context);
            var nonExistingBlogId = 11; //this ID doesn't exist

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
