namespace DapperSamples.Authorization.Models
{
    public sealed class CreatedTokenResult
    {
        public UserTokenData TokenData { get; set; }
        public string Token { get; set; }
    }
}
