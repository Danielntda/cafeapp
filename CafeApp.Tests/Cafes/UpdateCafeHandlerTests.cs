using System;
using System.Threading;
using System.Threading.Tasks;
using CafeApp.Persistence;
using CafeApp.Domain.Entities;
using CafeApp.Application.Cafes.Dtos;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class UpdateCafeHandlerTests
{
    private CafeDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<CafeDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new CafeDbContext(options);
    }

    private Cafe CreateTestCafe(string name = "Test Cafe")
    {
        return new Cafe
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = "Description",
            Location = "Location",
            Logo = null
        };
    }

    [Fact]
    public async Task Handle_ShouldUpdateCafe_WhenValidData()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var cafe = CreateTestCafe();
        await context.Cafe.AddAsync(cafe);
        await context.SaveChangesAsync();

        var handler = new UpdateCafeHandler(context);

        var dto = new UpdateCafeDto
        {
            Id = cafe.Id,
            Name = "Updated Name",
            Description = "Updated Desc",
            Location = "Updated Location",
            Logo = "logo.png"
        };

        var command = new UpdateCafeCommand { Cafe = dto };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        var updated = await context.Cafe.FirstOrDefaultAsync(c => c.Id == cafe.Id);
        Assert.NotNull(updated);
        Assert.Equal(dto.Name, updated!.Name);
        Assert.Equal(dto.Description, updated.Description);
        Assert.Equal(dto.Location, updated.Location);
        Assert.Equal(dto.Logo, updated.Logo);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenCafeDoesNotExist()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var handler = new UpdateCafeHandler(context);

        var dto = new UpdateCafeDto
        {
            Id = Guid.NewGuid(),
            Name = "Name",
            Description = "Desc",
            Location = "Location"
        };

        var command = new UpdateCafeCommand { Cafe = dto };

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
            handler.Handle(command, CancellationToken.None));

        Assert.Contains("does not exist", ex.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task Handle_ShouldThrow_WhenNameIsInvalid(string? invalidName)
    {
        var context = GetInMemoryDbContext();
        var cafe = CreateTestCafe();
        await context.Cafe.AddAsync(cafe);
        await context.SaveChangesAsync();

        var handler = new UpdateCafeHandler(context);

        var dto = new UpdateCafeDto
        {
            Id = cafe.Id,
            Name = invalidName,
            Description = "Desc",
            Location = "Location"
        };

        var command = new UpdateCafeCommand { Cafe = dto };

        await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenDescriptionIsMissing()
    {
        var context = GetInMemoryDbContext();
        var cafe = CreateTestCafe();
        await context.Cafe.AddAsync(cafe);
        await context.SaveChangesAsync();

        var handler = new UpdateCafeHandler(context);

        var dto = new UpdateCafeDto
        {
            Id = cafe.Id,
            Name = "Name",
            Description = "",
            Location = "Location"
        };

        var command = new UpdateCafeCommand { Cafe = dto };

        await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenLocationIsMissing()
    {
        var context = GetInMemoryDbContext();
        var cafe = CreateTestCafe();
        await context.Cafe.AddAsync(cafe);
        await context.SaveChangesAsync();

        var handler = new UpdateCafeHandler(context);

        var dto = new UpdateCafeDto
        {
            Id = cafe.Id,
            Name = "Name",
            Description = "Desc",
            Location = ""
        };

        var command = new UpdateCafeCommand { Cafe = dto };

        await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(command, CancellationToken.None));
    }
}
