using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using PSCBioIdentification.ConfigurationService;
using System.Collections.Specialized;
using System.ServiceModel;
using System.ServiceModel.Configuration;

namespace PSCBioIdentification
{
    static class MyConfigurationSettings
    {
        static private NameValueCollection appSettingsCollection = null;
        static private ConnectionStringSettingsCollection connectionStringCollection = null;
        
        static MyConfigurationSettings()
        {
            ConfigurationServiceClient configurationServiceClient = null;
            //ServiceHost configurationServiceClient = null;
            Dictionary<string, string> settings = null;
            String baseAddress = null;
            ClientSection serviceModelClient = null;

            if (ConfigurationManager.AppSettings["ConfigurationProvider"] == "remote")
            {
                baseAddress = ConfigurationManager.AppSettings["endPointServer"];
                //configurationServiceClient.Endpoint.Address

                //var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                //var serviceModel = configFile.SectionGroups["system.serviceModel"];
                //var clientSection = serviceModel.Sections["client"];

                //var serviceModelClient = ConfigurationManager.GetSection("system.serviceModel/client");

                serviceModelClient = ConfigurationManager.GetSection("system.serviceModel/client") as ClientSection;
                //foreach (ChannelEndpointElement cs in serviceModelClient.Endpoints)
                //{
                //    var address = cs.Address;
                //}

                String serviceName = "BasicHttpBinding_IConfigurationService";

                Uri endPoint = serviceModelClient.Endpoints.Cast<ChannelEndpointElement>()
                                                           .SingleOrDefault(endpoint => endpoint.Name == serviceName).Address;

                if (baseAddress.Length != 0)
                    baseAddress = endPoint.Scheme + "://" + baseAddress + ":" + endPoint.Port + endPoint.PathAndQuery;
                //baseAddress = endPoint.Scheme + "://" + baseAddress + ":" + endPoint.Port + "/" + endPoint.Host + endPoint.PathAndQuery;
                else
                    baseAddress = endPoint.AbsoluteUri;

                configurationServiceClient = new ConfigurationServiceClient(serviceName, baseAddress);
                //configurationServiceClient = new ServiceHost(typeof(ConfigurationServiceClient), new Uri(baseAddress));

                appSettingsCollection = new NameValueCollection();

                settings = configurationServiceClient.AppSettings();
                foreach (var key in settings.Keys)
                {
                    appSettingsCollection.Add(key, settings[key]);
                }

                settings.Clear();

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
