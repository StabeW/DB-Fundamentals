using Microsoft.Extensions.DependencyInjection;
using MyApp.Core.Contracts;

namespace MyApp.Core
{
    public class Engine : IEngine
    {
        private IServiceProvider provider;

        public Engine(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public void Run()
        {
            while (true)
            {
                var inputArgs = Console.ReadLine()
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .ToArray();

                var commandInterpreter = provider.GetService<ICommandInterpreter>();

                var result = commandInterpreter.Read(inputArgs);

                Console.WriteLine(result);


            }
        }
    }
}
