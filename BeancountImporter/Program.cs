using System;

namespace BeancountImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileHandler = new FileHandler();

            var transactions = fileHandler.GetExportTransactions();

            foreach (var transaction in transactions)
            {
                Console.WriteLine(transaction.Beancount);
            }
        }
    }
}
