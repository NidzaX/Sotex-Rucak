namespace Sotex.Api.Interfaces
{
    public interface IUsersService
    {
        Task<string> LoginGoogleAsync(string email, string token); 

    }
}
