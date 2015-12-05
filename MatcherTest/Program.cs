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
            string[] arrOfFingers, int arrOffingersSize,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeParamIndex = 1)]
            byte[] template,
            UInt32 size, System.Text.StringBuilder errorMessage, int messageSize);

        [DllImport("Lookup.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void terminateMatchingService();

        class Record
        {
            public UInt32 size;
            public byte[] template;
            public string[] arrOfFingers;
            public int arrOfFingersSize;
            public System.Text.StringBuilder errorMessage;
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
            Console.WriteLine(" ----- Press key to start  -------");
            Console.ReadKey(); 

            NSubject subject = null;
            Record record = new Record();
            record.errorMessage = new System.Text.StringBuilder(512);
            subject = NSubject.FromFile(@"C:\roman\psc\wsq\TwoFingersTemplate.temp");
            record.size = (UInt32)subject.Fingers[0].Objects[0].Template.GetSize();
            record.template = subject.Fingers[0].Objects[0].Template.Save().ToArray();

            record.arrOfFingers = new string[2] { "ri", "rm" };
            record.arrOfFingersSize = record.arrOfFingers.Length;

            for (int i = 1; i < 2; i++)
            {
                switch (i) {
                    case 0:
                        record.arrOfFingers = new string[2] { "ri", "rm" };
                        break;
                    case 1:
                        record.arrOfFingers = new string[2] { "li", "lm" };
                        break;
                    case 2:
                        record.arrOfFingers = new string[2] { "ri", "rm" };
                        break;
                    case 3:
                        record.arrOfFingers = new string[2] { "rl", "rr" };
                        break;
                }

                Stopwatch stw = new Stopwatch();
                stw.Start();

                UInt32 appId = 0;
                unsafe
                {
                    fixed (UInt32* ptr = &record.size)
                    {
                        appId = match(record.arrOfFingers, record.arrOfFingersSize, record.template, record.size, record.errorMessage, record.errorMessage.Capacity);
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
