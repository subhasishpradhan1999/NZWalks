using Microsoft.EntityFrameworkCore;

namespace abc.Models.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
    public class UserDbContex : DbContext
    {
        public UserDbContex(DbContextOptions<UserDbContex> dbContextOptions) : base(dbContextOptions) { }
        public DbSet<User> Users { get; set; }
    }
}
