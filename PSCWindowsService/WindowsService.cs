using System.ServiceProcess;
using System.ServiceModel;
using System;
using System.Configuration;
using System.ServiceModel.Configuration;

namespace PSCWindowsService
{
    class WindowsService : ServiceBase
    {
        public ServiceHost serviceHost = null;

        public WindowsService()
        {
            // Name the Windows Service
            ServiceName = "PSCWindowsService";

        }

        public static void Main()
        {
            ServiceBase.Run(new WindowsService());
        }

        // Start the Windows service.
        protected override void OnStart(string[] args)
        {

            String endPointHost = ConfigurationManager.AppSettings["endPointHost"];

            //String baseAddress = ConfigurationManager.AppSettings["endPointServer"];
            //configurationServiceClient.Endpoint.Address

            //var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //var serviceModel = configFile.SectionGroups["system.serviceModel"];
            //var clientSection = serviceModel.Sections["client"];

            //var serviceModelClient = ConfigurationManager.GetSection("system.serviceModel/client");

            //ClientSection serviceModelClient = ConfigurationManager.GetSection("system.serviceModel/client") as ClientSection;
            //foreach (ChannelEndpointElement cs in serviceModelClient.Endpoints)
            //{
            //    var address = cs.Address;
            //}

            //String serviceName = "BasicHttpBinding_IConfigurationService";

//#if DEBUG
//            System.Diagnostics.Debugger.Launch();
//#endif

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ServiceModelSectionGroup serviceModelSection = ServiceModelSectionGroup.GetSectionGroup(config);
            ClientSection serviceModelClientSection = serviceModelSection.Client;
            if (serviceModelClientSection.Endpoints[0].Address.Host != endPointHost)
            {
                foreach (ChannelEndpointElement endPoint in serviceModelClientSection.Endpoints)
                {
                    var uri = new Uri(endPoint.Address.Scheme + "://" + endPointHost + ":" + endPoint.Address.Port + endPoint.Address.PathAndQuery);
                    endPoint.Address = uri;
                    //endPoint.Address = new Uri(endPoint.Address.Scheme + "://" + endPointAddress + ":" + endPoint.Address.Port + endPoint.Address.PathAndQuery);
                }
                //serviceModelClientSection.Endpoints.Cast<ChannelEndpointElement>()
                //                                         .Select(endpoint => { endpoint.Address.Host = endPointHost; return endpoint; });

                //ChannelEndpointElement endPoint = serviceModelClientSection.Endpoints[0];
                //var uri = new Uri(endPoint.Address.Scheme + "://" + endPointHost + ":" + endPoint.Address.Port + endPoint.Address.PathAndQuery);

                //serviceModelClientSection.Endpoints[0].Address = uri;
                config.Save();

                ConfigurationManager.RefreshSection(serviceModelClientSection.SectionInformation.SectionName);
            }

            if (serviceHost != null)
            {
                serviceHost.Close();
            }

            // Create a ServiceHost for the CalculatorService type and 
            // provide the base address.
            serviceHost = new ServiceHost(typeof(CommandService));

            // Open the ServiceHostBase to create listeners and start 
            // listening for messages.
            serviceHost.Open();
        }

        protected override void OnStop()
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
                serviceHost = null;
            }
        }
    }
}
