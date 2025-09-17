using CafeApp.Application.Employees.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CafeApp.Api.Controllers
{
    [ApiController]
    [Route("employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET /employees?cafe=<cafeName>
        [HttpGet]
        public async Task<ActionResult<List<EmployeeDto>>> GetAllEmployees([FromQuery] string? cafe = null)
        {
            var employees = await _mediator.Send(new GetAllEmployeesQuery { Cafe = cafe });
            return Ok(employees);
        }

        // POST /employees
        [HttpPost]
        public async Task<ActionResult<string>> Create([FromBody] CreateEmployeeDto dto)
        {
            try
            {
                var id = await _mediator.Send(new CreateEmployeeCommand { Employee = dto });
                return CreatedAtAction(nameof(GetAllEmployees), new { id }, id);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        // PUT /employees
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateEmployeeDto dto)
        {
            try
            {
                await _mediator.Send(new UpdateEmployeeCommand { Employee = dto });
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // DELETE /employees/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _mediator.Send(new DeleteEmployeeCommand { EmployeeId = id });
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}
