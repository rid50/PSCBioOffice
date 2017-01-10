using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Neurotec.Biometrics;
using Neurotec.Licensing;

namespace MatcherTest
{

    class Program
    {
        [DllImport("Lookup.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 match(
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeParamIndex = 1)]
            string[] fingerList, int fingerListSize,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeParamIndex = 1)]
            byte[] probeTemplate,
            UInt32 probeTemplateSize,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeParamIndex = 1)]
            string[] appSettings,
            System.Text.StringBuilder errorMessage, int messageSize);

        [DllImport("Lookup.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void terminateMatchingService();

        //class Record
        //{
        //    public UInt32 size;
        //    public byte[] template;
        //    public string[] fingerList;
        //    public int fingerListSize;
        //    public System.Text.StringBuilder errorMessage;
        //}

        class Record
        {
            public UInt32 probeTemplateSize;
            public byte[] probeTemplate;
            public string[] fingerList;
            public int fingerListSize;
            public string[] appSettings;
            public System.Text.StringBuilder errorMessage;
            //public CallBackDelegate callback;
            //public String errorMessage;
            //public String[] errorMessage = new String[1];

        }
        //[STAThread]
        static void Main(string[] args)
        {
            const string Components = "Biometrics.FingerExtraction,Biometrics.FingerMatching,Devices.FingerScanners,Images.WSQ";

            try
            {
                foreach (string component in Components.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    NLicense.ObtainComponents("/local", "5000", component);
                }
                Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                NLicense.ReleaseComponents(Components);
            }
        }

        static void Run()
        {
            //Console.WriteLine(" ----- Press key to start  -------");
            //Console.ReadKey(); 

            NSubject subject = null;
            Record record = new Record();
            record.errorMessage = new System.Text.StringBuilder(512);
            subject = NSubject.FromFile(@"C:\roman\psc\wsq\LeftIndexMiddle.template");
            //record.probeTemplateSize = (UInt32)subject.Fingers[0].Objects[0].Template.GetSize();
            //record.probeTemplate = subject.Fingers[0].Objects[0].Template.Save().ToArray();
            record.probeTemplate = subject.GetTemplateBuffer().ToArray();
            record.probeTemplateSize = (UInt32)record.probeTemplate.Length;

            record.fingerList = new string[2] { "lm", "li" };
            record.fingerListSize = record.fingerList.Length;

//            for (int i = 1; i < 2; i++)
            {
                //switch (i) {
                //    case 0:
                //        record.fingerList = new string[2] { "ri", "rm" };
                //        break;
                //    case 1:
                //        record.fingerList = new string[2] { "li", "lm" };
                //        break;
                //    case 2:
                //        record.fingerList = new string[2] { "ri", "rm" };
                //        break;
                //    case 3:
                //        record.fingerList = new string[2] { "rl", "rr" };
                //        break;
                //}

                var fingerList = new System.Collections.ArrayList();
                fingerList.Add("DRIVER=ODBC Driver 11 for SQL Server;SERVER=(local);DATABASE=MCCS_FP;Trusted_Connection=no;UID=sa;PWD=psc;Mars_Connection=yes;");
                fingerList.Add("Egy_T_FingerPrint");
                fingerList.Add("AppID");
                fingerList.Add("AppWsq");
                record.appSettings = fingerList.ToArray(typeof(string)) as string[];

                Stopwatch stw = new Stopwatch();
                stw.Start();

                UInt32 appId = 0;
                unsafe
                {
                    fixed (UInt32* ptr = &record.probeTemplateSize)
                    {
                        appId = match(record.fingerList, record.fingerListSize, record.probeTemplate, record.probeTemplateSize, record.appSettings, record.errorMessage, record.errorMessage.Capacity);
                    }
                }

                stw.Stop();
                Console.WriteLine(" ----- Score: {0}", appId);
                Console.WriteLine(" ----- Time elapsed: {0}", stw.Elapsed);
            }

            Console.WriteLine(" ----- Press key to end  -------");
            Console.ReadKey();
        }
    }
}
