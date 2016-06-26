using System;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.ComponentModel;
using System.Xml;
using System.IO;
using Neurotec.Biometrics;
using System.Windows.Forms;
using System.ServiceModel;
using System.Threading;
using System.Collections;
using System.Runtime.InteropServices;
using System.ServiceModel.Configuration;
//using System.Windows.Forms;

//using DataSourceServices;

namespace PSCBioIdentification
{
    partial class Form1
    {
        [DllImport("Lookup.dll", EntryPoint = "fillCache", CallingConvention = CallingConvention.StdCall)]
        public static extern void fillCache(
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeParamIndex = 1)]
            string[] fingerList, int fingerListSize,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeParamIndex = 1)]
            string[] appSettings, CallBackDelegate callback);

        //[DllImport("Lookup.dll", CharSet = CharSet.Auto)]
        //public static extern void SetCallBack(CallBackDelegate callback);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct CallBackStruct
        {
            public short code;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string text;
        }

        CallbackFromCacheFillingService _callBack;

        public delegate void CallBackDelegate(ref CallBackStruct callBackParam);

        public void OnCallback(ref CallBackStruct callBackParam)
        {
            if (callBackParam.code == 0)
            {
                _callBack.CacheOperationComplete();
            }
            else if (callBackParam.code == 1)
            {
                int result;
                int.TryParse(callBackParam.text, out result);
                _callBack.RespondWithRecordNumbers(result);
            } 
            else if (callBackParam.code == 2)
                _callBack.RespondWithText(callBackParam.text);
            else if (callBackParam.code == 3)
                _callBack.RespondWithError(callBackParam.text);
        }

        private bool IsCachingServiceRunning
        {
            get { return backgroundWorkerCachingService.IsBusy; }
        }

        void startCachingServiceProcess(ManualResetEvent mre)
        {
            if (backgroundWorkerCachingService.IsBusy)
                return;

            startProgressBar();
            EnableControls(false);
            manageCacheButton.Text = "Cancel";

            _mre = mre;

            _callBack = new CallbackFromCacheFillingService { TotalRecords = 0, RunningSum = 0 };
            _callBack.MyEvent += MyEvent;
            InstanceContext context = new InstanceContext(_callBack);

            if (ConfigurationManager.AppSettings["cachingProvider"] == "managed")
            {
                //_mre = mre;

                //CallbackFromAppFabricCacheService callback = new CallbackFromAppFabricCacheService();
                //callback.MyEvent += MyEvent;
                //InstanceContext context = new InstanceContext(callback);
/*
                String baseAddress = ConfigurationManager.AppSettings["endPointServer"];
                //configurationServiceClient.Endpoint.Address

                //var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                //var serviceModel = configFile.SectionGroups["system.serviceModel"];
                //var clientSection = serviceModel.Sections["client"];

                //var serviceModelClient = ConfigurationManager.GetSection("system.serviceModel/client");

                ClientSection serviceModelClient = ConfigurationManager.GetSection("system.serviceModel/client") as ClientSection;
                //foreach (ChannelEndpointElement cs in serviceModelClient.Endpoints)
                //{
                //    var address = cs.Address;
                //}

                String serviceName = "WSDualHttpBinding_IPopulateCacheService";

                Uri endPoint = serviceModelClient.Endpoints.Cast<ChannelEndpointElement>()
                                                           .SingleOrDefault(endpoint => endpoint.Name == serviceName).Address;

                if (baseAddress.Length != 0)
                    baseAddress = endPoint.Scheme + "://" + baseAddress + ":" + endPoint.Port + endPoint.PathAndQuery;
                //baseAddress = endPoint.Scheme + "://" + baseAddress + ":" + endPoint.Port + "/" + endPoint.Host + endPoint.PathAndQuery;
                else
                    baseAddress = endPoint.AbsoluteUri;

                var client = new CachePopulateService.PopulateCacheServiceClient(context, serviceName, baseAddress);
*/

                var client = new CachePopulateService.PopulateCacheServiceClient(context);

                //try
                //{
                //    client.Run(new string[] { });
                //    //client.Run(new string[] { "0" });
                //}
                //catch (FaultException ex)
                //{
                //    MessageBox.Show(ex.Message);
                //    //ShowErrorMessage(ex.Message);
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message);
                //    //ShowErrorMessage(ex.Message);
                //}

                //buttonRequest.Enabled = false;

                backgroundWorkerCachingService.RunWorkerAsync(client);
            }
            else
            {
                //var client = new UnmanagedMatchingService.
                var client = new UnmanagedMatchingService.MatchingServiceClient(context);
                backgroundWorkerCachingService.RunWorkerAsync(client);

                //backgroundWorkerCachingService.RunWorkerAsync();

            }
        }

        private void backgroundWorkerCachingService_DoWork(object sender, DoWorkEventArgs e)
        {
            var fingerList = new System.Collections.ArrayList();

            CheckBox cb; Label lb;
            for (int i = 1; i < 11; i++)
            {
                lb = this.Controls.Find("labCache" + i.ToString(), true)[0] as Label;
                lb.BackColor = Color.Transparent;

                cb = this.Controls.Find("checkBoxCache" + i.ToString(), true)[0] as CheckBox;
                if (cb.Checked)
                    fingerList.Add(cb.Tag);
            }

            if (fingerList.Count == 0)
            {
                e.Result = fingerList;
                return;
            }

            if (ConfigurationManager.AppSettings["cachingProvider"] == "managed")
            {
                var client = e.Argument as CachePopulateService.PopulateCacheServiceClient;

                //var fingerList = new System.Collections.ArrayList();

                //CheckBox cb; Label lb;
                //for (int i = 1; i < 11; i++)
                //{
                //    lb = this.Controls.Find("labCache" + i.ToString(), true)[0] as Label;
                //    lb.BackColor = Color.Transparent;

                //    cb = this.Controls.Find("checkBoxCache" + i.ToString(), true)[0] as CheckBox;
                //    if (cb.Checked)
                //        fingerList.Add(cb.Tag);
                //}

                //if (fingerList.Count == 0)
                //{
                //    e.Result = fingerList;
                //    return;
                //}
                //record.fingerListSize = ar.Count;
                //record.fingerList = new string[ar.Count];
                //record.fingerList = ar.ToArray(typeof(string)) as string[];

                //ar.Clear();
                //int id = Thread.CurrentThread.ManagedThreadId;
                try
                {
                    client.Run(fingerList);
                    _mre.WaitOne();
                } catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                //client.Run(new string[] { });
                //client.Run(new string[] { "0" });

                //_mre.WaitOne();

                //if (!backgroundWorkerCachingService.CancellationPending)
                //    e.Result = list;
                //else
                //    e.Result = null;
            }
            else
            {
                record = new Record();
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


                var list = new System.Collections.ArrayList();

                //record.appSettings = new System.Text.StringBuilder(4);
                //ar.Add(MyConfigurationSettings.AppSettings["serverName"]);
                //ar.Add(MyConfigurationSettings.AppSettings["dbName"]);

                list.Add(MyConfigurationSettings.ConnectionStrings["ODBCConnectionString"].ToString());
                list.Add(MyConfigurationSettings.AppSettings["dbFingerTable"]);
                list.Add(MyConfigurationSettings.AppSettings["dbIdColumn"]);
                list.Add(MyConfigurationSettings.AppSettings["dbFingerColumn"]);
                record.appSettings = list.ToArray(typeof(string)) as string[];

                //CallBackDelegate deleg = new CallBackDelegate(OnCallback);
                //record.callback = new CallBackDelegate(OnCallback);

                //UInt32 score = 0;
                unsafe
                {
                    fixed (UInt32* ptr = &record.probeTemplateSize)
                    {
                        if (ConfigurationManager.AppSettings["cachingService"] == "local")
                        {
                            //CallBackDelegate d = new CallBackDelegate(OnCallback);
                            //SetCallBack(new CallBackDelegate(OnCallback));

                            fillCache(record.fingerList, record.fingerListSize, record.appSettings, new CallBackDelegate(OnCallback));
                        }
                        else
                        {
                            //CallbackFromAppFabricCacheService callback = new CallbackFromAppFabricCacheService();
                            //callback.MyEvent += MyEvent;
                            //InstanceContext context = new InstanceContext(callback);
                            var client = e.Argument as UnmanagedMatchingService.MatchingServiceClient;

                            //client.setCallBack(deleg);

                            //var matchingServiceClient = new PSCBioIdentification.MatchingService.MatchingServiceClient(context);
                            client.fillCache(record.fingerList, record.fingerListSize, record.appSettings);
                            _mre.WaitOne();
                        }
                    }
                }
            }

            if (!backgroundWorkerCachingService.CancellationPending)
                e.Result = fingerList;
            else
                e.Result = null;
        }

        private void backgroundWorkerCachingService_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                LogLine("Caching service: " + e.Error.Message, true);
                ShowErrorMessage(e.Error.Message);
            }
            else
            {
                ArrayList fingerList = (ArrayList)e.Result;
                if (fingerList == null)
                    fingerList = new ArrayList();

                Label lb;
                for (int i = 1; i < 11; i++)
                {
                    lb = this.Controls.Find("labCache" + i.ToString(), true)[0] as Label;
                    if (fingerList.IndexOf(lb.Text) != -1)
                        lb.BackColor = Color.Cyan;
                    else
                        lb.BackColor = Color.Transparent;
                }

                CheckBox cb;
                for (int i = 1; i < 11; i++)
                {
                    cb = this.Controls.Find("checkBox" + i.ToString(), true)[0] as CheckBox;
                    if (fingerList.IndexOf(cb.Tag) != -1)
                        cb.Enabled = true;
                    else
                        cb.Enabled = false;
                }

                labelCacheValidationTime.Text = string.Format("Valid until: {0:MMM dd} {0:t}", DateTime.Now + new TimeSpan(24, 0, 0));
            }

            stopProgressBar();
            EnableControls(true);
            manageCacheButton.Text = "Refresh Cache";
        }
    }
}
