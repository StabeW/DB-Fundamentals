using _01.InitialSetup;
using System.Data.SqlClient;

namespace _02.VillainNames
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using SqlConnection connection = new(Configuration.ConnectionString);
            connection.Open();

            string villainNames = @"SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
                                FROM Villains AS v 
                                JOIN MinionsVillains AS mv ON v.Id = mv.VillainId 
                                GROUP BY v.Id, v.Name 
                                HAVING COUNT(mv.VillainId) > 3 
                                ORDER BY COUNT(mv.VillainId)";

            using SqlCommand command = new(villainNames, connection);
            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string name = (string)reader["Name"];
                int count = (int)reader["MinionsCount"];

                Console.WriteLine($"{name} - {count}");
            }
        }
    }
}