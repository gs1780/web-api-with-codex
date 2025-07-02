using Microsoft.EntityFrameworkCore;
using EmployeeManagementApi.Data;
using EmployeeManagementApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Determine which database provider to use.
var useSqlServer = builder.Configuration.GetValue<bool>("UseSqlServer") ||
                   bool.TryParse(Environment.GetEnvironmentVariable("USE_SQL_SERVER"), out var envUseSqlServer) && envUseSqlServer;

if (useSqlServer)
{
    var connectionString = builder.Configuration.GetConnectionString("SqlServerConnection")!;
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString));
}
else
{
    var dbPath = Path.Combine(builder.Environment.ContentRootPath, "DataFiles", "employeedb.db");
    Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite($"Data Source={dbPath}"));
}

builder.Services.AddScoped<ExcelHelper>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Swagger UI is enabled for all environments so that the API
// documentation is always available.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
