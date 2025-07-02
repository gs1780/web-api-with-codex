using ClosedXML.Excel;
using EmployeeManagementApi.Models;

namespace EmployeeManagementApi.Services;

public class ExcelHelper
{
    public List<Employee> ReadEmployees(Stream stream)
    {
        using var workbook = new XLWorkbook(stream);
        var worksheet = workbook.Worksheets.First();
        var employees = new List<Employee>();
        foreach (var row in worksheet.RowsUsed().Skip(1))
        {
            employees.Add(new Employee
            {
                Name = row.Cell(1).GetString(),
                DOJ = row.Cell(2).GetDateTime(),
                Department = row.Cell(3).GetString()
            });
        }
        return employees;
    }

    public byte[] WriteEmployees(IEnumerable<Employee> employees)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.AddWorksheet("Employees");
        worksheet.Cell(1, 1).Value = "Name";
        worksheet.Cell(1, 2).Value = "DOJ";
        worksheet.Cell(1, 3).Value = "Department";
        var row = 2;
        foreach (var emp in employees)
        {
            worksheet.Cell(row, 1).Value = emp.Name;
            worksheet.Cell(row, 2).Value = emp.DOJ;
            worksheet.Cell(row, 3).Value = emp.Department;
            row++;
        }
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
