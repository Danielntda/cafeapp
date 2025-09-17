using System;
using System.Threading;
using System.Threading.Tasks;
using CafeApp.Application.Common.Validators;
using CafeApp.Application.Employees.Dtos;
using CafeApp.Application.Employees;
using CafeApp.Domain.Entities;
using CafeApp.Persistence;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class UpdateEmployeeHandlerTests
{
    private async Task<CafeDbContext> GetDbContextAsync()
    {
        var options = new DbContextOptionsBuilder<CafeDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new CafeDbContext(options);

        // Seed a cafe
        context.Cafe.Add(new Cafe
        {
            Id = Guid.NewGuid(),
            Name = "Central Cafe",
            Description = "Test Cafe",
            Location = "123 Street"
        });

        // Seed an employee
        context.Employee.Add(new Employee
        {
            Id = "UI1234567",
            Name = "Existing Employee",
            EmailAddress = "existing@example.com",
            PhoneNumber = "91234567",
            Gender = "Male",
            StartDate = DateOnly.FromDateTime(DateTime.Today)
        });

        await context.SaveChangesAsync();
        return context;
    }

    private UpdateEmployeeDto CreateValidDto(string? cafeName = null)
    {
        return new UpdateEmployeeDto
        {
            Id = "UI1234567", // existing employee
            Name = "John Doe",
            EmailAddress = "john@example.com",
            PhoneNumber = "91234567",
            Gender = "Male",
            CafeName = cafeName,
            StartDate = DateOnly.FromDateTime(DateTime.Today)
        };
    }

    [Fact]
    public async Task Handle_ValidUpdate_ReturnsTrue()
    {
        var context = await GetDbContextAsync();
        var handler = new UpdateEmployeeHandler(context);

        var dto = CreateValidDto("Central Cafe");
        var result = await handler.Handle(new UpdateEmployeeCommand { Employee = dto }, CancellationToken.None);

        Assert.True(result);

        var updated = await context.Employee.FirstAsync(e => e.Id == dto.Id);
        Assert.Equal(dto.Name, updated.Name);
        Assert.Equal(dto.EmailAddress, updated.EmailAddress);
        Assert.Equal(dto.PhoneNumber, updated.PhoneNumber);
        Assert.Equal(dto.Gender, updated.Gender);
        Assert.NotNull(updated.CafeId);
    }

    [Fact]
    public async Task Handle_InvalidEmail_ThrowsArgumentException()
    {
        var context = await GetDbContextAsync();
        var handler = new UpdateEmployeeHandler(context);

        var dto = CreateValidDto();
        dto.EmailAddress = "invalid-email";

        var ex = await Assert.ThrowsAsync<ArgumentException>(
            () => handler.Handle(new UpdateEmployeeCommand { Employee = dto }, CancellationToken.None)
        );

        Assert.Equal("EmailAddress format is invalid.", ex.Message);
    }

    [Fact]
    public async Task Handle_InvalidPhone_ThrowsArgumentException()
    {
        var context = await GetDbContextAsync();
        var handler = new UpdateEmployeeHandler(context);

        var dto = CreateValidDto();
        dto.PhoneNumber = "12345678";

        var ex = await Assert.ThrowsAsync<ArgumentException>(
            () => handler.Handle(new UpdateEmployeeCommand { Employee = dto }, CancellationToken.None)
        );

        Assert.Equal("PhoneNumber must start with 8 or 9 and be 8 digits.", ex.Message);
    }

    [Fact]
    public async Task Handle_MissingRequiredField_ThrowsArgumentException()
    {
        var context = await GetDbContextAsync();
        var handler = new UpdateEmployeeHandler(context);

        var dto = CreateValidDto();
        dto.Name = "";

        var ex = await Assert.ThrowsAsync<ArgumentException>(
            () => handler.Handle(new UpdateEmployeeCommand { Employee = dto }, CancellationToken.None)
        );

        Assert.Equal("Name is required.", ex.Message);
    }

    [Fact]
    public async Task Handle_NonExistingCafe_ThrowsArgumentException()
    {
        var context = await GetDbContextAsync();
        var handler = new UpdateEmployeeHandler(context);

        var dto = CreateValidDto("NonExistingCafe");

        var ex = await Assert.ThrowsAsync<ArgumentException>(
            () => handler.Handle(new UpdateEmployeeCommand { Employee = dto }, CancellationToken.None)
        );

        Assert.Equal("Cafe 'NonExistingCafe' does not exist.", ex.Message);
    }

    [Fact]
    public async Task Handle_NonExistingEmployee_ThrowsArgumentException()
    {
        var context = await GetDbContextAsync();
        var handler = new UpdateEmployeeHandler(context);

        var dto = CreateValidDto();
        dto.Id = "UI9999999"; // does not exist

        var ex = await Assert.ThrowsAsync<ArgumentException>(
            () => handler.Handle(new UpdateEmployeeCommand { Employee = dto }, CancellationToken.None)
        );

        Assert.Equal("Employee with ID 'UI9999999' does not exist.", ex.Message);
    }
}
