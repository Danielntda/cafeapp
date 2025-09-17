using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CafeApp.Application.Employees.Dtos;
using CafeApp.Persistence;
using CafeApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class GetAllEmployeesHandlerTests
{
    private async Task<CafeDbContext> GetDbContextAsync()
    {
        var options = new DbContextOptionsBuilder<CafeDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new CafeDbContext(options);

        // Seed cafes with unique names
        var cafeA = new Cafe { Id = Guid.NewGuid(), Name = "Cafe A", Description = "Desc A", Location = "Loc A" };
        var cafeB = new Cafe { Id = Guid.NewGuid(), Name = "Cafe B", Description = "Desc B", Location = "Loc B" };

        context.Cafe.AddRange(cafeA, cafeB);

        // Seed employees
        context.Employee.AddRange(
            new Employee
            {
                Id = "UI0000001",
                Name = "Daniel",
                EmailAddress = "daniel@test.com",
                PhoneNumber = "91111111",
                Gender = "Male",
                CafeId = cafeA.Id,
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-10))
            },
            new Employee
            {
                Id = "UI0000002",
                Name = "Ali",
                EmailAddress = "ali@test.com",
                PhoneNumber = "92222222",
                Gender = "Male",
                CafeId = cafeB.Id,
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-5))
            },
            new Employee
            {
                Id = "UI0000003",
                Name = "Muthu",
                EmailAddress = "muthu@test.com",
                PhoneNumber = "93333333",
                Gender = "Female",
                CafeId = null,
                StartDate = null
            }
        );

        await context.SaveChangesAsync();
        return context;
    }

    [Fact]
    public async Task Handle_ReturnsAllEmployees()
    {
        var context = await GetDbContextAsync();
        var handler = new GetAllEmployeesHandler(context);

        var result = await handler.Handle(new GetAllEmployeesQuery(), CancellationToken.None);

        Assert.Equal(3, result.Count);
        Assert.Contains(result, e => e.Name == "Daniel");
        Assert.Contains(result, e => e.Name == "Ali");
        Assert.Contains(result, e => e.Name == "Muthu");
    }

    [Fact]
    public async Task Handle_FiltersByCafeName()
    {
        var context = await GetDbContextAsync();
        var handler = new GetAllEmployeesHandler(context);

        var result = await handler.Handle(new GetAllEmployeesQuery { Cafe = "Cafe A" }, CancellationToken.None);

        Assert.Single(result);
        Assert.Equal("Daniel", result.First().Name);
    }

    [Fact]
    public async Task Handle_EmployeeWithNoCafe_ReturnsEmptyCafeName()
    {
        var context = await GetDbContextAsync();
        var handler = new GetAllEmployeesHandler(context);

        var result = await handler.Handle(new GetAllEmployeesQuery(), CancellationToken.None);

        var employeeWithoutCafe = result.First(e => e.Name == "Muthu");
        Assert.Equal(string.Empty, employeeWithoutCafe.CafeName);
    }
}
