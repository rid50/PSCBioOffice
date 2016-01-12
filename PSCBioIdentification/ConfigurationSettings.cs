using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using PSCBioIdentification.ConfigurationService;
using System.Collections.Specialized;

namespace PSCBioIdentification
{
    static class MyConfigurationSettings
    {
        static private NameValueCollection appSettingsCollection = null;
        static private ConnectionStringSettingsCollection connectionStringCollection = null;
        
        static MyConfigurationSettings()
        {
            ConfigurationServiceClient configurationServiceClient = null;
            Dictionary<string, string> settings = null;

            if (ConfigurationManager.AppSettings["ConfigurationProvider"] == "remote")
            {
                configurationServiceClient = new ConfigurationServiceClient();

                appSettingsCollection = new NameValueCollection();

                settings = configurationServiceClient.AppSettings();
                foreach (var key in settings.Keys)
                {
                    appSettingsCollection.Add(key, settings[key]);
                }

                settings.Clear();
            }

            if (ConfigurationManager.AppSettings["ConfigurationProvider"] == "remote")
            {
                connectionStringCollection = new ConnectionStringSettingsCollection();

                settings = configurationServiceClient.ConnectionStrings();
                foreach (var key in settings.Keys)
                {
                    connectionStringCollection.Add(new ConnectionStringSettings(key, settings[key]));
                }
            }
        }

        static public NameValueCollection AppSettings
        {
            get
            {
                if (ConfigurationManager.AppSettings["ConfigurationProvider"] == "remote")
                    return appSettingsCollection;
                else
                    return ConfigurationManager.AppSettings;
            }
        }

        static public ConnectionStringSettingsCollection ConnectionStrings
        {
            get
            {
                if (ConfigurationManager.AppSettings["ConfigurationProvider"] == "remote")
                    return connectionStringCollection;
                else
                    return ConfigurationManager.ConnectionStrings;
            }
        }
    }
}
