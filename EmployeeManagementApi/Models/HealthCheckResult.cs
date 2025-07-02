using System.Collections.Generic;

namespace EmployeeManagementApi.Models;

public class HealthCheckResult
{
    public string? ConnectionString { get; set; }
    public bool CanConnect { get; set; }
    public List<Employee>? Employees { get; set; }
    public string? Exception { get; set; }
}
