namespace ProjectManagmentAPI.Features.Priorities.Dtos
{
    public sealed class UpdatePriorityDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
