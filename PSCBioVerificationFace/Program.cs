using System;
using System.Windows.Forms;
using Neurotec.Licensing;
using System.Collections.Generic;

namespace PSCBioVerificationFace
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            IList<string> licenses = new List<string>(new string[] { "Biometrics.Standards.Faces", "Biometrics.FaceExtraction", "Biometrics.FaceMatching" });

            ObtainLicenses(licenses);
            //string Components = "Biometrics.Standards.Faces";
            //string components = "Biometrics.FaceExtraction";
/*
            try
            {
                if (!NLicense.ObtainComponents("/local", 5000, Components))
                {
                    Console.WriteLine(@"Could not obtain licenses for components: {0}", Components);
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
*/
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FaceForm());

            ReleaseLicenses(licenses);

/*
            try
            {
                NLicense.ReleaseComponents(Components);
            }
            catch { }
*/
        }


        public static void ObtainLicenses(string license)
        {
            ObtainLicenses(new string[] { license });
        }

        public static void ObtainLicenses(IList<string> licenses)
        {
            foreach (string license in licenses)
            {
                try
                {
                    bool available = NLicense.ObtainComponents("/local", "5000", license);

                    if (!available)
                    {
                        throw new ApplicationException(string.Format("license for {0} was not obtained", license));
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(string.Format("Error while obtaining license. Please check if Neurotec Activation Service is running. Details: {0}", ex));
                }
            }
        }

        public static void ReleaseLicenses(IList<string> licenses)
        {
            foreach (string license in licenses)
            {
                NLicense.ReleaseComponents(license);
            }
        }

    }
}
