using Neurotec.Licensing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BiometricsTest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            const string Components = "Biometrics.FingerExtractionFast,Biometrics.FingerMatchingFast,Images.WSQ";
            try
            {
                foreach (string component in Components.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!NLicense.IsComponentActivated(component))
                    {
                        if (!NLicense.ObtainComponents("/local", "5000", component))
                            if (component.Equals("Biometrics.FingerExtractionFast"))
                                NLicense.ObtainComponents("/local", "5000", "Biometrics.FingerExtraction");
                            else if (component.Equals("Biometrics.FingerMatchingFast"))
                                NLicense.ObtainComponents("/local", "5000", "Biometrics.FingerMatching");
                    }
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
    }
}
