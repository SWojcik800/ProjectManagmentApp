using Microsoft.AspNetCore.Authorization;

namespace DapperSamples.Authorization
{
    public interface IAuthorizationService
    {
        Task<AuthorizationResult> SignIn(string login, string password);
    }
}