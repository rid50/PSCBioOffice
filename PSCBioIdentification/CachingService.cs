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

        CallbackFromDualHttpBindingService _callBack = new CallbackFromDualHttpBindingService { TotalRecords = 0, RunningSum = 0 };

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

        void startCachingServiceProcess()
        {
            if (backgroundWorkerCachingService.IsBusy)
                return;

            startProgressBar();
            EnableControls(false);
            manageCacheButton.Text = "Cancel";

            _mre = new ManualResetEvent(false);

            //_callBack = new CallbackFromCacheFillingService { TotalRecords = 0, RunningSum = 0 };
            //_callBack.MyEvent += MyEvent;
            //InstanceContext context = new InstanceContext(_callBack);

            //_callbackFromCacheFillingService = new CallbackFromCacheFillingService { TotalRecords = 0, RunningSum = 0 };
            //_callbackFromCacheFillingService.MyEvent += MyEvent;
            //_instanceContext = new InstanceContext(_callbackFromCacheFillingService);

            _callbackFromDualHttpBindingService.TotalRecords = 0;
            _callbackFromDualHttpBindingService.RunningSum = 0;

            if (ConfigurationManager.AppSettings["cachingProvider"] == "MemoryCache")
                _serviceClient = new MemoryCachePopulateService.PopulateCacheServiceClient(_instanceContext);
            else if (ConfigurationManager.AppSettings["cachingProvider"] == "AppFabricCache")
                _serviceClient = new AppFabricCachePopulateService.PopulateCacheServiceClient(_instanceContext);
            else
                _serviceClient = new UnmanagedMatchingService.MatchingServiceClient(_instanceContext);

            backgroundWorkerCachingService.RunWorkerAsync(_serviceClient);

            //dynamic client;

            //if (ConfigurationManager.AppSettings["cachingProvider"] == "MemoryCache")
            //    client = new MemoryCachePopulateService.PopulateCacheServiceClient(context);
            //else if (ConfigurationManager.AppSettings["cachingProvider"] == "AppFabricCache")
            //    client = new AppFabricCachePopulateService.PopulateCacheServiceClient(context);
            //else
            //    client = new UnmanagedMatchingService.MatchingServiceClient(context);

            //backgroundWorkerCachingService.RunWorkerAsync(client);
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

            dynamic client = null;

            if (ConfigurationManager.AppSettings["cachingProvider"] == "MemoryCache")
                client = e.Argument as MemoryCachePopulateService.PopulateCacheServiceClient;
            else if (ConfigurationManager.AppSettings["cachingProvider"] == "AppFabricCache")
                client = e.Argument as AppFabricCachePopulateService.PopulateCacheServiceClient;

            //if (!ReferenceEquals(null, client))
            if (client != null) {
                try
                {
                    client.Run(fingerList);
                    _mre.WaitOne();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else    // ConfigurationManager.AppSettings["cachingProvider"] == "ODBCCache"
            {
                record = new Record();
                record.errorMessage = new System.Text.StringBuilder(512);

                record.fingerListSize = fingerList.Count;
                record.fingerList = fingerList.ToArray(typeof(string)) as string[];


                var list = new System.Collections.ArrayList();

                list.Add(MyConfigurationSettings.ConnectionStrings["ODBCConnectionString"].ToString());
                list.Add(MyConfigurationSettings.AppSettings["dbFingerTable"]);
                list.Add(MyConfigurationSettings.AppSettings["dbIdColumn"]);
                list.Add(MyConfigurationSettings.AppSettings["dbFingerColumn"]);
                record.appSettings = list.ToArray(typeof(string)) as string[];

                unsafe
                {
                    fixed (UInt32* ptr = &record.probeTemplateSize)
                    {
                        if (ConfigurationManager.AppSettings["cachingService"] == "local")
                        {
                            fillCache(record.fingerList, record.fingerListSize, record.appSettings, new CallBackDelegate(OnCallback));
                        }
                        else
                        {
                            client = e.Argument as UnmanagedMatchingService.MatchingServiceClient;

                            client.fillCache(record.fingerList, record.fingerListSize, record.appSettings);
                            _mre.WaitOne();
                        }
                    }
                }
            }

            if (!backgroundWorkerCachingService.CancellationPending)
                e.Result = fingerList;
            else
            {
                e.Result = null;
                client.Terminate();
            }
        }

        private void backgroundWorkerCachingService_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _serviceClient = null;

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
                else
                {
                    if (ConfigurationManager.AppSettings["cachingProvider"] != "ODBCCache")
                        labelCacheValidationTime.Text = string.Format("Valid until: {0:MMM dd} {0:t}", DateTime.Now + new TimeSpan(24, 0, 0));
                }

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
            }

            stopProgressBar();
            EnableControls(true);
            manageCacheButton.Text = "Refresh Cache";
        }
    }
}
