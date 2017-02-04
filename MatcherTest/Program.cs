using System;
using System.Diagnostics;
using System.IO;
using Neurotec.Biometrics;
using Neurotec.Biometrics.Client;
using Neurotec.Licensing;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neurotec.Tutorials
{
	class Program
	{
		static int Main(string[] args)
		{
			const string Components = "Biometrics.FingerMatchingFast";
            NLicense.ObtainComponents("/local", 5000, "Biometrics.FingerMatching");

            //TutorialUtils.PrintTutorialHeader(args);
            //List<Tuple<string, int>> list = new List<Tuple<string, int>>();
            List<Task<int>> tasks = new List<Task<int>>();

            //try
            //{
                for (int k = 1; k < 11; k++)
                {
                    //tasks.Add(Task.Factory.StartNew((Object obj) =>
                    tasks.Add(Task.Factory.StartNew((Object o) =>
                    {
                        using (var biometricClient = new NBiometricClient())
                        using (NSubject probeSubject = CreateSubject("C:\\roman\\fingerprints\\LeftIndexMiddle.dat", "ProbeSubject"))
                        {
                            NBiometricTask enrollTask = biometricClient.CreateTask(NBiometricOperations.Enroll, null);

                            DirectoryInfo di = new DirectoryInfo(string.Format(@"C:\roman\fingerprints\records{0}", (int)o));
                            Console.WriteLine(di.FullName);

                            var fi = di.GetFiles("*.bin", SearchOption.TopDirectoryOnly);
                            //var fi = di.GetFiles("*.bin", SearchOption.TopDirectoryOnly).Take(1000);

                            int i = 0;
                            foreach (FileInfo info in fi)
                            {
                                //NSubject s = CreateSubject(info.FullName, string.Format("GallerySubject_{0}", i++));

                                biometricClient.Enroll(CreateSubject(info.FullName, string.Format("GallerySubject_{0}", i++)));
                                //if ((i % 1000) == 0)
                                //    Console.WriteLine(i.ToString());
                            }

                            //enrollTask.Subjects.Add(probeSubject);

                            biometricClient.PerformTask(enrollTask);
                            NBiometricStatus status = enrollTask.Status;
                            Console.WriteLine(status);
                            if (status != NBiometricStatus.Ok)
                            {
                                Console.WriteLine("Enrollment was unsuccessful. Status: {0}.", status);
                                if (enrollTask.Error != null) throw enrollTask.Error;
                                return -1;
                            }

                            // Set matching threshold
                            biometricClient.MatchingThreshold = 48;

                            // Set matching speed
                            biometricClient.FingersMatchingSpeed = NMatchingSpeed.High;

                            //NLicense.ObtainComponents("/local", 5000, "Biometrics.FingerMatching");

                            Stopwatch stw = new Stopwatch();
                            stw.Start();

                            //throw new Exception("kuku");
                            status = biometricClient.Identify(probeSubject);

                            stw.Stop();

                            Console.WriteLine("Finger matcher, elapsed ms: {0}", stw.ElapsedMilliseconds);
                            //NLicense.ReleaseComponents("Biometrics.FingerMatching");

                            //NLicense.ObtainComponents("/local", 5000, "Biometrics.FingerMatchingFast");

                            //stw = new Stopwatch();
                            //stw.Start();

                            //status = biometricClient.Identify(probeSubject);

                            //stw.Stop();

                            //Console.WriteLine("Finger Fast matcher, elapsed ms: {0}", stw.ElapsedMilliseconds);
                            //NLicense.ReleaseComponents("Biometrics.FingerMatchingFast");
                        }
                        return 0;
                    //}
                    }, k));
                }
            //}
            //catch (Exception ex)
            //{
            //    TutorialUtils.PrintException(ex);
            //}
            //finally
            //{
            //    NLicense.ReleaseComponents(Components);
            //}

            Task task = Task.WhenAll(tasks.ToArray());
            //Task task = Task.WhenAll(tasks.ToArray().Where(t => t != null));
            try
            {
                task.Wait();

                foreach (var t in tasks)
                {
                    if (t.Status == TaskStatus.RanToCompletion && (int)(t.Result) != 0)
                    {
                        //list = t.Result;
                        break;
                    }
                }

                //return retcode;
            }
            catch (Exception ex)
            {

                //while ((ex is System.AggregateException) && (ex.InnerException != null))
                //    ex = ex.InnerException;

                ////while ((ex.InnerException != null))
                ////    ex = ex.InnerException;

                //Console.WriteLine(ex);
                TutorialUtils.PrintException(ex);

                //foreach (var t in tasks)
                //{
                //    if (t == null)
                //        continue;

                //    if (t.Status == TaskStatus.RanToCompletion && t.Result != 0)
                //    {
                //        //list = (List<Tuple<string, int>>)(t.Result);
                //        break;
                //    }
                //    else if (t.Status == TaskStatus.Faulted || t.Status == TaskStatus.Running)
                //    {
                //        bool fault = true;
                //        if (ex.Message.Equals("The operation was canceled."))
                //        {
                //            fault = true;
                //            //continue;
                //        }

                //        //while ((ex is AggregateException) && (ex.InnerException != null))
                //        while (ex.InnerException != null)
                //        {
                //            if (ex.Message.EndsWith("Operation cancelled by user."))
                //            {
                //                fault = false;
                //                break;
                //            }
                //            else if (ex.InnerException.GetType().Name.Equals("TaskCanceledException"))
                //            {
                //                if (ex.InnerException.Message.StartsWith("A task was canceled"))
                //                {
                //                    fault = false;
                //                    break;
                //                }
                //            }

                //            //ex = ex.InnerException;

                //            if (ex.Message.Equals("The operation was canceled."))
                //            {
                //                fault = true;
                //                //break;
                //            }
                //        }

                //        if (fault)
                //        {
                //            throw new Exception(ex.Message);
                //        }
                //    }
                //}
            }
            finally
            {
                NLicense.ReleaseComponents(Components);
            }
            return 0;


            //         try
            //         {
            //	// Obtain license


            //	using (var biometricClient = new NBiometricClient())
            //	using (NSubject probeSubject = CreateSubject("C:\\roman\\fingerprints\\LeftIndexMiddle.dat", "ProbeSubject"))
            //	{
            //		NBiometricTask enrollTask = biometricClient.CreateTask(NBiometricOperations.Enroll, null);

            //                 DirectoryInfo di = new DirectoryInfo(@"C:\roman\fingerprints\records1");

            //                 //var fi = di.GetFiles("*.bin", SearchOption.AllDirectories).Take(10000);
            //                 var fi = di.GetFiles("*.bin", SearchOption.TopDirectoryOnly);

            //                 int i = 0;
            //                 foreach (FileInfo info in fi)
            //                 {
            //                     //NSubject s = CreateSubject(info.FullName, string.Format("GallerySubject_{0}", i++));

            //                     biometricClient.Enroll(CreateSubject(info.FullName, string.Format("GallerySubject_{0}", i++)));
            //                     if ((i % 1000) == 0)
            //                         Console.WriteLine(i.ToString());

            //                 }

            //                 enrollTask.Subjects.Add(probeSubject);

            //		biometricClient.PerformTask(enrollTask);
            //                 NBiometricStatus status = enrollTask.Status;
            //                 Console.WriteLine(status);
            //		if (status != NBiometricStatus.Ok)
            //		{
            //			Console.WriteLine("Enrollment was unsuccessful. Status: {0}.", status);
            //			if (enrollTask.Error != null) throw enrollTask.Error;
            //			return -1;
            //		}

            //		// Set matching threshold
            //		biometricClient.MatchingThreshold = 48;

            //		// Set matching speed
            //		biometricClient.FingersMatchingSpeed = NMatchingSpeed.High;

            //		// Identify probe subject


            //                 for (int j = 0; j < 20; j++)
            //                 {
            //                     NLicense.ObtainComponents("/local", 5000, "Biometrics.FingerMatching");
            //                     Stopwatch stw = new Stopwatch();
            //                     stw.Start();

            //                     status = biometricClient.Identify(probeSubject);

            //                     stw.Stop();

            //                     Console.WriteLine("Finger matcher, elapsed ms: {0}", stw.ElapsedMilliseconds);

            //                     NLicense.ReleaseComponents("Biometrics.FingerMatching");

            //                     NLicense.ObtainComponents("/local", 5000, "Biometrics.FingerMatchingFast");

            //                     stw = new Stopwatch();
            //                     stw.Start();

            //                     status = biometricClient.Identify(probeSubject);

            //                     stw.Stop();

            //                     Console.WriteLine("Finger Fast matcher, elapsed ms: {0}", stw.ElapsedMilliseconds);

            //                     NLicense.ReleaseComponents("Biometrics.FingerMatchingFast");
            //                 }

            //	}
            //	return 0;
            //}
            //catch (Exception ex)
            //{
            //	return TutorialUtils.PrintException(ex);
            //}
            //finally
            //{
            //	NLicense.ReleaseComponents(Components);
            //}
            //return 0;
        }

        private static NSubject CreateSubject(string fileName, string subjectId)
		{
			var subject = new NSubject();
            subject.SetTemplateBuffer(new IO.NBuffer(File.ReadAllBytes(fileName)));
			subject.Id = subjectId;
			return subject;
		}
	}
}
