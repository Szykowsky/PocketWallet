using Microsoft.EntityFrameworkCore;
using PocketWallet.Data.Models;

namespace PocketWallet.Data
{
    public class PasswordWalletContext : DbContext
    {
        public virtual  DbSet<User> Users { get; set; }
        public virtual DbSet<Password> Passwords { get; set; }

        public PasswordWalletContext(DbContextOptions<PasswordWalletContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(user =>
            {
                user.HasKey(t => t.Id);
                user.Property(t => t.Id).ValueGeneratedOnAdd();
                user.HasIndex(t => t.Login).IsUnique(true);
            });

            modelBuilder.Entity<Password>(password =>
            {
                password.HasKey(t => t.Id);
                password.Property(t => t.Id).ValueGeneratedOnAdd();
                password.HasOne(t => t.User).WithMany().HasForeignKey(t => t.UserId);
            });
        }
    }
}
