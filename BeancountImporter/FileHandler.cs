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
        private string _beancountFile;
        private string _exportFile;

        public FileHandler()
        {
            LocateBeancount();
            LocateExportFile();
            Console.WriteLine(_exportFile);
        }

        private string GetHomeDirectory()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        }

        private void LocateBeancount()
        {
            _beancountFile = String.Format("{0}/kasboek/kasboek2.beancount", GetHomeDirectory());
            if (!File.Exists(_beancountFile))
            {
                throw new FileNotFoundException("Beancount file not found");
            }
        }

        private void LocateExportFile()
        {
            string downloadFolder = String.Concat(GetHomeDirectory(), "/Downloads");

            var exportFile = Directory.GetFiles(downloadFolder, "*.csv").FirstOrDefault();
            if (exportFile != null)
            {
                _exportFile = exportFile;
            }
        }

        public List<ExportTransaction> GetExportTransactions()
        {
            if (_exportFile == null || _exportFile == String.Empty)
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
    }
}
