using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementApi.Models;

public class Employee
{
    [Key]
    public int EmployeeID { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime DOJ { get; set; }

    [Required]
    public string Department { get; set; } = string.Empty;
}
