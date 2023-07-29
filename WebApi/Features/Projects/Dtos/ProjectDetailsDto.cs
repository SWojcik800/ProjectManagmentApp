namespace ProjectManagmentAPI.Features.Projects.Dtos
{
    public sealed class ProjectDetailsDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<ProjectDetailsUserDto> Users { get; set; } = new List<ProjectDetailsUserDto>();
    }
}
