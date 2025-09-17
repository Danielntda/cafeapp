using CafeApp.Domain.Entities;
using CafeApp.Persistence;
using Microsoft.EntityFrameworkCore;

public class GetCafesHandlerTests
{
    private CafeDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<CafeDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new CafeDbContext(options);
    }

    private async Task SeedData(CafeDbContext context)
    {
        var cafeA = new Cafe { Id = Guid.NewGuid(), Name = "Cafe A", Description = "Desc A", Location = "Loc1" };
        var cafeB = new Cafe { Id = Guid.NewGuid(), Name = "Cafe B", Description = "Desc B", Location = "Loc1" };
        var cafeC = new Cafe { Id = Guid.NewGuid(), Name = "Cafe C", Description = "Desc C", Location = "Loc2" };

        // Employees
        cafeA.Employees.Add(new Employee { Id = "UI00001", Name = "Emp1", EmailAddress = "e1@a.com", PhoneNumber = "91234567", Gender = "Male" });
        cafeA.Employees.Add(new Employee { Id = "UI00002", Name = "Emp2", EmailAddress = "e2@a.com", PhoneNumber = "91234568", Gender = "Female" });

        cafeB.Employees.Add(new Employee { Id = "UI00003", Name = "Emp3", EmailAddress = "e3@b.com", PhoneNumber = "92234567", Gender = "Male" });

        context.Cafe.AddRange(cafeA, cafeB, cafeC);
        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_ReturnsAllCafes_OrderedByEmployees()
    {
        var context = GetInMemoryDbContext();
        await SeedData(context);

        var handler = new GetCafesHandler(context);

        var result = await handler.Handle(new GetCafesQuery(), CancellationToken.None);

        Assert.Equal(3, result.Count);
        Assert.Equal("Cafe A", result[0].Name); // Cafe A has 2 employees
        Assert.Equal("Cafe B", result[1].Name); // Cafe B has 1 employee
        Assert.Equal("Cafe C", result[2].Name); // Cafe C has 0 employees
    }

    [Fact]
    public async Task Handle_FiltersByLocation()
    {
        var context = GetInMemoryDbContext();
        await SeedData(context);

        var handler = new GetCafesHandler(context);

        var result = await handler.Handle(new GetCafesQuery { Location = "Loc1" }, CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.All(result, c => Assert.Equal("Loc1", c.Location));
    }

    [Fact]
    public async Task Handle_NoEmployees_ReturnsZeroEmployeesCount()
    {
        var context = GetInMemoryDbContext();

        var cafe = new Cafe
        {
            Id = Guid.NewGuid(),
            Name = "Cafe D",
            Description = "Desc D",
            Location = "Loc3"
        };

        context.Cafe.Add(cafe);
        await context.SaveChangesAsync();

        var handler = new GetCafesHandler(context);

        var result = await handler.Handle(new GetCafesQuery(), CancellationToken.None);

        var cafeDto = result.First(c => c.Name == "Cafe D");
        Assert.Equal(0, cafeDto.Employees);
    }

    [Fact]
    public async Task Handle_FilterWithNoMatches_ReturnsEmptyList()
    {
        var context = GetInMemoryDbContext();
        await SeedData(context);

        var handler = new GetCafesHandler(context);

        var result = await handler.Handle(new GetCafesQuery { Location = "NonExistentLoc" }, CancellationToken.None);

        Assert.Empty(result);
    }
}
