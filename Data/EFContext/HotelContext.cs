using HotelListing.api.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.api.Data.EFContext
{
    public class HotelContext : DbContext
    {
        public HotelContext(DbContextOptions<HotelContext> options) : base(options)
        {            
        }

        public DbSet<Country>? Countries { get; set; }
        public DbSet<Hotel>? Hotels { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Country>().HasData(
                new Country
                {
                    Id = 1,
                    Name = "South Africa",
                    ShortName = "RSA"
                },
                new Country
                {
                    Id = 2,
                    Name = "Jamaica",
                    ShortName = "JM"
                },
                new Country
                {
                    Id = 3,
                    Name = "Bahamas",
                    ShortName = "BS"
                }
            );
            
            builder.Entity<Hotel>().HasData(
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
            );
        }
    }
}