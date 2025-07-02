using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementApi.Models;

public class DepartmentMaster
{
    [Key]
    public string Department { get; set; } = string.Empty;
}
