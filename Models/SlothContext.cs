using Microsoft.EntityFrameworkCore;
using Sloths.Models;
using System.Linq;

namespace Sloths.Models {
    public class SlothContext : DbContext {
        public SlothContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users {get;set;}

        public int Create(User u)
        {
            Users.Add(u);
            SaveChanges();
            return u.UserId;
        }

        public User GetUserByEmail(string email)
        {
            return Users.FirstOrDefault(u => u.Email == email);
        }

        public User GetUserById(int UserId)
        {
            return Users.FirstOrDefault(u => u.UserId == UserId);
        }

    }
}