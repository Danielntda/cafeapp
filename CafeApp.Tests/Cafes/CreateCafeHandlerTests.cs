using CafeApp.Application.Cafes.Dtos;
using CafeApp.Persistence;
using Microsoft.EntityFrameworkCore;

public class CreateCafeHandlerTests
{
    private CafeDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<CafeDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new CafeDbContext(options);
    }

    [Fact]
    public async Task Handle_ShouldCreateCafe_WithValidData()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var handler = new CreateCafeHandler(context);

        var dto = new CreateCafeDto
        {
            Name = "Test Cafe",
            Description = "Test Description",
            Location = "Test Location",
            Logo = "test-logo.png"
        };

        var command = new CreateCafeCommand { Cafe = dto };

        // Act
        var id = await handler.Handle(command, CancellationToken.None);

        // Assert
        var cafe = await context.Cafe.FirstOrDefaultAsync(c => c.Id == id);
        Assert.NotNull(cafe);
        Assert.Equal(dto.Name, cafe.Name);
        Assert.Equal(dto.Description, cafe.Description);
        Assert.Equal(dto.Location, cafe.Location);
        Assert.Equal(dto.Logo, cafe.Logo);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task Handle_ShouldThrow_WhenNameIsInvalid(string? invalidName)
    {
        var context = GetInMemoryDbContext();
        var handler = new CreateCafeHandler(context);

        var dto = new CreateCafeDto
        {
            Name = invalidName,
            Description = "Desc",
            Location = "Loc",
            Logo = "logo.png"
        };

        var command = new CreateCafeCommand { Cafe = dto };

        await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenDescriptionIsMissing()
    {
        var context = GetInMemoryDbContext();
        var handler = new CreateCafeHandler(context);

        var dto = new CreateCafeDto
        {
            Name = "Cafe Name",
            Description = "",
            Location = "Loc",
            Logo = null!
        };

        var command = new CreateCafeCommand { Cafe = dto };

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(command, CancellationToken.None));
        Assert.Contains("Description is required", ex.Message);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenLocationIsMissing()
    {
        var context = GetInMemoryDbContext();
        var handler = new CreateCafeHandler(context);

        var dto = new CreateCafeDto
        {
            Name = "Cafe Name",
            Description = "Desc",
            Location = null!,
            Logo = null!
        };

        var command = new CreateCafeCommand { Cafe = dto };

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(command, CancellationToken.None));
        Assert.Contains("Location is required", ex.Message);
    }
}
