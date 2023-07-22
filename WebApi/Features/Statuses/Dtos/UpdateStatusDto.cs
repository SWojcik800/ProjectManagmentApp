namespace ProjectManagmentAPI.Features.Statuses.Dtos
{
    public sealed class UpdateStatusDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
