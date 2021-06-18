using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using BeancountImporter.Models;
using CsvHelper;
using CsvHelper.Configuration;

namespace BeancountImporter
{
    public class FileHandler
    {
        private string _exportFile;
        private Configuration _configuration;

        public FileHandler(Configuration configuration)
        {
            _configuration = configuration;
            LocateExportFile();
        }

        private void LocateExportFile()
        {
            var exportFile = Directory.GetFiles(_configuration.DownloadsDirectory, "*.csv").FirstOrDefault();
            if (exportFile != null)
            {
                _exportFile = exportFile;
            }
        }

        public List<ExportTransaction> GetExportTransactions()
        {
            if (string.IsNullOrEmpty(_exportFile))
            {
                return new List<ExportTransaction>();
            }

            
            var config = new CsvConfiguration(new CultureInfo("nl-NL")) {
                HasHeaderRecord = false,
                MissingFieldFound = null,
                Delimiter = ","
            };

            using var reader = new StreamReader(_exportFile);
            using var csv = new CsvReader(reader, config);
            
            return csv.GetRecords<ExportTransaction>().ToList();
        }

        public void WriteToBeancount(ExportTransaction exportTransaction)
        {
            using (var writer = File.AppendText(_configuration.BeancountFile))
            {
                writer.Write(exportTransaction.Beancount);
            }
        }

        public void MarkAsDone()
        {
            if (string.IsNullOrEmpty(_exportFile))
            {
                return;
            }

            File.Move(_exportFile, String.Concat(_exportFile, ".done"));
        }
    }
}
