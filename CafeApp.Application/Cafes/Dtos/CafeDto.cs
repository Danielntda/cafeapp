namespace CafeApp.Application.Cafes.Dtos
{
    public class CafeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Employees { get; set; }
        public string Logo { get; set; } = null!;
        public string Location { get; set; } = null!;
    }
}
