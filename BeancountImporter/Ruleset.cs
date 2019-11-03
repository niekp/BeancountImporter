using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BeancountImporter.Models;
using Newtonsoft.Json;

namespace BeancountImporter
{
    public class Ruleset
    {
        private List<Rule> _ruleset = new List<Rule>();

        public Ruleset(Configuration configuration)
        {
            if (string.IsNullOrEmpty(configuration.RulesetFile))
            {
                return;
            }

            using (StreamReader r = new StreamReader(configuration.RulesetFile))
            {
                string json = r.ReadToEnd();
                _ruleset = JsonConvert.DeserializeObject<List<Rule>>(json);
            }
        }

        public Rule FindRule(ExportTransaction transaction)
        {
            foreach (var rule in _ruleset)
            {
                bool match = true;
                foreach (var search in rule.Search)
                {
                    if (search.Key == Rule.SearchKey.Description
                        && !transaction.Description.ToLower().Contains(search.Value.ToLower()))
                    {
                        match = false;
                    }

                    if (search.Key == Rule.SearchKey.Person
                        && !transaction.Person.ToLower().Contains(search.Value.ToLower()))
                    {
                        match = false;
                    }
                }

                if (match)
                {
                    return rule;
                }
            }

            return null;   
        }

    }
}
