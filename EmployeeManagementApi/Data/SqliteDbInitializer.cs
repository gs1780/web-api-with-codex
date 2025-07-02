using Microsoft.Data.Sqlite;
using System.IO;

namespace EmployeeManagementApi.Data;

public static class SqliteDbInitializer
{
    public static void EnsureCreated(string connectionString, string contentRootPath)
    {
        var builder = new SqliteConnectionStringBuilder(connectionString);

        if (!Path.IsPathRooted(builder.DataSource))
            builder.DataSource = Path.Combine(contentRootPath, builder.DataSource);

        var dbPath = builder.DataSource;
        if (File.Exists(dbPath))
            return;

        Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

        var scriptPath = Path.Combine(contentRootPath, "DatabaseScripts", "Sqlite", "create_tables.sql");
        var script = File.ReadAllText(scriptPath);

        using var connection = new SqliteConnection(builder.ToString());
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = script;
        command.ExecuteNonQuery();
    }
}
