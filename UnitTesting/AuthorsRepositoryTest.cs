using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Reflection.Metadata;
using XP_UnitTesting.Models;
using XP_UnitTesting.Repositories;

namespace XP_UnitTesting.UnitTesting
{
    [TestFixture]
    public class AuthorsRepositoryTest : Controller
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
        public void GetAllAuthors()
        {
            //ARRANGE
            using var context = new BlogsContext(_options!);
            InitArrange(context);
            AuthorsRepository authorRepository = new(context);
            //ACT
            var authorList = authorRepository.GetAuthors();

            //ASSERT
            Assert.That(authorList.Count, Is.EqualTo(4));
        }

        [Test]
        public void GetSpecificAuthor()
        {
            //ARRANGE
            using var context = new BlogsContext(_options!);
            InitArrange(context);
            AuthorsRepository authorRepository = new(context);

            //ACT
            var specificAuthor = authorRepository.FindById(1);

            //ASSERT
            Assert.That(specificAuthor, Is.Not.Null);
            Assert.That(specificAuthor.Name, Is.EqualTo("Frank Herbert"));

        }

        [Test]
        public void GetBlogAuthor()
        {
            //ARRANGE
            using var context = new BlogsContext(_options!);
            InitArrange(context);
            AuthorsRepository authorRepository = new(context);
            BlogsRepository blogRepository = new(context);

            //ACT
            var specificBlog = blogRepository.FindById(4);
            var BlogAuthor = authorRepository.FindById(specificBlog.AuthorId);

            //ASSERT
            Assert.That(BlogAuthor, Is.Not.Null);
            Assert.That(BlogAuthor.Name, Is.EqualTo("Andrzej Sapkowski"));
        }

        [Test]
        public void AddAuthors()
        {
            //ARRANGE
            var NewAuthor = new Author
            {
                Id = 5,
                Email = "PKD@example.com",
                Name = "Philip K. Dick",
                DateCreated = DateTime.Now,
            };

            using var entities = new BlogsContext(_options!);
            var authorRepository = new AuthorsRepository(entities);
            authorRepository.AddAuthor(NewAuthor);

            //ACT
            var newAuthor = authorRepository.FindById(NewAuthor.Id);

            //ASSERT
            Assert.That(NewAuthor.Name, Is.EqualTo(NewAuthor.Name));
            Assert.That(NewAuthor.Email, Is.EqualTo(NewAuthor.Email));
            Assert.That(NewAuthor.DateCreated, Is.EqualTo(NewAuthor.DateCreated));

        }
        [Test]
        public void AddAuthorWithoutName()
        {
            //ARRANGE
            using var context = new BlogsContext(_options!);
            InitArrange(context);
            AuthorsRepository authorRepository = new(context);

            var EmptyAuthor = new Author
            {
                Id = 5,
                Email = "PKD@gmail.com",
                DateCreated = DateTime.Now,
            };
            authorRepository.AddAuthor(EmptyAuthor);


            //ACT
            var addedAuthor = authorRepository.FindById(EmptyAuthor.Id);

            //ASSERT
            Assert.That(addedAuthor, Is.Null);

        }

        [Test]
        public void AddAuthorWithoutEmail()
        {
            //ARRANGE
            using var context = new BlogsContext(_options!);
            InitArrange(context);
            AuthorsRepository authorRepository = new(context);

            var EmptyAuthor = new Author
            {
                Id = 5,
                Name = "Philip K. Dick",
                DateCreated = DateTime.Now,
            };
            authorRepository.AddAuthor(EmptyAuthor);

            //ACT
            var addedAuthor = authorRepository.FindById(EmptyAuthor.Id);

            //ASSERT
            Assert.That(addedAuthor, Is.Null);

        }
        [Test]
        public void RemoveAuthorWBlog()
        {
            //ARRANGE
            using var context = new BlogsContext(_options!);
            InitArrange(context);
            AuthorsRepository authorRepository = new(context);

            //ACT
            var removeAuthor = authorRepository.RemoveAuthor(3);

            //ASSERT
            Assert.That(removeAuthor, Is.False);
            var removedAuthor = authorRepository.FindById(3);
            Assert.That(removedAuthor, Is.Not.Null);

        }
        [Test]
        public void RemoveAuthorWithoutBlog()
        {
            //ARRANGE
            using var context = new BlogsContext(_options!);
            InitArrange(context);
            AuthorsRepository authorRepository = new(context);
            var EmptyAuthor = new Author
            {
                Id = 5,
                Email = "PKD@gmail.com",
                Name = "Philip K. Dick",
                DateCreated = DateTime.Now,
            };
            authorRepository.AddAuthor(EmptyAuthor);
            var addedAuthor = authorRepository.FindById(EmptyAuthor.Id); ;
            Assert.That(addedAuthor, Is.Not.Null);

            //ACT
            authorRepository.RemoveAuthor(5);

            //ASSERT
            var removedAuthor = authorRepository.FindById(5);
            Assert.That(removedAuthor, Is.Null);

        }
        [Test]
        public void UpdateAuthor()
        {
            //ARRANGE
            using var context = new BlogsContext(_options!);
            InitArrange(context);
            AuthorsRepository authorRepository = new(context);

            //ACT
            var newEmail = "JackRyan@gmail.com";
            var authorUpdate = authorRepository.FindById(4);
            authorUpdate.Email = newEmail;
            var didAuthorUpdate = authorRepository.UpdateAuthor(authorUpdate);

            //ASSERT
            Assert.That(didAuthorUpdate, Is.True);
            var AuthorUpdated = authorRepository.FindById(4);
            Assert.That(AuthorUpdated, Is.Not.Null);
            Assert.That(AuthorUpdated.Email, Is.EqualTo(newEmail));
        }
        [Test]
        public void UpdateNonExistingAuthor()
        {
            //ARRANGE
            using var context = new BlogsContext(_options!);
            InitArrange(context);
            AuthorsRepository authorRepository = new(context);

            //ACT
            var NewBlogID = 77;
            var NewBlogEmail = "PKD@gmail.com";
            var nonExistantAuthor = new Author
            {
                Id = NewBlogID,
                Email = NewBlogEmail,
                Name = "Ghost",
                DateCreated = DateTime.Now,
            };
            var didAuthorUpdate = authorRepository.UpdateAuthor(nonExistantAuthor);

            //ASSERT
            Assert.That(didAuthorUpdate, Is.False);
            var AuthorUpdated = authorRepository.FindById(NewBlogID);
            Assert.That(AuthorUpdated, Is.Null);
        }



    }
}

//TEMPLATE


//[Test]
//public void TestTitle()
//{
//    //ARRANGE
//    using var context = new BlogsContext(_options!);
//    InitArrange(context);
//    AuthorsRepository authorRepository = new(context);

//    //ACT
//    var authorList = authorRepository.GetAuthors();

//    //ASSERT

//}