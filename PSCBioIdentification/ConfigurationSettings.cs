using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using PSCBioIdentification.ConfigurationService;
using System.Collections.Specialized;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.Reflection;
using System.ServiceModel.Description;

namespace PSCBioIdentification
{
    public class MyConfigurationSettings
    {
        static private NameValueCollection appSettingsCollection = null;
        static private ConnectionStringSettingsCollection connectionStringCollection = null;

        public MyConfigurationSettings() { }

        static MyConfigurationSettings()
        {
            ConfigurationServiceClient configurationServiceClient = null;
            Dictionary<string, string> settings = null;

            //String endPointHost = ConfigurationManager.AppSettings["endPointHost"];
            //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //ServiceModelSectionGroup serviceModelSection = ServiceModelSectionGroup.GetSectionGroup(config);
            //ClientSection serviceModelClientSection = serviceModelSection.Client;
            //if (serviceModelClientSection.Endpoints[0].Address.Host != endPointHost)
            //{
            //    foreach (ChannelEndpointElement endPoint in serviceModelClientSection.Endpoints)
            //    {
            //        var uri = new Uri(endPoint.Address.Scheme + "://" + endPointHost + ":" + endPoint.Address.Port + endPoint.Address.PathAndQuery);
            //        endPoint.Address = uri;
            //        //endPoint.Address = new Uri(endPoint.Address.Scheme + "://" + endPointAddress + ":" + endPoint.Address.Port + endPoint.Address.PathAndQuery);
            //    }
            //    //serviceModelClientSection.Endpoints.Cast<ChannelEndpointElement>()
            //        //                                         .Select(endpoint => { endpoint.Address.Host = endPointHost; return endpoint; });

            //    //ChannelEndpointElement endPoint = serviceModelClientSection.Endpoints[0];
            //    //var uri = new Uri(endPoint.Address.Scheme + "://" + endPointHost + ":" + endPoint.Address.Port + endPoint.Address.PathAndQuery);

            //    //serviceModelClientSection.Endpoints[0].Address = uri;
            //    config.Save();

            //    ConfigurationManager.RefreshSection(serviceModelClientSection.SectionInformation.SectionName);
            //}

            configurationServiceClient = new ConfigurationServiceClient();
            //ServiceEndpoint serviceEndpoint = configurationServiceClient.Endpoint;

            //Uri uri = new Uri(serviceEndpoint.Address.Uri.Scheme + "://" + endPointHost + ":" + serviceEndpoint.Address.Uri.Port + serviceEndpoint.Address.Uri.PathAndQuery);
            //configurationServiceClient.Endpoint.Address = new EndpointAddress(uri.ToString());

            //string absoluteUri = configurationServiceClient.Endpoint.Address.Uri.AbsoluteUri;
            string errorMessage;
            //if (!Form1.IsServiceAvailable(absoluteUri, out errorMessage))
            if (!Form1.IsServiceAvailable(configurationServiceClient, out errorMessage))
            {
                configurationServiceClient.Close();
                configurationServiceClient = null;
                throw new Exception(errorMessage);
                //throw new Exception(errorMessage + " : " + absoluteUri);
            }
            //configurationServiceClient = new ConfigurationServiceClient(serviceName, endPointAddress);
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

            configurationServiceClient.Close();
        }

        static public NameValueCollection AppSettings
        {
            get
            {
                if (appSettingsCollection != null)
                    return appSettingsCollection;
                else
                    return ConfigurationManager.AppSettings;
            }
        }

        static public ConnectionStringSettingsCollection ConnectionStrings
        {
            get
            {
                if (connectionStringCollection != null)
                    return connectionStringCollection;
                else
                    return ConfigurationManager.ConnectionStrings;
            }
        }
    }
}
