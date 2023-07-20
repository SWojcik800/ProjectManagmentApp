namespace DapperSamples.Authorization.Models
{
    public class UserTokenData
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}
