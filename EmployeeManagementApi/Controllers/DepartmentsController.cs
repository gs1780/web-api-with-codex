using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementApi.Data;
using EmployeeManagementApi.Models;

namespace EmployeeManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public DepartmentsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DepartmentMaster>>> GetDepartments()
        => await _context.Departments.ToListAsync();

    [HttpPost]
    public async Task<ActionResult<DepartmentMaster>> CreateDepartment(DepartmentMaster department)
    {
        _context.Departments.Add(department);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetDepartments), new { id = department.Department }, department);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDepartment(string id, DepartmentMaster department)
    {
        if (id != department.Department)
            return BadRequest();
        _context.Entry(department).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!DepartmentExists(id))
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDepartment(string id)
    {
        var department = await _context.Departments.FindAsync(id);
        if (department == null)
            return NotFound();
        _context.Departments.Remove(department);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("import")]
    public async Task<IActionResult> Import(IFormFile file, [FromServices] ExcelHelper excelHelper)
    {
        if (file == null || file.Length == 0)
            return BadRequest();
        using var stream = file.OpenReadStream();
        using var workbook = new ClosedXML.Excel.XLWorkbook(stream);
        var worksheet = workbook.Worksheets.First();
        var departments = new List<DepartmentMaster>();
        foreach (var row in worksheet.RowsUsed().Skip(1))
        {
            departments.Add(new DepartmentMaster { Department = row.Cell(1).GetString() });
        }
        _context.Departments.AddRange(departments);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("export")]
    public async Task<IActionResult> Export([FromServices] ExcelHelper excelHelper)
    {
        var data = await _context.Departments.ToListAsync();
        using var workbook = new ClosedXML.Excel.XLWorkbook();
        var worksheet = workbook.AddWorksheet("Departments");
        worksheet.Cell(1, 1).Value = "Department";
        var row = 2;
        foreach (var dept in data)
        {
            worksheet.Cell(row, 1).Value = dept.Department;
            row++;
        }
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "departments.xlsx");
    }

    private bool DepartmentExists(string id) => _context.Departments.Any(d => d.Department == id);
}
