using System;
using System.Windows.Forms;
using Neurotec.Licensing;

namespace PSCBioIdentification
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            //const string Components = "Biometrics.FingerExtraction,Biometrics.FingerMatching,Devices.FingerScanners,Images.WSQ,Biometrics.FingerSegmentation,Biometrics.FingerQualityAssessmentBase";
            //try
            //{
            //    foreach (string component in Components.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            //    {
            //        NLicense.ObtainComponents(LicensePanel.Address, LicensePanel.Port, component);
            //    }

            //    Application.EnableVisualStyles();
            //    Application.SetCompatibleTextRenderingDefault(false);
            //    Application.Run(new Form1());
            //}
            //catch (Exception ex)
            //{
            //    Utils.ShowException(ex);
            //}
            //finally
            //{
            //    NLicense.ReleaseComponents(Components);
            //}

            //IList<string> licensesMain = new List<string>(new string[] { "Biometrics.FingerExtraction", "Biometrics.FingerMatching" });
//            IList<string> licensesMain = new List<string>(new string[] { "FingersExtractor", "FingersMatcher" });
//            IList<string> licensesBss = new List<string>(new string[] { "FingersBSS" });

            //const string Components = "Biometrics.FingerExtraction,Biometrics.FingerMatching,Devices.FingerScanners,Images.WSQ,Biometrics.FingerSegmentation,Biometrics.FingerQualityAssessmentBase";
            //const string Components = "Biometrics.FingerExtraction,Biometrics.FingerMatching,Biometrics.FingerDetection,Devices.FingerScanners,Images.WSQ";
            const string Components = "Biometrics.FingerExtraction,Biometrics.FingerMatching,Devices.FingerScanners,Images.WSQ";

            try
            {
                foreach (string component in Components.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    NLicense.ObtainComponents("/local", "5000", component);
                }

                //Helpers.ObtainLicenses(licensesMain);

                //try
                //{
                //    Helpers.ObtainLicenses(licensesBss);
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine(ex.ToString());
                //}

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            catch (Exception ex)
            {

                Utils.ShowException(ex);

                //MessageBox.Show("Error. Details: " + ex.Message, "Fingers Sample", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                NLicense.ReleaseComponents(Components);
                //Helpers.ReleaseLicenses(licensesMain);
                //Helpers.ReleaseLicenses(licensesBss);
            }
        }
    }
}
