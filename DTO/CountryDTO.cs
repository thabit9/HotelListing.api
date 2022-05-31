using System.ComponentModel.DataAnnotations;

namespace HotelListing.api.DTO
{
    public class CreateCountryDTO
    {
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage ="Country Name is too long")]
        public string Name { get; set; } = null!;
        [Required]
        [StringLength(maximumLength: 3, ErrorMessage ="Short Country Name is too long")]
        public string ShortName { get; set; } = null!;
    }
    public class UpdateCountryDTO : CreateCountryDTO
    {
        public IList<CreateHotelDTO>? Hotels { get;set; }
    }
    public class CountryDTO : CreateCountryDTO
    {
        public int Id { get; set; }
        public IList<HotelDTO>? Hotels { get;set; }
    }

}