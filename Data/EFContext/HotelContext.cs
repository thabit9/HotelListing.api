using HotelListing.api.Configurations;
using HotelListing.api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.api.Data.EFContext
{
    public class HotelContext : IdentityDbContext<ApiUser>
    {
        public HotelContext(DbContextOptions<HotelContext> options) : base(options)
        {            
        }

        public DbSet<Country>? Countries { get; set; }
        public DbSet<Hotel>? Hotels { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Telling Identity To Change Table Names
            /*
            builder.Entity<IdentityRole<int>>().ToTable("Role");
            builder.Entity<IdentityUser<int>>().ToTable("User");
            builder.Entity<IdentityUserRole<int>>().ToTable("UserRole");
            builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaim");
            builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaim");
            builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogin");
            builder.Entity<IdentityUserToken<int>>().ToTable("UserToken");
            */
            #endregion

            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new CountryConfiguration());
            builder.ApplyConfiguration(new HotelConfiguration());
            #region Seeding Without Configuration
            /*builder.Entity<Hotel>().HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "Chabala Resort",
                    Address = "Hammanskraal",
                    Rating = 4.5,
                    CountryId = 1
                },
                new Hotel
                {
                    Id = 2,
                    Name = "OnPoint Resort",
                    Address = "Hammanskraal",
                    Rating = 4.5,
                    CountryId = 1
                },
                new Hotel
                {
                    Id = 3,
                    Name = "Sandals Resort and Spa",
                    Address = "Negril",
                    Rating = 5,
                    CountryId = 2
                },
                new Hotel
                {
                    Id = 4,
                    Name = "Grand Palladium",
                    Address = "Nassua",
                    Rating = 5,
                    CountryId = 3
                }
            );*/
            #endregion
        }
    }
}