using System;

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
            var transactions = fileHandler.GetExportTransactions();


            foreach (var transaction in transactions)
            {
                transaction.ExpenseAccount = configuration.DefaultExpenseAccount;
                transaction.AssetAccount = configuration.DefaultAssetAccount;
                fileHandler.WriteToBeancount(transaction);
            }
        }
    }
}
