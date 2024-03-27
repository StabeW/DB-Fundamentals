using Microsoft.VisualBasic;
using SoftUni.Data;
using SoftUni.Models;
using System.Globalization;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var context = new SoftUniContext())
            {
                var result = GetEmployeesInPeriod(context);
                Console.WriteLine(result);
            }
        }

        //03.Employees Full Information
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employees = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    e.Salary
                }).ToList();

            employees.ForEach(e => sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:F2}"));

            return sb.ToString().TrimEnd();
        }

        //04.Employees with Salary Over 50 000
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var sb = new StringBuilder();
            var employees = context.Employees
                .Where(s => s.Salary > 50000)
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .OrderBy(f => f.FirstName)
                .ToList();

            employees.ForEach(e => sb.AppendLine($"{e.FirstName} - {e.Salary:F2}"));

            return sb.ToString().TrimEnd();
        }

        //05.Employees from Research and Development
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var sb = new StringBuilder();
            var employees = context.Employees
                .Where(d => d.Department.Name == "Research and Development")
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.Department.Name,
                    e.Salary
                })
                .OrderBy(s => s.Salary)
                .ThenByDescending(f => f.FirstName)
                .ToList();

            employees.ForEach(e => sb.AppendLine($"{e.FirstName} {e.LastName} from Research and Development - ${e.Salary:F2}"));

            return sb.ToString().TrimEnd();
        }

        //06.Adding a New Address and Updating Employee
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var address = new Address();
            address.AddressText = "Vitoshka 15";
            address.TownId = 4;

            var employees = context.Employees.FirstOrDefault(e => e.LastName == "Nakov");

            employees.Address = address;

            context.SaveChanges();
            var sb = new StringBuilder();

            var employee = context.Employees
                .OrderByDescending(a => a.AddressId)
                .Select(a => a.Address.AddressText)
                .Take(10)
                .ToList();

            employee.ForEach(e => sb.AppendLine(e));

            return sb.ToString().TrimEnd();
        }

        //07.Employees and Projects
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var dateFormat = @"M/d/yyyy h:mm:ss tt";

            var employees = context.Employees
                .Where(x => !x.EmployeesProjects.Any() || x.EmployeesProjects.Any(y => y.Project.StartDate.Year >= 2001 && y.Project.StartDate.Year <= 2003))
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    ManagerFirstName = x.Manager.FirstName,
                    ManagerLastName = x.Manager.LastName,
                    Projects = x.EmployeesProjects
                        .Select(y => new
                        {
                            ProjectName = y.Project.Name,
                            StartDate = y.Project.StartDate.ToString(dateFormat, CultureInfo.InvariantCulture),
                            EndDate = y.Project.EndDate.HasValue ?
                            y.Project.EndDate.Value.ToString(dateFormat, CultureInfo.InvariantCulture)
                            : "not finished"
                        }).ToList()
                })
                .Take(10)
                .ToList();

            foreach (var employee in employees)
            {
                sb.AppendLine(
                    $"{employee.FirstName} {employee.LastName} - Manager: {employee.ManagerFirstName} {employee.ManagerLastName}");

                foreach (var project in employee.Projects)
                {
                    sb.AppendLine(
                        $"--{project.ProjectName} - {project.StartDate} - {project.EndDate}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //08.Addresses by Town
        public static string GetAddressesByTown(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var addresses = context.Addresses
                .Where(e => e.Employees.Any())
                .Select(x => new
                {
                    TownName = x.Town.Name,
                    x.AddressText,
                    EmployeeCount = x.Employees.Count()
                })
                .OrderByDescending(x => x.EmployeeCount)
                .ThenBy(x => x.TownName)
                .ThenBy(x => x.AddressText)
                .Take(10)
                .ToList();

            foreach (var address in addresses)
            {
                sb.AppendLine($"{address.AddressText}, {address.TownName} - {address.EmployeeCount} employees");
            }

            return sb.ToString().TrimEnd();
        }

        //09.Employee 147
        public static string GetEmployee147(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    EmployeeProject = e.EmployeesProjects.Select(ep => new
                    {
                        ep.Project.Name
                    })
                    .OrderBy(ep => ep.Name)
                    .ToList()
                })
                .ToList();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

                foreach (var project in employee.EmployeeProject)
                {
                    sb.AppendLine($"{project.Name}");
                }
            }
            return sb.ToString().TrimEnd();
        }

        //10.Departments with More Than 5 Employees
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var contextInfo = context.Departments
               .Where(x => x.Employees.Count > 5)
               .Select(d => new
               {
                   d.Name,
                   d.Manager.FirstName,
                   d.Manager.LastName,
                   Employee = d.Employees.Select(e => new
                   {
                       e.FirstName,
                       e.LastName,
                       e.JobTitle
                   })
                   .OrderBy(x => x.JobTitle)
                   .ToList()
               })
               .OrderBy(x => x.Employee.Count)
               .ToList();

            foreach (var info in contextInfo)
            {
                sb.AppendLine($"{info.Name} – {info.FirstName} {info.LastName}");
                foreach (var employee in info.Employee)
                {
                    sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //11.Find Latest 10 Projects
        public static string GetLatestProjects(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var dateFormat = @"M/d/yyyy h:mm:ss tt";

            var projectInfo = context.Projects
                .OrderByDescending(sd => sd.StartDate)
                .Take(10)
                .Select(pi => new
                {
                    pi.Name,
                    pi.Description,
                    StartDate = pi.StartDate.ToString(dateFormat, CultureInfo.InvariantCulture),
                })
                .OrderBy(n => n.Name)
                .ToList();

            foreach (var project in projectInfo)
            {
                sb.AppendLine(project.Name);
                sb.AppendLine(project.Description);
                sb.AppendLine(project.StartDate);
            }

            return sb.ToString().TrimEnd();

        }

        //12.Increase Salaries
        public static string IncreaseSalaries(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var increaseSalary = context.Employees
                .Where(x => x.Department.Name == "Engineering" ||
                            x.Department.Name == "Marketing" ||
                            x.Department.Name == "Tool Design" ||
                            x.Department.Name == "Information Services")
                .OrderBy(f => f.FirstName)
                .ThenBy(l => l.LastName)
                .ToList();

            increaseSalary.ForEach(s => s.Salary *= 1.12M);

            increaseSalary.ForEach(i => sb.AppendLine($"{i.FirstName} {i.LastName} (${i.Salary:F2})"));

            return sb.ToString().TrimEnd();

        }

        //13.Find Employees by First Name Starting With Sa
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employees = context.Employees
                .Where(n => n.FirstName.StartsWith("Sa"))
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .OrderBy(f => f.FirstName)
                .ThenBy(l => l.LastName)
                .ToList();

            employees.ForEach(e => sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:F2})"));

            return sb.ToString().TrimEnd();
        }

        //14.Delete Project by Id
        public static string DeleteProjectById(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var projects = context.Projects.FirstOrDefault(p => p.ProjectId == 2);

            var employeeProjects = context.EmployeesProjects
                .Where(ep => ep.ProjectId == 2)
                .ToList();

            employeeProjects.ForEach(ep => context.EmployeesProjects.Remove(ep));

            context.Projects.Remove(projects);

            context.SaveChanges();

            foreach (var project in context.Projects)
            {
                sb.AppendLine(project.Name);
            }

            return sb.ToString().TrimEnd();
        }

        //15.Remove Town
        public static string RemoveTown(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var towns = context.Employees
                .Where(x => x.Address.Town.Name == "Seattle")
                .ToList();

            var addresses = context.Addresses
                .Where(x => x.Town.Name == "Seattle")
                .ToList();

            var employee = context.Employees
                .Select(x => x.Address)
                .ToList();

            towns.ForEach(x => x.AddressId = null);


            sb.AppendLine($"{addresses.Count} addresses in Seattle were deleted");

            addresses.ForEach(x => x.TownId = null);

            var town = context.Towns.FirstOrDefault(x => x.Name == "Seattle");
            context.Towns.Remove(town);

            context.SaveChanges();

            return sb.ToString().TrimEnd();




        }
    }
}

