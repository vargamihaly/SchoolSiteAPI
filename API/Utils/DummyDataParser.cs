using API.Models.DBContext;
using GenFu;
using System.Linq;
using API.Models.Entities;


namespace API.Utils
{
    public class DummyDataParser
    {        
        public void AddDummyBlogPostDataIfNeeded(ref APIDBContext _context)
        {
            if (_context.BlogPosts.Count() < 2)
            {
                A.Configure<BlogPost>().Fill(c => c.BlogpostId, 0);
                var blogPosts = A.ListOf<BlogPost>(100);

                foreach (var blogPost in blogPosts)
                {
                    BlogPost blogPostWithoutId = new BlogPost()
                    {
                        Title = blogPost.Title,
                        Content = blogPost.Content,
                        Date = blogPost.Date,
                        IsObsolete = blogPost.IsObsolete
                    };

                    _context.Add(blogPostWithoutId);
                }

                _context.SaveChanges();
            }
        }

        public void AddDummyUserDataIfNeeded(ref APIDBContext _context)
        {
            if (_context.Users.Count() < 2)
            {
                A.Configure<User>().Fill(c => c.UserId, 0);
                var users = A.ListOf<User>(10);

                foreach (var user in users)
                {
                    CreatePasswordHash(user.Name, out byte[] passwordHash, out byte[] passwordSalt);

                    User userWithoutId = new User()
                    {
                        Name = user.Name,
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt

                    };

                    _context.Add(userWithoutId);
                }

                _context.SaveChanges();

            }
        }

           private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

    }
}
