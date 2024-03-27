using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Core;
using MyApp.Core.Contracts;
using MyApp.Data;

public class StartUp
{
    public static void Main(string[] args)
    {
        IServiceProvider services = ConfigureServices();
        IEngine engine = new Engine(services);
        engine.Run();
    }

    private static IServiceProvider ConfigureServices()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddDbContext<MyAppContext>(db => db.UseSqlServer(Configuration.SqlConnection));

        serviceCollection.AddTransient<ICommandInterpreter, CommandInterpreter>();
        serviceCollection.AddTransient<Mapper>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        return serviceProvider;
    }
}