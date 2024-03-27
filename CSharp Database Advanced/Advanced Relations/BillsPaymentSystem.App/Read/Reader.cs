using BillsPaymentSystem.App.Enums;
using BillsPaymentSystem.Data;
using BillsPaymentSystem.Models.Enums;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BillsPaymentSystem.App.Read
{
    public class Reader
    {
        public static async void Read(string command)
        {
            using var context = new BillsPaymentSystemContext();
            {
                if (command.Equals(Commands.Read.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    var filePath = "../../../text.txt";

                    using (StreamWriter writer = File.CreateText(filePath))
                    {
                        var userId = int.Parse(Console.ReadLine());

                        var user = context.Users
                            .Include(u => u.PaymentMethods)
                                .ThenInclude(pm => pm.BankAccount)
                            .Include(u => u.PaymentMethods)
                                .ThenInclude(pm => pm.CreditCard)
                            .FirstOrDefault(u => u.UserId == userId);

                        if (user != null)
                        {
                            await writer.WriteLineAsync($"User: {user.FirstName} {user.LastName}");

                            var bankAccounts = user.PaymentMethods
                                .Where(pm => pm.Type == PaymentMethodType.BankAccount)
                                .OrderBy(pm => pm.BankAccountId)
                                .Select(pm => pm.BankAccount);

                            await writer.WriteLineAsync("Bank Accounts:");

                            foreach (var bankAccount in bankAccounts)
                            {
                                await writer.WriteLineAsync($"-- ID: {bankAccount.BankAccountId}");
                                await writer.WriteLineAsync($"--- Balance: {bankAccount.Balance:C2}");
                                await writer.WriteLineAsync($"--- Bank: {bankAccount.BankName}");
                                await writer.WriteLineAsync($"--- SWIFT: {bankAccount.SWIFT}");
                            }

                            var creditCards = user.PaymentMethods
                                .Where(pm => pm.Type == PaymentMethodType.CreditCard)
                                .OrderBy(pm => pm.CreditCardId)
                                .Select(pm => pm.CreditCard);

                            await writer.WriteLineAsync("Credit Cards:");

                            foreach (var creditCard in creditCards)
                            {
                                await writer.WriteLineAsync($"-- ID: {creditCard.CreditCardId}");
                                await writer.WriteLineAsync($"--- Limit: {creditCard.Limit:C2}");
                                await writer.WriteLineAsync($"--- Money Owed: {creditCard.MoneyOwed:C2}");
                                await writer.WriteLineAsync($"--- Limit Left: {creditCard.LimitLeft:C2}");
                                await writer.WriteLineAsync($"--- Expiration Date: {creditCard.ExpirationData:yyyy/MM}");
                            }
                        }
                        else
                        {
                            await writer.WriteLineAsync($"User with id {userId} not found!");
                        }
                    }
                }
            }
        }

    }
}
