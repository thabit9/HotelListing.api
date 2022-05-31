using HotelListing.api.DTO;

namespace HotelListing.api.Sevices
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(LoginDTO userDTO);
        Task<string> CreateToken();
    }
}