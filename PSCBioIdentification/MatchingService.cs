using System;
using System.Configuration;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.ComponentModel;
using System.Xml;
using System.IO;
using Neurotec.Biometrics;
using System.Windows.Forms;
using System.Collections;
using System.ServiceModel;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.ServiceModel.Description;
using System.Linq;
using System.Net;
//using System.Windows.Forms;

//using DataSourceServices;

namespace PSCBioIdentification
{
    partial class Form1
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

        class Record
        {
            public UInt32   probeTemplateSize;
            public byte[]   probeTemplate;
            public string[] fingerList;
            public int      fingerListSize;
            public string[] appSettings;
            public System.Text.StringBuilder errorMessage;
            //public CallBackDelegate callback;
            //public String errorMessage;
            //public String[] errorMessage = new String[1];

        }


        //TaskLoop tl = new TaskLoop();

        string guid = string.Empty;

        byte[] probeTemplate;

        Record record;

        Stopwatch _stw = new Stopwatch();

        private bool IsMatchingServiceRunning
        {
            get { return backgroundWorkerMatchingService.IsBusy; }
        }

        //void startMatchingServiceProcess(NSubject.FingerCollection probeFingerCollection)
        void startMatchingServiceProcess(byte[] probeTemplate)
        {
            //int processorCount = Environment.ProcessorCount;

            //int workerThreads; int completionPortThreads;
            //ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);
            //ThreadPool.GetMinThreads(out workerThreads, out completionPortThreads);
            //ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
            //ThreadPool.SetMinThreads(Environment.ProcessorCount, 10);


            if (backgroundWorkerMatchingService.IsBusy)
                return;

            this.probeTemplate = probeTemplate;

            //_tokenSource = new CancellationTokenSource();
            //_ct = _tokenSource.Token;

            _mre = new ManualResetEvent(false);

            //CookieContainer cookieContainer = new CookieContainer();

            if (ConfigurationManager.AppSettings["cachingProvider"] == "MemoryCache")
            {
                _serviceClient = new MemoryCacheMatchingService.MatchingServiceClient();

                //_serviceClient.CookieContainer = cookieContainer;
            }
            else if (ConfigurationManager.AppSettings["cachingProvider"] == "AppFabricCache")
                _serviceClient = new AppFabricCacheMatchingService.MatchingServiceClient();

            string errorMessage;
            if (!IsServiceAvailable(_serviceClient.Endpoint.Address.Uri.AbsoluteUri, out errorMessage))
            {
                ShowErrorMessage(errorMessage + " : " + _serviceClient.Endpoint.Address.Uri.AbsoluteUri);
                _serviceClient.Close();
                return;
            }


            guid = Guid.NewGuid().ToString();

            backgroundWorkerMatchingService.RunWorkerAsync(_serviceClient);
        }

        private void backgroundWorkerMatchingService_DoWork(object sender, DoWorkEventArgs e)
        {
            var fingerList = new ArrayList();
            CheckBox cb;
            for (int i = 4; i > 0; i--)
            {
                cb = this.Controls.Find("checkBox" + i.ToString(), true)[0] as CheckBox;
                if (cb.Checked)
                    fingerList.Add(cb.Tag as string);
            }

            for (int i = 5; i < 11; i++)
            {
                cb = this.Controls.Find("checkBox" + i.ToString(), true)[0] as CheckBox;
                if (cb.Checked)
                    fingerList.Add(cb.Tag as string);
            }

            int gender = 1;
            if (radioButtonMan.Checked)
                gender = 1;
            else if (radioButtonWoman.Checked)
                gender = 2;
            else if (radioButtonManAndWoman.Checked)
                gender = 0;

            _stw.Restart();

            record = new Record();
            record.probeTemplate = this.probeTemplate;
            //record.probeTemplate = e.Argument as byte[];

            //dynamic client = null;

            //if (ConfigurationManager.AppSettings["cachingProvider"] == "MemoryCache")
            //    _serviceClient = new MemoryCacheMatchingService.MatchingServiceClient(_instanceContext);
            //else if (ConfigurationManager.AppSettings["cachingProvider"] == "AppFabricCache")
            //    _serviceClient = new AppFabricCacheMatchingService.MatchingServiceClient();

            //if (!ReferenceEquals(null, client))
            e.Result = null;

            dynamic client = null;

            if (ConfigurationManager.AppSettings["cachingProvider"] == "MemoryCache")
            {
                client = e.Argument as MemoryCacheMatchingService.MatchingServiceClient;
                e.Result = null; _matchingResult = 0;
                //try
                //{
                    int i = Thread.CurrentThread.ManagedThreadId;
                    e.Result = client.match(guid, fingerList, gender, record.probeTemplate);

                    //tl.Loop();

                    //RunMatching(client, fingerList, gender, record, e);
                    //_mre.WaitOne();
                //}
                //catch (FaultException ex)
                //{


                //}
            }
            else if (ConfigurationManager.AppSettings["cachingProvider"] == "AppFabricCache")
            {
                client = e.Argument as AppFabricCacheMatchingService.MatchingServiceClient;
                e.Result = client.match(guid, fingerList, gender, record.probeTemplate);
            }
            //    if (client != null)
            //{
            //    try
            //    {
            //        client.Run(fingerList);
            //        _mre.WaitOne();
            //    }
            //    catch (Exception ex)
            //    {
            //        throw new Exception(ex.Message);
            //    }
            //}

            //if (_serviceClient != null)
            //{
            //    try
            //    {
            //        if (_serviceClient is AppFabricCacheMatchingService.MatchingServiceClient)
            //        {
            //            e.Result = _serviceClient.match(fingerList, gender, record.probeTemplate);
            //        } else if (_serviceClient is MemoryCacheMatchingService.MatchingServiceClient)
            //        {
            //            e.Result = null; _matchingResult = 0;
            //            _serviceClient.match(fingerList, gender, record.probeTemplate);
            //            _mre.WaitOne();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        throw new Exception(ex.Message);
            //    } finally
            //    {
            //        //_serviceClient = null;
            //    }
            //}
            else    // ConfigurationManager.AppSettings["cachingProvider"] == "ODBCCache"
            {
                //record = new Record();
                //record.size = (UInt32)template.GetSize();
                //record.template = template.Save();
                //record.probeTemplateSize = (UInt32)(e.Argument as NFRecord).GetSize();
                record.probeTemplateSize = (UInt32)(e.Argument as byte[]).Length;
                //record.probeTemplate = e.Argument as byte[];
                //record.probeTemplate[0] = (e.Argument as NFRecord).Save().ToArray();
                record.errorMessage = new System.Text.StringBuilder(512);

                //var ar = new ArrayList();

                ////record.fingerList = new string[3] { "ri", "rm", "rr" };
                ////record.fingerListSize = 3;
                //CheckBox cb;
                //for (int i = 1; i < 11; i++)
                //{
                //    cb = this.Controls.Find("checkBox" + i.ToString(), true)[0] as CheckBox;
                //    if (cb.Checked)
                //        ar.Add(cb.Tag);
                //}
                record.fingerListSize = fingerList.Count;
                //record.fingerList = new string[ar.Count];
                record.fingerList = fingerList.ToArray(typeof(string)) as string[];

                fingerList.Clear();

                //record.appSettings = new System.Text.StringBuilder(4);
                //ar.Add(MyConfigurationSettings.AppSettings["serverName"]);
                //ar.Add(MyConfigurationSettings.AppSettings["dbName"]);

                fingerList.Add(MyConfigurationSettings.ConnectionStrings["ODBCConnectionString"].ToString());
                fingerList.Add(MyConfigurationSettings.AppSettings["dbFingerTable"]);
                fingerList.Add(MyConfigurationSettings.AppSettings["dbIdColumn"]);
                fingerList.Add(MyConfigurationSettings.AppSettings["dbFingerColumn"]);
                record.appSettings = fingerList.ToArray(typeof(string)) as string[];

                //UInt32 score = 0;
                unsafe
                {
                    fixed (UInt32* ptr = &record.probeTemplateSize)
                    {
                        if (ConfigurationManager.AppSettings["cachingService"] == "local")
                        {
                            e.Result = match(record.fingerList, record.fingerListSize, record.probeTemplate, record.probeTemplateSize, record.appSettings, record.errorMessage, record.errorMessage.Capacity);
                            //e.Result = match(record.fingerList, record.fingerListSize, record.probeTemplate[0], record.probeTemplateSize, record.appSettings, record.errorMessage, record.errorMessage.Capacity);
                        }
                        else
                        {
                            CallbackFromDualHttpBindingService callback = new CallbackFromDualHttpBindingService();
                            callback.MyEvent += MyEvent;
                            InstanceContext context = new InstanceContext(callback);

                            //var matchingServiceClient = new PSCBioIdentification.UnmanagedMatchingService.MatchingServiceClient(context);
                            //e.Result = matchingServiceClient.match(record.fingerList, record.fingerListSize, record.probeTemplate, record.probeTemplateSize, record.appSettings, ref record.errorMessage, record.errorMessage.Capacity);
                            _serviceClient = new PSCBioIdentification.UnmanagedMatchingService.MatchingServiceClient(context);
                            e.Result = _serviceClient.match(record.fingerList, record.fingerListSize, record.probeTemplate, record.probeTemplateSize, record.appSettings, ref record.errorMessage, record.errorMessage.Capacity);
                        }
                    }
                }
            }

            if (backgroundWorkerMatchingService.CancellationPending)
            {
                //try
                //{
                //    TerminateMatching(client);
                //}
                //catch (Exception ex)
                //{
                //    throw new Exception(ex.Message);
                //}

                //await proxy.GetMessagesAsync();

                //var task = System.Threading.Tasks.Task<int>.Factory.FromAsync(client.BeginTerminate, client.EndTerminate, 0, null);
                //int i = task.Result;

                //client.Terminate(0);
                //try
                //{

                //    Action myAction = () =>
                //    {
                //        _serviceClient.Terminate();
                //    };


                //    IAsyncResult result = myAction.BeginInvoke(null, null);

                //    myAction.EndInvoke(result);

                //    //if (_tokenSource != null)
                //    //    _tokenSource.Cancel();
                //}
                //catch (Exception) { }
            }
        }

        private async Task RunMatching(dynamic client, ArrayList fingerList, int gender, Record record, DoWorkEventArgs e)
        {
            Task<uint> t = client.matchAsync(fingerList, gender, record.probeTemplate);
            e.Result = await t;
            //int i = 0;
            //await Task.Delay(5000);
            _mre.Set();
        }

        private void TerminateMatching(dynamic client)
        //private async Task TerminateMatching(dynamic client)
        {
            //int i = 0;
            //Task<int> t = Task.Run(() =>
            //{
            //    //try
            //    //{
            //    i = Thread.CurrentThread.ManagedThreadId;
            //    i = client.Terminate(guid);
            //    return i;
            //    //tl.Terminate();
            //    //i = 2;

            //    //}
            //    //catch (Exception) { }
            //});

            //i = await t;
            //i = 2;

            //Task<int> t = client.TerminateAsync();
            //int result = await t;
            //int i = 0;

            //var cli = new MemoryCacheMatchingService.MatchingServiceClient();
            //int k = cl.Terminate();
            //cl.Close();

            ////// Create a channel factory.
            ////BasicHttpBinding binding = new BasicHttpBinding(client.Endpoint.Name);
            //WSHttpBinding binding = new WSHttpBinding(client.Endpoint.Name);
            ////NetTcpBinding binding = new NetTcpBinding(client.Endpoint.Name);

            ////binding.ReliableSession.Enabled = true;
            ////binding.Security.Mode = SecurityMode.None;
            //cli.Endpoint.Address.Uri.AbsoluteUri
            //EndpointAddress endpoint = new EndpointAddress("http://localhost/MemoryCacheService/MatchingService.svc");
            //ChannelFactory<MemoryCacheMatchingService.IMatchingService> factory = new ChannelFactory<MemoryCacheMatchingService.IMatchingService>(binding, endpoint);

            //////WSDualHttpBinding binding = new WSDualHttpBinding();
            //////ContractDescription contract = new ContractDescription("WSDualHttpBinding_IMatchingService");
            //////EndpointAddress myEndpoint = new EndpointAddress("http://localhost/MemoryCacheService/MatchingService.svc");
            //////ServiceEndpoint serviceEndpoint = new ServiceEndpoint(contract, binding, myEndpoint);
            //////DuplexChannelFactory<MemoryCacheMatchingService.IMatchingService> myChannelFactory =
            //////    new DuplexChannelFactory<MemoryCacheMatchingService.IMatchingService>(_instanceContext, serviceEndpoint);

            //DuplexChannelFactory<MemoryCacheMatchingService.IMatchingService> factory =
            //    new DuplexChannelFactory<MemoryCacheMatchingService.IMatchingService>(_instanceContext, client.Endpoint.Name);

            // Create a channel.
            ChannelFactory<MemoryCacheMatchingService.IMatchingService> factory = new ChannelFactory<MemoryCacheMatchingService.IMatchingService>(client.Endpoint.Binding, client.Endpoint.Address);
            MemoryCacheMatchingService.IMatchingService cl = factory.CreateChannel();
            int k = cl.Terminate(guid);
            LogLine(k.ToString(), true);
            ((IClientChannel)cl).Close();
        }

        private void backgroundWorkerMatchingService_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //if (_tokenSource != null)
            //    _tokenSource.Dispose();
            try
            {
                if (_serviceClient != null)
                {
                    if (_serviceClient.State == CommunicationState.Opened)
                        _serviceClient.Close();

                    _serviceClient = null;
                }
            } catch (Exception) { }
            //Application.DoEvents();

            //MessageBox.Show(toolStripProgressBar.Value.ToString());
            _stw.Stop();

            if (e.Error != null)
            {
                LogLine("Matching service: " + e.Error.Message, true);
                ShowErrorMessage("Matching service: " + e.Error.Message);
                stopProgressBar();
                EnableControls(true);
            }
            else
            {
                UInt32 score;

                if (e.Result == null)
                    score = _matchingResult;
                else
                    score = (UInt32)e.Result;

                //string str = string.Format("Identification {0}", score == 0 ? "failed" : string.Format("succeeded. Score: {0}", score));
                //string str = string.Format("Identification: {0}", score == 0 ? "failed" : "succeess");
                //LogLine(str, true);

                string str = string.Format("{0}, Time elapsed: {1:hh\\:mm\\:ss}", score == 0 ? "Failure" : "Succeess", _stw.Elapsed);
                LogLine(str, true);

                //ShowStatusMessage(str);

                personId.Text = "";
                if (score > 0)
                {
                    this.userId = (int)score;
                    personId.Text = score.ToString();
                    pictureBoxCheckMark.Image = Properties.Resources.checkmark;
                    Mode = ProgramMode.PreEnrolled;
                    startDataServiceProcess();
                }
                else
                {
                    pictureBoxCheckMark.Image = Properties.Resources.redcross;

                    if (record != null && record.errorMessage != null && record.errorMessage.Length != 0)
                    {
                        //retcode = false;
                        //ShowErrorMessage("ERROR!!!");
                        //System.Windows.Forms.MessageBox.Show(record.errorMessage.ToString());
                        ShowErrorMessage(record.errorMessage.ToString());
                    }
                    stopProgressBar();
                    EnableControls(true);
                    
                    //else
                    //{
                    //    this.BeginInvoke(new MethodInvoker(delegate() { startCapturing(); }));
                    //}
                }

                //if (score > 0)
                //{
                //    //startProgressBar();
                //    startDataServiceProcess();
                //}
                //else
                //{
                //    stopProgressBar();

                //    if (record.errorMessage.Length != 0)
                //    {
                //        //retcode = false;
                //        System.Windows.Forms.MessageBox.Show(record.errorMessage.ToString());
                //        //ShowErrorMessage(record.errorMessage.ToString());
                //    }

                //}
            }

            //buttonRequest.Enabled = true;
        }
    }

    //class TaskLoop
    //{
    //    CancellationTokenSource source = null;
    //    CancellationToken ct;

    //    public void Loop()
    //    {
    //        source = new CancellationTokenSource();
    //        ct = source.Token;

    //        //Task.Run(() =>
    //        //{
    //        Console.WriteLine("Main Thread={0}", Thread.CurrentThread.ManagedThreadId);

    //        //int i = 0;
    //        //Task<int>[] tasks = new Task<int>[9];
    //        //var tasks = new List<Task>();
    //        List<Task<int>> tasks = new List<Task<int>>();
    //        //source.Cancel();
    //        //ct.ThrowIfCancellationRequested();
    //        for (int i = 0; i < 2; i++)
    //        {
    //            tasks.Add(Task.Factory.StartNew((i2) =>
    //            {

    //                //source.Cancel();

    //                //var delay = Task.Run(async () => {
    //                //    await Task.Delay(5000);
    //                //    int k = 0;
    //                //});

    //                Task.Delay(5000).Wait();

    //                //if (ct.IsCancellationRequested)
    //                //  throw new Exception("kuku");

    //                ct.ThrowIfCancellationRequested();

    //                //Console.WriteLine("MainTask {0} Thread={1}", i, Thread.CurrentThread.ManagedThreadId);
    //                Console.WriteLine("Thread={0}, i={1}", Thread.CurrentThread.ManagedThreadId, i2);
    //                //String str = new SyncTasks().HelloAsync(i.ToString());

    //                //Console.WriteLine(str);
    //                return 0;

    //            }, i, ct));
    //        }

    //        //Task t = Task.WhenAll(tasks.ToArray().Where(ta => ta != null));
    //        //Task t = Task.WhenAll(tasks.ToArray());

    //        try
    //        {
    //            Task.WhenAll(tasks.ToArray()).Wait();
    //            //int k = 0;
    //            //t.Wait();

    //            //Task.WhenAll(tasks.ToArray().Where(t => t != null));
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new Exception(ex.Message);
    //            Console.WriteLine(ex.Message);

    //            Console.WriteLine("Caught first exception: {0}", ex);
    //            Console.WriteLine("***************************");
    //            //Console.WriteLine("Aggregate exception is: {0}", task.Exception);
                
    //        }

    //        //            });
    //        //int k = 0;
    //    }

    //    public void Terminate()
    //    {
    //        Console.WriteLine("Termination Thread={0}", Thread.CurrentThread.ManagedThreadId);

    //        source.Cancel();
    //        Console.WriteLine("===================");
    //    }
    //}
}
