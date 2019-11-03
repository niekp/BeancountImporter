using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BeancountImporter.Models;
using CsvHelper;

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

            using (var reader = new StreamReader(_exportFile))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.HasHeaderRecord = false;
                csv.Configuration.MissingFieldFound = null;
                csv.Configuration.Delimiter = ",";
                return csv.GetRecords<ExportTransaction>().ToList();
            }
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
