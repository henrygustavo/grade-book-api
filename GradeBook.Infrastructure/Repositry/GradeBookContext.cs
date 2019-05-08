namespace GradeBook.Infrastructure.Repositry
{
    using GradeBook.Domain.Entity;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class GradeBookContext :  IdentityDbContext<User, Role, int>
    {
        public GradeBookContext(DbContextOptions<GradeBookContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

          
            modelBuilder.Entity<User>(b =>
            {
                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne()
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();

                b.ToTable("user");
            });

            modelBuilder.Entity<Role>()
                .ToTable("role");

            modelBuilder.Entity<IdentityRoleClaim<int>>()
                .ToTable("roles_claims");

            modelBuilder.Entity<IdentityUserRole<int>>()
                .ToTable("user_roles");

            modelBuilder.Entity<IdentityUserClaim<int>>()
                .ToTable("user_claims");

            modelBuilder.Entity<IdentityUserLogin<int>>()
                .ToTable("user_logins");

            modelBuilder.Entity<IdentityUserToken<int>>()
                .ToTable("user_tokens");

            modelBuilder.Entity<Student>()
             .ToTable("student");

            modelBuilder.Entity<Teacher>()
             .ToTable("teacher");

            modelBuilder.Entity<Course>()
             .ToTable("course");

            modelBuilder.Entity<GradeBook>()
            .ToTable("gradebook");

        }

    }
}
