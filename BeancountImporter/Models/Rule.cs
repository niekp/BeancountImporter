using System;
using System.Collections.Generic;

namespace BeancountImporter.Models
{
    public class Rule
    {
        public string ExpenseAccount { get; set; }
        public string AssetAccount { get; set; }
        public string Title { get; set; }

        public Dictionary<SearchKey, string> Search { get; set; }

        public enum SearchKey
        {
            Description,
            Person,
        };

    }
}
