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
using System.ServiceModel;
using System.Threading;
using System.Collections;
//using System.Windows.Forms;

//using DataSourceServices;

namespace PSCBioIdentification
{
    partial class Form1
    {
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

            CallbackFromCacheFillingService callback = new CallbackFromCacheFillingService();
            callback.MyEvent += MyEvent;
            InstanceContext context = new InstanceContext(callback);

            if (ConfigurationManager.AppSettings["matchingProvider"] == "managed")
            {
                //_mre = mre;

                //CallbackFromAppFabricCacheService callback = new CallbackFromAppFabricCacheService();
                //callback.MyEvent += MyEvent;
                //InstanceContext context = new InstanceContext(callback);

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
                var client = new MatchingService.MatchingServiceClient(context);
                backgroundWorkerCachingService.RunWorkerAsync(client);

                //backgroundWorkerCachingService.RunWorkerAsync();

            }
        }

        private void backgroundWorkerCachingService_DoWork(object sender, DoWorkEventArgs e)
        {
            var list = new System.Collections.ArrayList();

            CheckBox cb; Label lb;
            for (int i = 1; i < 11; i++)
            {
                lb = this.Controls.Find("labCache" + i.ToString(), true)[0] as Label;
                lb.BackColor = Color.Transparent;

                cb = this.Controls.Find("checkBoxCache" + i.ToString(), true)[0] as CheckBox;
                if (cb.Checked)
                    list.Add(cb.Tag);
            }

            if (list.Count == 0)
            {
                e.Result = list;
                return;
            }

            if (ConfigurationManager.AppSettings["matchingProvider"] == "managed")
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
                //record.arrOfFingersSize = ar.Count;
                //record.arrOfFingers = new string[ar.Count];
                //record.arrOfFingers = ar.ToArray(typeof(string)) as string[];

                //ar.Clear();

                client.Run(list);
                //client.Run(new string[] { });
                //client.Run(new string[] { "0" });
                _mre.WaitOne();

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

                ////record.arrOfFingers = new string[3] { "ri", "rm", "rr" };
                ////record.arrOfFingersSize = 3;
                //CheckBox cb;
                //for (int i = 1; i < 11; i++)
                //{
                //    cb = this.Controls.Find("checkBox" + i.ToString(), true)[0] as CheckBox;
                //    if (cb.Checked)
                //        ar.Add(cb.Tag);
                //}
                record.arrOfFingersSize = list.Count;
                //record.arrOfFingers = new string[ar.Count];
                record.arrOfFingers = list.ToArray(typeof(string)) as string[];

                list.Clear();

                //record.appSettings = new System.Text.StringBuilder(4);
                //ar.Add(MyConfigurationSettings.AppSettings["serverName"]);
                //ar.Add(MyConfigurationSettings.AppSettings["dbName"]);

                list.Add(MyConfigurationSettings.ConnectionStrings["ODBCConnectionString"].ToString());
                list.Add(MyConfigurationSettings.AppSettings["dbFingerTable"]);
                list.Add(MyConfigurationSettings.AppSettings["dbIdColumn"]);
                list.Add(MyConfigurationSettings.AppSettings["dbFingerColumn"]);
                record.appSettings = list.ToArray(typeof(string)) as string[];

                //UInt32 score = 0;
                unsafe
                {
                    fixed (UInt32* ptr = &record.size)
                    {
                        if (ConfigurationManager.AppSettings["matchingService"] == "local")
                        {
                            fillCache(record.arrOfFingers, record.arrOfFingersSize, record.appSettings);
                        }
                        else
                        {
                            //CallbackFromAppFabricCacheService callback = new CallbackFromAppFabricCacheService();
                            //callback.MyEvent += MyEvent;
                            //InstanceContext context = new InstanceContext(callback);
                            var client = e.Argument as MatchingService.MatchingServiceClient;

                            //var matchingServiceClient = new PSCBioIdentification.MatchingService.MatchingServiceClient(context);
                            client.fillCache(record.arrOfFingers, record.arrOfFingersSize, record.appSettings);
                            _mre.WaitOne();
                        }
                    }
                }
            }

            if (!backgroundWorkerCachingService.CancellationPending)
                e.Result = list;
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

            stopProgressBar();
            EnableControls(true);
            manageCacheButton.Text = "Refresh Cache";
        }
    }
}
