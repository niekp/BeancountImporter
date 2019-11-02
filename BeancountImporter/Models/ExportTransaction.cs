using System;
using System.Text;
using CsvHelper.Configuration.Attributes;

namespace BeancountImporter.Models
{
    public class ExportTransaction
    {
        private string description;

        [Index(0)]
        public string Date { get; set; }

        [Index(3)]
        public string Person { get; set; }

        [Index(10)]
        public string Amount { get; set; }

        [Index(17)]
        public string Description
        {
            get
            {
                return description.Replace("'", "");
            }
            set => description = value;
        }

        public string AssetAccount { get; set; }

        public string ExpenseAccount { get; set; }

        public string Beancount
        {
            get
            {
                StringBuilder output = new StringBuilder();

                var dateArray = Date.Split("-");

                output.Append(String.Format("{0}-{1}-{2} * \"{3}\"\n",
                    dateArray[2], dateArray[1], dateArray[0], Description));

                output.Append(String.Format(" {0} {1} EUR\n", AssetAccount, Amount));
                output.Append(String.Format(" {0}\n\n", ExpenseAccount));
                return output.ToString();
            }
        }
    }
}
