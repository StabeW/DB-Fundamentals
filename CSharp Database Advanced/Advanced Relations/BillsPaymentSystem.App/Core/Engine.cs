using BillsPaymentSystem.App.Core.Contracts;
using BillsPaymentSystem.App.Read;
using BillsPaymentSystem.App.Seed;
using BillsPaymentSystem.Data;

namespace BillsPaymentSystem.App.Core
{
    public class Engine : IEngine
    {
        public async void Run()
        {
            using var context = new BillsPaymentSystemContext();
            var command = Console.ReadLine();

            Seeder.SeedDb(context, command);
            Reader.Read(command);
        }

    }
}

