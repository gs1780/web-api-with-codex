using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementApi.Data;
using EmployeeManagementApi.Models;

namespace EmployeeManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly AppDbContext _context;

    public HealthController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<HealthCheckResult>> Get()
    {
        var result = new HealthCheckResult
        {
            ConnectionString = _context.Database.GetConnectionString()
        };
        try
        {
            result.CanConnect = await _context.Database.CanConnectAsync();
            result.Employees = await _context.Employees
                .Take(10)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            result.CanConnect = false;
            result.Exception = ex.ToString();
        }
        return Ok(result);
    }
}
