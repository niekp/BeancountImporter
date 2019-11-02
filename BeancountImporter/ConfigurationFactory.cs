using System;
namespace BeancountImporter
{
    public class ConfigurationFactory
    {
        Configuration configuration;

        public Configuration GetConfiguration()
        {
            if (configuration == null)
            {
                configuration = new Configuration();
            }

            return configuration;
        }
    }
}
