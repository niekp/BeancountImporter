using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace BeancountImporter.Models
{
    public class Configuration
    {
        private IConfigurationRoot _configuration;
        public Configuration()
        {
            _configuration = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json", true, true)
              .Build();

            var HomeDirectory = GetHomeDirectory();
            DefaultAssetAccount = _configuration["DefaultAssetAccount"];
            DefaultExpenseAccount = _configuration["DefaultExpenseAccount"];
            DownloadsDirectory = _configuration["DownloadsDirectory"].Replace("~", HomeDirectory);
            BeancountFile = _configuration["BeancountFile"].Replace("~", HomeDirectory);
            RulesetFile = _configuration["RulesetFile"].Replace("~", HomeDirectory);
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(DefaultAssetAccount)
                && !string.IsNullOrEmpty(DefaultExpenseAccount)
                && !string.IsNullOrEmpty(DownloadsDirectory)
                && Directory.Exists(DownloadsDirectory)
                && File.Exists(BeancountFile);
        }

        private string GetHomeDirectory()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        }

        public string DefaultAssetAccount { get; set; }
        public string DefaultExpenseAccount { get; set; }
        public string DownloadsDirectory { get; set; }
        public string BeancountFile { get; set; }
        public string RulesetFile { get; set; }
    }
}
