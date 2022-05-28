using AutoMapper;
using HotelListing.api.DTO;
using HotelListing.api.Models;

namespace HotelListing.api.Configurations
{
    public class MapperInitializer : Profile 
    {
        public MapperInitializer() 
        {
            CreateMap<Country, CountryDTO>().ReverseMap();
            CreateMap<Country, CreateCountryDTO>().ReverseMap();
            CreateMap<Hotel, HotelDTO>().ReverseMap();
            CreateMap<Hotel, CreateHotelDTO>().ReverseMap();
        }
    }
}