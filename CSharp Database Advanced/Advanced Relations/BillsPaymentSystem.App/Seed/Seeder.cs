using BillsPaymentSystem.App.Enums;
using BillsPaymentSystem.Data;
using BillsPaymentSystem.Models;
using BillsPaymentSystem.Models.Enums;

namespace BillsPaymentSystem.App.Seed
{
    public class Seeder
    {
        private readonly BillsPaymentSystemContext context;

        public Seeder(BillsPaymentSystemContext context)
        {
            this.context = context;
        }

        public static void SeedDb(BillsPaymentSystemContext context, string command)
        {
            SeedUsers(context, command);
        }
        private static void SeedUsers(BillsPaymentSystemContext context, string command)
        {
            if (command == Commands.Seed.ToString().ToLower())
            {
                var user = new User
                {
                    FirstName = "Guy",
                    LastName = "Gilbert",
                    Email = "guy_gilbert@gmail.com",
                    Password = PasswordHelper.HashPassword("guygilbert123"),
                    PaymentMethods = new List<PaymentMethod>()
                    {
                        new()
                        {
                            Type = PaymentMethodType.BankAccount,
                            BankAccount = new BankAccount
                            {
                                Balance = 2000.00m,
                                BankName = "Unicredit Bulbank",
                                SWIFT = "UNCRBGSF"
                            }
                        },

                        new()
                        {
                            Type = PaymentMethodType.BankAccount,
                            BankAccount = new BankAccount
                            {
                                Balance = 1000.00m,
                                BankName = "First Investment Bank",
                                SWIFT = "FINVBGSF"
                            }
                        },

                        new()
                        {
                            Type = PaymentMethodType.CreditCard,
                            CreditCard = new CreditCard
                            {
                                Limit = 800.00m,
                                MoneyOwed = 100.00m,
                                ExpirationData = new DateTime(2020, 3, 1)
                            }
                        }
                    }
                };

                var existingUser = context.Users.FirstOrDefault(u => u.FirstName == user.FirstName);

                if (existingUser == null)
                {
                    context.Users.Add(user);
                    context.SaveChanges();
                    Console.WriteLine("User added to the database.");
                }
                else
                {
                    Console.WriteLine($"User with name {user.FirstName} {user.LastName} already exists in the database. Skipping addition.");
                }
            }
        }
    }
}
