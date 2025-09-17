using System;
using System.Threading;
using System.Threading.Tasks;
using CafeApp.Application.Employees.Dtos;
using CafeApp.Domain.Entities;
using CafeApp.Persistence;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CafeApp.Tests.Employees
{
    public class CreateEmployeeHandlerTests
    {
        private CafeDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<CafeDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // unique DB per test
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
        public async Task Handle_ShouldCreateEmployee_WithValidCafeName()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var cafe = CreateTestCafe();
            await context.Cafe.AddAsync(cafe);
            await context.SaveChangesAsync();

            var handler = new CreateEmployeeHandler(context);

            var dto = new CreateEmployeeDto
            {
                Name = "John Doe",
                EmailAddress = "john.doe@example.com",
                PhoneNumber = "91234567",
                Gender = "Male",
                CafeName = cafe.Name,
                StartDate = DateOnly.FromDateTime(DateTime.Today)
            };

            var command = new CreateEmployeeCommand { Employee = dto };

            // Act
            var newId = await handler.Handle(command, CancellationToken.None);

            // Assert
            var employee = await context.Employee.FirstOrDefaultAsync(e => e.Id == newId);
            Assert.NotNull(employee);
            Assert.Equal(dto.Name, employee.Name);
            Assert.Equal(dto.EmailAddress, employee.EmailAddress);
            Assert.Equal(dto.PhoneNumber, employee.PhoneNumber);
            Assert.Equal(dto.Gender, employee.Gender);
            Assert.Equal(cafe.Id, employee.CafeId);
            Assert.Equal(dto.StartDate, employee.StartDate);
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenCafeDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var handler = new CreateEmployeeHandler(context);

            var dto = new CreateEmployeeDto
            {
                Name = "Jane Doe",
                EmailAddress = "jane.doe@example.com",
                PhoneNumber = "81234567",
                Gender = "Female",
                CafeName = "NonExistent Cafe"
            };

            var command = new CreateEmployeeCommand { Employee = dto };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(command, CancellationToken.None));
            Assert.Contains("does not exist", ex.Message);
        }

        [Fact]
        public async Task Handle_ShouldCreateEmployee_WhenNoCafeNameProvided()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var handler = new CreateEmployeeHandler(context);

            var dto = new CreateEmployeeDto
            {
                Name = "Alice",
                EmailAddress = "alice@example.com",
                PhoneNumber = "81234567",
                Gender = "Female"
            };

            var command = new CreateEmployeeCommand { Employee = dto };

            // Act
            var newId = await handler.Handle(command, CancellationToken.None);

            // Assert
            var employee = await context.Employee.FirstOrDefaultAsync(e => e.Id == newId);
            Assert.NotNull(employee);
            Assert.Equal(dto.Name, employee.Name);
            Assert.Null(employee.CafeId);
        }

        [Fact]
        public async Task Handle_ShouldGenerateUniqueIds()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var handler = new CreateEmployeeHandler(context);

            // Pre-add an employee with a conflicting ID
            var existingEmployee = new Employee
            {
                Id = "UI1234567",
                Name = "Existing",
                EmailAddress = "existing@example.com",
                PhoneNumber = "91234567",
                Gender = "Male",
                StartDate = DateOnly.FromDateTime(DateTime.Today)
            };
            await context.Employee.AddAsync(existingEmployee);
            await context.SaveChangesAsync();

            var dto = new CreateEmployeeDto
            {
                Name = "New Employee",
                EmailAddress = "new@example.com",
                PhoneNumber = "81234567",
                Gender = "Female"
            };

            var command = new CreateEmployeeCommand { Employee = dto };

            // Act
            var newId = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotEqual(existingEmployee.Id, newId);
            var employee = await context.Employee.FirstOrDefaultAsync(e => e.Id == newId);
            Assert.NotNull(employee);
        }
    }
}
