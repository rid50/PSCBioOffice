using System;
using System.Windows.Forms;
using Neurotec.Licensing;
using System.Threading;

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
            const string Components = "Biometrics.FingerExtraction,Biometrics.FingerSegmentation,Images.WSQ,Devices.FingerScanners";
            //const string Components = "Devices.FingerScanners";

            try
            {
                string str = typeof(Program).AssemblyQualifiedName;
                using (Mutex mutex = new Mutex(false, @"Global\" + str))
                {
                    if (!mutex.WaitOne(0, false))
                    {
                        MessageBox.Show("PSCBioIdentification already running");
                        return;
                    }

                    foreach (string component in Components.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        NLicense.ObtainComponents("/local", "5000", component);
                    }

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Form1());
                }
            }
            catch (Exception ex)
            {
                while ((ex is AggregateException) && (ex.InnerException != null))
                    ex = ex.InnerException;

                MessageBox.Show(ex.ToString(), null, MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Utils.ShowException(ex);

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
