namespace CafeApp.Application.Cafes.Dtos
{
    public class UpdateCafeDto
    {
        public Guid Id { get; set; }          // existing café ID
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? Logo { get; set; }     // optional
        public string Location { get; set; } = null!;
    }
}
