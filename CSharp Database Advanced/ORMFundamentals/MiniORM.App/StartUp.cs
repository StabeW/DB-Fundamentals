using MiniORM.App.Data;
using MiniORM.App.Data.Entities;

namespace MiniORM.App
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var connectionString = @"Server=LAPTOP-6KHLO8OB;DATABASE=MiniORM;Integrated Security=True";

            var context = new SoftUniDbContext(connectionString);

            context.Employees.Add(new Employee
            {

                FirstName = "Gosho",
                LastName = "Update",
                DepartmentId = context.Departments.First().Id,
                IsEmployed = true,

            });

            var employee = context.Employees.Last();
            employee.FirstName = "Stabeew";

            context.SaveChanges();
        }
    }
}