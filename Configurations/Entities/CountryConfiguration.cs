using HotelListing.api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.api.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasData(
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
        }
    }
}