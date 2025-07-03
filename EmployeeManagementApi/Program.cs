using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
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
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                         ?? "Data Source=DataFiles/employeedb.db";

    var sqliteBuilder = new SqliteConnectionStringBuilder(connectionString);

    if (!Path.IsPathRooted(sqliteBuilder.DataSource))
    {
        sqliteBuilder.DataSource = Path.Combine(builder.Environment.ContentRootPath, sqliteBuilder.DataSource);
    }

    Directory.CreateDirectory(Path.GetDirectoryName(sqliteBuilder.DataSource)!);

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite(sqliteBuilder.ToString()));

    SqliteDbInitializer.EnsureCreated(sqliteBuilder.ToString(), builder.Environment.ContentRootPath);
}

builder.Services.AddScoped<ExcelHelper>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
// Swagger UI is enabled for all environments so that the API
// documentation is always available.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
