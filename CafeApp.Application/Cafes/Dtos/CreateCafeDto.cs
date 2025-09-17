namespace CafeApp.Application.Cafes.Dtos
{
    public class CreateCafeDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Logo { get; set; } = null!;
        public string Location { get; set; } = null!;
    }
}
