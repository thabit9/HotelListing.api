using Microsoft.AspNetCore.Identity;

namespace HotelListing.api.Models
{
    public class ApiUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}