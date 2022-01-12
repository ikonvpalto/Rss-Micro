using Auth.Common.Models;

namespace Auth.Common.Contracts;

public interface IAuthService
{
    Task<string> GetLoginUrlAsync();

    Task<string> GetLogoutUrlAsync();

    Task<UserProfile> GetUserProfileAsync(string token);
}
