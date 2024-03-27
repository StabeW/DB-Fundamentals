using _01.InitialSetup;
using System.Data.SqlClient;

public class StartUp
{
    public static void Main(string[] args)
    {
        var minionInfo = Console.ReadLine()
            .Split(" ", StringSplitOptions.RemoveEmptyEntries);
        var villainInfo = Console.ReadLine()
            .Split(" ", StringSplitOptions.RemoveEmptyEntries);

        var minionName = minionInfo[1];
        var minionAge = int.Parse(minionInfo[2]);
        var townName = minionInfo[3];

        var villainName = villainInfo[1];

        using SqlConnection connection = new(Configuration.ConnectionString);
        connection.Open();

        int? townId = GetTownByName(townName, connection);

        if (townId == null)
        {
            AddTown(townName, connection);
        }

        townId = GetTownByName(townName, connection);

        int? villainId = GetVillianByName(villainName, connection);

        if (villainId == null)
        {
            AddVillian(villainName, connection);
        }

        villainId = GetVillianByName(villainName, connection);

        int? minionId = GetMinionByName(minionName, connection);

        if (minionId == null)
        {
            var insertMinion = "INSERT INTO Minions (Name, Age, TownId) VALUES (@name, @age, @townId)";

            using SqlCommand command = new(insertMinion, connection);
            command.Parameters.AddWithValue("@name", minionName);
            command.Parameters.AddWithValue("@age", minionAge);
            command.Parameters.AddWithValue("@townId", townId);
            command.ExecuteNonQuery();
        }

        minionId = GetMinionByName(minionName, connection);

        AddMinionToVillain(minionId, villainId, connection);
    }

    private static int? GetMinionByName(string minionName, SqlConnection connection)
    {
        var selectVillianId = "SELECT Id FROM Minions WHERE Name = @Name";

        using SqlCommand command = new SqlCommand(selectVillianId, connection);
        command.Parameters.AddWithValue("@Name", minionName);
        return (int?)command.ExecuteScalar();
    }

    private static void AddMinionToVillain(int? minionId, int? villainId, SqlConnection connection)
    {
        var insertMinion = "INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@minionId, @villainId)";

        using SqlCommand command = new(insertMinion, connection);

        command.Parameters.AddWithValue("@villainId", villainId);
        command.Parameters.AddWithValue("@minionId", minionId);
        command.ExecuteScalar();

        Console.WriteLine($"Successfully added {minionId} to be minion of {villainId}.");
    }

    private static void AddVillian(string villainName, SqlConnection connection)
    {
        string insertTown = "INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)";

        using SqlCommand command = new(insertTown, connection);

        command.Parameters.AddWithValue("@villainName", villainName);
        command.ExecuteNonQuery();


        Console.WriteLine($"Villain {villainName} was added to the database.");
    }

    private static int? GetVillianByName(string villainName, SqlConnection connection)
    {
        string selectVillianId = "SELECT Id FROM Villains WHERE Name = @Name";

        using SqlCommand command = new(selectVillianId, connection);
        command.Parameters.AddWithValue("@Name", villainName);
        return (int?)command.ExecuteScalar();
    }

    private static void AddTown(string townName, SqlConnection connection)
    {
        string insertTown = "INSERT INTO Towns (Name) VALUES (@townName)";

        using SqlCommand command = new(insertTown, connection);

        command.Parameters.AddWithValue("@townName", townName);
        command.ExecuteNonQuery();


        Console.WriteLine($"Town {townName} was added to the database.");
    }

    private static int? GetTownByName(string townName, SqlConnection connection)
    {
        string selectTownId = "SELECT Id FROM Towns WHERE Name = @townName";

        using SqlCommand command = new(selectTownId, connection);
        command.Parameters.AddWithValue("@townName", townName);
        return (int?)command.ExecuteScalar();
    }
}