using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using XP_UnitTesting.Models;
using XP_UnitTesting.Repositories;

namespace XP_UnitTesting.UnitTesting
{
    [TestFixture]
    public class AuthorsRepositoryTest : Controller
    {
        private DbContextOptions<BlogsContext>? _options;
        
        private void InitArrange(BlogsContext context)
        {
            var author = new Author
            {
                Email = "FHerbert@gmail.com",
                Name = "Frank Herbert",
                DateCreated = DateTime.Now,
            };

            var author2 = new Author
            {
                Email = "ASap@gmail.com",
                Name = "Andrzej Sapkowski",
                DateCreated = DateTime.Now,
            };

            var author3 = new Author
            {
                Email = "WGibson@gmail.com",
                Name = "William Gibson",
                DateCreated = DateTime.Now,
            };

            var author4 = new Author
            {
                Email = "TClancy@gmail.com",
                Name = "Tom Clancy",
                DateCreated = DateTime.Now,
            };

            var blog1 = new BlogPost
            {
                BlogContent = "Dune Content",
                AuthorId = author.Id,
                Slug = "Dune",
                Title = "Dune",
            };

            var blog2 = new BlogPost
            {
                BlogContent = "Dune Content",
                AuthorId = author.Id,
                Slug = "Dune",
                Title = "Dune Messiah",
            };

            var blog3 = new BlogPost
            {
                BlogContent = "Dune Content",
                AuthorId = author.Id,
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

            //using var entities = new BlogsContext(_options!);
            AuthorsRepository authorRepository = new(context);
            authorRepository.AddAuthor(author);
            authorRepository.AddAuthor(author2);
            authorRepository.AddAuthor(author3);
                
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
        }

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<BlogsContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;
        }

        public void GetAllAuthors()
        {
            //ARRANGE
            using var context = new BlogsContext(_options!);
            InitArrange(context);
            //ACT

            //ASSERT
        }
    }
}
