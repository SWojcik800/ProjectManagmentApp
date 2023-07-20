namespace DapperSamples.Authorization.Jwt
{
    public interface ITokenGenerator
    {
        string GenerateJwtToken(string userName, List<string> userRoles, long userId);
    }
}