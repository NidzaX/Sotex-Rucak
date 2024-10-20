using Sotex.Api.Dto.UserDto;
using Sotex.Api.Model;

namespace Sotex.Api.Interfaces
{
    public interface IUsersService
    {
        Task<string> LoginGoogleAsync(string email, string token);
        Task<User> RegisterGoogleUserAsync(GoogleRegisterDto dto); 

    }
}
