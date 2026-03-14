using Dapper;

namespace CodingTrackerProgram;

internal class CodingSessionRepository
{
    private readonly DataBase _database;

    public CodingSessionRepository(DataBase dataBase)
    {
        _database = dataBase;
    }

    public void Insert(CodingSession session)
    {
        const string SQL = @"INSERT INTO CodingSession(Date,StartTime,EndTime,Duration) Values(@Date,@StartTime,@EndTime,@Duration)";

        using var connection = _database.GetConnection();
        connection.Execute(SQL, session);
    }

    public IEnumerable<CodingSession> GetAll()
    {
        const string SQL = @"SELECT Id,Date,StartTime,EndTime,Duration FROM CodingSession ORDER BY Date ASC";

        using var connection = _database.GetConnection();
        return connection.Query<CodingSession>(SQL);
    }

    public void Delete(int id)
    {
        const string SQL = @"DElETE FROM CodingSession WHERE Id = @Id";

        using var connection = _database.GetConnection();
        int rows = connection.Execute(SQL, new { Id = id });

        if (rows == 0)
            throw new Exception("No records Found.");
    }

    public void Update(CodingSession session)
    {
        const string SQL = @"UPDATE CodingSession SET Date = @Date, StartTime = @StartTime, EndTime = @EndTime, Duration = @Duration WHERE Id = @Id";

        using var connection = _database.GetConnection();
        int rows = connection.Execute(SQL, session);

        if (rows == 0)
            throw new Exception("No record found to update.");
    }

    public bool SessionExist(int id)
    {
        const string SQL = "SELECT 1 FROM CodingSession WHERE Id = @Id LIMIT 1";

        using var connection = _database.GetConnection();
        return connection.ExecuteScalar<int?>(SQL, new { Id = id }) != null;
    }

    public IEnumerable<CodingSession> GetSessionsByDateRange(string start, string end)
    {
        const string SQL = @"SELECT * FROM CodingSession WHERE Date BETWEEN @Start AND @End ORDER BY Date ASC, Duration ASC;";

        using var connection = _database.GetConnection();
        return connection.Query<CodingSession>(SQL, new { Start = start, End = end });
    }

    public bool HasAnySessions()
    {
        const string SQL = "SELECT 1 FROM CodingSession LIMIT 1";

        using var connection = _database.GetConnection();
        return connection.ExecuteScalar<int?>(SQL) != null;
    }
}