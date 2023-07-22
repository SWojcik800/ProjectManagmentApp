using DapperSamples.Authorization.Models;
using DapperSamples.Features.Account.Dtos;

namespace ProjectManagmentAPI.Authorization.Providers
{
    public interface ITokenDataProvider
    {
        Task<UserTokenData> GetTokenData(AuthorizeDto authorizeDto);
    }
}