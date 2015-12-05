using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace CommonService
{
    public class ConfigurationService : IConfigurationService
    {
        public System.Configuration.ConnectionStringSettingsCollection ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings; }
        }

        public Dictionary<string, string> AppSettings()
        {
            Dictionary<string, string> settings = new Dictionary<string, string>();

            foreach (var key in ConfigurationManager.AppSettings.AllKeys)
            {
                settings.Add(key, ConfigurationManager.AppSettings[key]);
            }

            return settings; 
        }

        public Dictionary<string, string> ConnectionStrings()
        {
            Dictionary<string, string> settings = new Dictionary<string, string>();

            foreach (ConnectionStringSettings cs in ConfigurationManager.ConnectionStrings)
            {
                settings.Add(cs.Name, cs.ConnectionString);
            }

            return settings;
        }
        
        public string getAppSetting(string key)
        {
            var setting = ConfigurationManager.AppSettings[key];

            // If we didn't find setting, try to load it from current dll's config file
            if (string.IsNullOrEmpty(setting))
            {
                var assemly = System.Reflection.Assembly.GetExecutingAssembly();
                var configuration = ConfigurationManager.OpenExeConfiguration(assemly.Location);
                var value = configuration.AppSettings.Settings[key];
                if (value != null)
                {
                    setting = value.Value;
                }
            }

            return setting;
        }

        public string getConnectionString(string name = "ConnectionString")
        {
            string connectionString = null;
            var connectionStringSettings = ConfigurationManager.ConnectionStrings[name];
            if (connectionStringSettings != null)
                connectionString = connectionStringSettings.ConnectionString;

            // If we didn't find setting, try to load it from current dll's config file
            if (string.IsNullOrEmpty(connectionString))
            {
                var assemly = System.Reflection.Assembly.GetExecutingAssembly();
                var configuration = ConfigurationManager.OpenExeConfiguration(assemly.Location);
                var value = configuration.ConnectionStrings.ConnectionStrings[name];
                if (value != null)
                {
                    connectionString = value.ConnectionString;
                }
            }

            if (System.Configuration.ConfigurationManager.AppSettings["Enroll"] == "service")
                connectionString += ";UID=rid50;Password=faiha-642;";

            return connectionString;
        }
    }
}
