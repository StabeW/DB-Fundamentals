using _01.InitialSetup;
using System.Data.SqlClient;

namespace _03.MinionNames
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var id = int.Parse(Console.ReadLine());
            using SqlConnection connection = new(Configuration.ConnectionString);
            connection.Open();

            var villainId = @"SELECT Name FROM Villains WHERE Id = @Id";

            using SqlCommand command = new(villainId, connection);

            command.Parameters.AddWithValue("@Id", id);
            var villainName = (string)command.ExecuteScalar();

            if (villainName == null)
            {
                Console.WriteLine($"No villain with ID {id} exists in the database.");
            }

            Console.WriteLine($"Villain: {villainName}");


            var minionsName = @"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,
                                         m.Name, 
                                         m.Age
                                    FROM MinionsVillains AS mv
                                    JOIN Minions As m ON mv.MinionId = m.Id
                                   WHERE mv.VillainId = @Id
                                ORDER BY m.Name";

            using SqlCommand sqlCommand = new(minionsName, connection);

            sqlCommand.Parameters.AddWithValue("@Id", id);

            using SqlDataReader reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                var rowNumber = (long)reader["RowNum"];
                var name = (string)reader["Name"];
                var age = (int)reader["Age"];

                Console.WriteLine($"{rowNumber}. {name} {age}");
            }

            if (!reader.HasRows)
            {
                Console.WriteLine("(no minions)");
            }
        }
    }
}