using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementApi.Data;
using EmployeeManagementApi.Models;
using EmployeeManagementApi.Services;

namespace EmployeeManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ExcelHelper _excelHelper;

    public EmployeesController(AppDbContext context, ExcelHelper excelHelper)
    {
        _context = context;
        _excelHelper = excelHelper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        => await _context.Employees.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> GetEmployee(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null)
            return NotFound();
        return employee;
    }

    [HttpPost]
    public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeID }, employee);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(int id, Employee employee)
    {
        if (id != employee.EmployeeID)
            return BadRequest();
        _context.Entry(employee).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!EmployeeExists(id))
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null)
            return NotFound();
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("import")]
    public async Task<IActionResult> Import(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest();
        using var stream = file.OpenReadStream();
        var employees = _excelHelper.ReadEmployees(stream);
        _context.Employees.AddRange(employees);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("export")]
    public async Task<IActionResult> Export(string? department, DateTime? startDate, DateTime? endDate)
    {
        var query = _context.Employees.AsQueryable();
        if (!string.IsNullOrEmpty(department))
            query = query.Where(e => e.Department == department);
        if (startDate.HasValue)
            query = query.Where(e => e.DOJ >= startDate);
        if (endDate.HasValue)
            query = query.Where(e => e.DOJ <= endDate);
        var data = await query.ToListAsync();
        var bytes = _excelHelper.WriteEmployees(data);
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "employees.xlsx");
    }

    private bool EmployeeExists(int id) => _context.Employees.Any(e => e.EmployeeID == id);
}
