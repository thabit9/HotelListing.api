using HotelListing.api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.api.Configurations
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(
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