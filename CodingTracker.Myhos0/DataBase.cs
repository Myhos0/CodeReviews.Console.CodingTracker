using Microsoft.Data.Sqlite;

namespace CodingTrackerProgram;

internal class DataBase
{
    private readonly string _connectionString;

    public DataBase(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqliteConnection GetConnection() => new(_connectionString);

    internal void CreateTable() 
    {
        using var connection = GetConnection();

        connection.Open();

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText =
            @"CREATE TABLE IF NOT EXISTS CodingSession(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT NOT NULL,
                    StartTime TEXT NOT NULL,
                    EndTime TEXT NOT NULL,
                    Duration TEXT NOT NULL)";

        tableCmd.ExecuteNonQuery();
    }
}