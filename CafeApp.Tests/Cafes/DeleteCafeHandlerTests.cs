using CafeApp.Domain.Entities;
using CafeApp.Persistence;
using Microsoft.EntityFrameworkCore;

public class DeleteCafeHandlerTests
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
            Description = "Test Description",
            Location = "Test Location"
        };
    }

    [Fact]
    public async Task Handle_ShouldDeleteCafe_WhenExists()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var cafe = CreateTestCafe();
        await context.Cafe.AddAsync(cafe);
        await context.SaveChangesAsync();

        var handler = new DeleteCafeHandler(context);

        // Act
        var result = await handler.Handle(new DeleteCafeCommand { CafeId = cafe.Id }, CancellationToken.None);

        // Assert
        Assert.True(result);
        var deletedCafe = await context.Cafe.FirstOrDefaultAsync(c => c.Id == cafe.Id);
        Assert.Null(deletedCafe);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenCafeDoesNotExist()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var handler = new DeleteCafeHandler(context);
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
            handler.Handle(new DeleteCafeCommand { CafeId = nonExistentId }, CancellationToken.None));

        Assert.Contains("does not exist", ex.Message);
    }

    [Fact]
    public async Task Handle_ShouldDeleteEmployees_WhenCafeDeleted()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var cafe = CreateTestCafe();
        await context.Cafe.AddAsync(cafe);

        var employee1 = new Employee
        {
            Id = "UI0000001",
            Name = "John",
            EmailAddress = "john@test.com",
            PhoneNumber = "91234567",
            Gender = "Male",
            CafeId = cafe.Id
        };
        var employee2 = new Employee
        {
            Id = "UI0000002",
            Name = "Jane",
            EmailAddress = "jane@test.com",
            PhoneNumber = "92222222",
            Gender = "Female",
            CafeId = cafe.Id
        };
        await context.Employee.AddRangeAsync(employee1, employee2);
        await context.SaveChangesAsync();

        var handler = new DeleteCafeHandler(context);

        // Act
        var result = await handler.Handle(new DeleteCafeCommand { CafeId = cafe.Id }, CancellationToken.None);

        // Assert
        Assert.True(result);
        Assert.Null(await context.Cafe.FirstOrDefaultAsync(c => c.Id == cafe.Id));
        Assert.Empty(context.Employee.Where(e => e.CafeId == cafe.Id));
    }
}
