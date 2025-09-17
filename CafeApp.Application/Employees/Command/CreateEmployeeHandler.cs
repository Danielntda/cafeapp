using CafeApp.Persistence;
using CafeApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CafeApp.Application.Common.Validators;

public class CreateEmployeeHandler : IRequestHandler<CreateEmployeeCommand, string>
{
    private readonly CafeDbContext _context;

    public CreateEmployeeHandler(CafeDbContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Employee;

        // --- Validation ---
        EmployeeValidator.ValidateRequired(dto.Name, "Name");
        EmployeeValidator.ValidateRequired(dto.EmailAddress, "EmailAddress");
        EmployeeValidator.ValidateRequired(dto.PhoneNumber, "PhoneNumber");
        EmployeeValidator.ValidateRequired(dto.Gender, "Gender");
        EmployeeValidator.ValidateEmail(dto.EmailAddress);
        EmployeeValidator.ValidatePhoneNumber(dto.PhoneNumber);

        // --- Map CafeName to CafeId ---
        Guid? cafeId = null;
        if (!string.IsNullOrWhiteSpace(dto.CafeName))
        {
            var cafe = await _context.Cafe.FirstOrDefaultAsync(c => c.Name == dto.CafeName, cancellationToken);
            if (cafe == null)
                throw new ArgumentException($"Cafe '{dto.CafeName}' does not exist.");
            cafeId = cafe.Id;
        }

        // --- Generate unique Employee ID ---
        string newId;
        do
        {
            newId = "UI" + Guid.NewGuid().ToString("N").Substring(0, 7).ToUpper();
        } while (await _context.Employee.AnyAsync(e => e.Id == newId, cancellationToken));

        var employee = new Employee
        {
            Id = newId,
            Name = dto.Name,
            EmailAddress = dto.EmailAddress,
            PhoneNumber = dto.PhoneNumber,
            Gender = dto.Gender,
            CafeId = cafeId,
            StartDate = dto.StartDate
        };

        _context.Employee.Add(employee);
        await _context.SaveChangesAsync(cancellationToken);

        return employee.Id;
    }
}
