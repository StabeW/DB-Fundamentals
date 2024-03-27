using MyApp.Core.Commands.Contracts;
using MyApp.Core.Contracts;
using System.Reflection;

namespace MyApp.Core
{
    public class CommandInterpreter : ICommandInterpreter
    {
        private const string Suffix = "Command";
        private readonly IServiceProvider serviceProvider;

        public CommandInterpreter(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public string Read(string[] inputArgs)
        {
            var commandName = inputArgs[0] + Suffix;
            var commandParams = inputArgs.Skip(1).ToArray();

            var type = Assembly.GetCallingAssembly()
                .GetTypes()
                .FirstOrDefault(t => t.Name == commandName);

            if (type == null)
            {
                throw new ArgumentNullException("Invalid command!");
            }

            var constructor = type.GetConstructors()
                .FirstOrDefault();

            var constructorParams = constructor
                .GetParameters()
                .Select(x => x.ParameterType)
                .ToArray();

            var service = constructorParams
                .Select(this.serviceProvider.GetService)
                .ToArray();

            var command = (ICommand)Activator.CreateInstance(type, service);

            string result = command.Execute(commandParams);

            return result;
        }
    }
}
