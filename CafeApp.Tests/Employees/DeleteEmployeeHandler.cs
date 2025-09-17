using CafeApp.Domain.Entities;
using CafeApp.Persistence;
using Microsoft.EntityFrameworkCore;

public class DeleteEmployeeHandlerTests
{
    private async Task<CafeDbContext> GetDbContextAsync()
    {
        var options = new DbContextOptionsBuilder<CafeDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new CafeDbContext(options);

        // Seed employee
        context.Employee.Add(new Employee
        {
            Id = "UI1234567",
            Name = "Test Employee",
            EmailAddress = "test@example.com",
            PhoneNumber = "91234567",
            Gender = "Male",
            StartDate = DateOnly.FromDateTime(DateTime.Today)
        });

        await context.SaveChangesAsync();
        return context;
    }

    [Fact]
    public async Task Handle_ExistingEmployee_DeletesSuccessfully()
    {
        var context = await GetDbContextAsync();
        var handler = new DeleteEmployeeHandler(context);

        var command = new DeleteEmployeeCommand { EmployeeId = "UI1234567" };
        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result);

        // Confirm deletion
        var employee = await context.Employee.FirstOrDefaultAsync(e => e.Id == "UI1234567");
        Assert.Null(employee);
    }

    [Fact]
    public async Task Handle_NonExistingEmployee_ThrowsArgumentException()
    {
        var context = await GetDbContextAsync();
        var handler = new DeleteEmployeeHandler(context);

        var command = new DeleteEmployeeCommand { EmployeeId = "UI9999999" };

        var ex = await Assert.ThrowsAsync<ArgumentException>(
            () => handler.Handle(command, CancellationToken.None)
        );

        Assert.Equal("Employee with ID UI9999999 does not exist.", ex.Message);
    }
}
