using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;
using Neurotec.Licensing;

namespace PassportReaderNS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //ToggleConfigEncryption("PassportReader.exe");

            //const string Components = "Biometrics.FingerExtraction,Biometrics.FingerMatching,Devices.FingerScanners,Images.WSQ";
            //const string Components = "Biometrics.FingerExtraction,Biometrics.FingerMatching,Images.WSQ";
            //const string Components = "Biometrics.FingerExtraction,Biometrics.FingerMatching,Devices.FingerScanners,Images.WSQ";
            const string Components = "Images.WSQ";

            try
            {
                foreach (string component in Components.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    NLicense.ObtainComponents("/local", "5000", component);
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            catch (Exception ex)
            {
			    while ((ex is AggregateException) && (ex.InnerException != null))
				    ex = ex.InnerException;

			    MessageBox.Show(ex.ToString(), null, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                NLicense.ReleaseComponents(Components);
            }
        }

        static void ToggleConfigEncryption(string exeConfigName)
        {
            // Takes the executable file name without the
            // .config extension.
            try
            {
                // Open the configuration file and retrieve 
                // the connectionStrings section.
                Configuration config = ConfigurationManager.OpenExeConfiguration(exeConfigName);

                ConnectionStringsSection section = config.GetSection("connectionStrings")
                    as ConnectionStringsSection;

                if (section.SectionInformation.IsProtected)
                {
                    // Remove encryption.
                    section.SectionInformation.UnprotectSection();
                }
                else
                {
                    // Encrypt the section.
                    section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                }
                // Save the current configuration.
                config.Save();

                Console.WriteLine("Protected={0}", section.SectionInformation.IsProtected);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
