using System;
using System.Linq;
using BeancountImporter.Models;

namespace BeancountImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new Configuration();
            if (!configuration.IsValid())
            {
                Console.WriteLine("Invalid configuration.");
                return;
            }

            var fileHandler = new FileHandler(configuration);
            var ruleset = new Ruleset(configuration);

            var transactions = fileHandler.GetExportTransactions();

            foreach (var transaction in transactions.OrderBy(t => t.Date))
            {
                var rule = ruleset.FindRule(transaction);

                if (rule == null) {
                    transaction.ExpenseAccount = configuration.DefaultExpenseAccount;
                    transaction.AssetAccount = configuration.DefaultAssetAccount;
                } else {
                    transaction.ExpenseAccount = rule.ExpenseAccount ?? configuration.DefaultExpenseAccount;
                    transaction.AssetAccount = rule.AssetAccount ?? configuration.DefaultAssetAccount;
                    transaction.Description = rule.Title ?? transaction.Description;
                }

                fileHandler.WriteToBeancount(transaction);
            }

            Console.WriteLine(String.Format("{0} transactions imported", transactions.Count));
            fileHandler.MarkAsDone();
        }
    }
}
