using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PSCBioVerification
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            IList<string> licensesMain = new List<string>(new string[] { "FingersExtractor", "FingersMatcher" });
            IList<string> licensesBss = new List<string>(new string[] { "FingersBSS" });

            try
            {
                Helpers.ObtainLicenses(licensesMain);

                try
                {
                    Helpers.ObtainLicenses(licensesBss);
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.ToString());
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error. Details: " + ex.Message, "Fingers Sample", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Helpers.ReleaseLicenses(licensesMain);
                Helpers.ReleaseLicenses(licensesBss);
            }
        }
    }
}
