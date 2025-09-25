using LibaryApi.Models;
using Xunit;

namespace Project1.Test
{
    public class LibraryTests
    {
        [Fact]
        public void CanCreateAuthor()
        {
            var author = new Author { Name = "Test Author" }; // singular Author
            Assert.Equal("Test Author", author.Name);
        }

        [Fact]
        public void CanCreateMember()
        {
            var member = new Member { Name = "Test Member", Email = "test@example.com" }; // singular Member
            Assert.Equal("Test Member", member.Name);
            Assert.Equal("test@example.com", member.Email);
        }
    }
}
