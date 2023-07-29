namespace ProjectManagmentAPI.Features.Projects.Dtos
{
    public sealed class UpdateProjectDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<long>? UserIds { get; set; }
    }
}
