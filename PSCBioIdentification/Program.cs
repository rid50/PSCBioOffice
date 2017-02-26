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
            const string Components = "Biometrics.FingerExtraction,Biometrics.FingerSegmentation,Images.WSQ,Devices.FingerScanners";

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
                        if (!NLicense.IsComponentActivated(component))
                        {
                            NLicense.ObtainComponents("/local", "5000", component);
                        }
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
            }
            finally
            {
                NLicense.ReleaseComponents(Components);
            }
        }
    }
}
