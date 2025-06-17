using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed Roles (user, admin, superadmin)
            var userRoleId = "b4d452b1-487f-4a23-8a4c-19ac7d32759e";
            var adminRoleId = "d62ebe94-c0f7-4ecb-a0d4-3be0d146a758";
            var superAdminRoleId = "5fb157e2-9c55-4cf7-8124-25b435e8a5f6";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER",
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId
                },
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId
                },
                new IdentityRole
                {
                    Name = "SuperAdmin",
                    NormalizedName = "SUPERADMIN",
                    Id = superAdminRoleId,
                    ConcurrencyStamp = superAdminRoleId
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);

            // Seed SuperAdmin USer
            var superAdminUserId = "af38886f-7ca9-4a65-962a-6a49903ca2c6";

            var superAdminUser = new IdentityUser
            {
                UserName = "superadmin@gmail.com",
                Email = "superadmin@gmail.com",
                NormalizedEmail = "superadmin@gmail.com".ToUpper(),
                NormalizedUserName = "superadmin@gmail.com".ToUpper(),
                Id = superAdminUserId
            };

            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>()
                .HashPassword(superAdminUser, "superadmin@gmail.com");

            builder.Entity<IdentityUser>().HasData(superAdminUser);

            // Add All roles to SuperAdmin User

            var superAdminUserRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    UserId = superAdminUserId,
                    RoleId = userRoleId
                },
                new IdentityUserRole<string>
                {
                    UserId = superAdminUserId,
                    RoleId = adminRoleId
                },
                new IdentityUserRole<string>
                {
                    UserId = superAdminUserId,
                    RoleId = superAdminRoleId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(superAdminUserRoles);
        }
    }
}
