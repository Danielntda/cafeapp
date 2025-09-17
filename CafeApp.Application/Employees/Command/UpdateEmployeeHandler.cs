using CafeApp.Persistence;
using CafeApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CafeApp.Application.Common.Validators;

public class UpdateEmployeeHandler : IRequestHandler<UpdateEmployeeCommand, bool>
{
    private readonly CafeDbContext _context;

    public UpdateEmployeeHandler(CafeDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Employee;

        // --- Find existing employee ---
        var employee = await _context.Employee.FirstOrDefaultAsync(e => e.Id == dto.Id, cancellationToken);
        if (employee == null)
            throw new ArgumentException($"Employee with ID '{dto.Id}' does not exist.");

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

        // --- Update fields ---
        employee.Name = dto.Name;
        employee.EmailAddress = dto.EmailAddress;
        employee.PhoneNumber = dto.PhoneNumber;
        employee.Gender = dto.Gender;
        employee.CafeId = cafeId; // null if unassigned
        employee.StartDate = dto.StartDate ?? employee.StartDate;

        _context.Employee.Update(employee);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
