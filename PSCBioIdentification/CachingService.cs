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

            _mre = mre;
            startProgressBar();
            EnableControls(false);

            CallbackFromAppFabricCacheService callback = new CallbackFromAppFabricCacheService();
            callback.MyEvent += MyEvent;
            InstanceContext context = new InstanceContext(callback);

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

        private void backgroundWorkerCachingService_DoWork(object sender, DoWorkEventArgs e)
        {
            // This method will run on a thread other than the UI thread.
            // Be sure not to manipulate any Windows Forms controls created
            // on the UI thread from this method.
            var client = e.Argument as CachePopulateService.PopulateCacheServiceClient;

            var cbArray = new System.Collections.ArrayList();

            CheckBox cb; Label lb;
            for (int i = 1; i < 11; i++)
            {
                lb = this.Controls.Find("labCache" + i.ToString(), true)[0] as Label;
                lb.BackColor = Color.Transparent;

                cb = this.Controls.Find("checkBoxCache" + i.ToString(), true)[0] as CheckBox;
                if (cb.Checked)
                    cbArray.Add(cb.Tag);
            }

            if (cbArray.Count == 0)
            {
                e.Result = cbArray;
                return;
            }
            //record.arrOfFingersSize = ar.Count;
            //record.arrOfFingers = new string[ar.Count];
            //record.arrOfFingers = ar.ToArray(typeof(string)) as string[];

            //ar.Clear();

            client.Run(cbArray);
            //client.Run(new string[] { });
            //client.Run(new string[] { "0" });
            _mre.WaitOne();
            e.Result = cbArray;
        }

        private void backgroundWorkerCachingService_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                LogLine("Caching service: " + e.Error.Message, true);
                ShowErrorMessage(e.Error.Message);
            }

            ArrayList cbArray = (ArrayList)e.Result;

            if (cbArray.Count == 0)
            {
                ShowErrorMessage("At least one finger should be selected");
            }
            else
            {

                Label lb;
                for (int i = 1; i < 11; i++)
                {
                    lb = this.Controls.Find("labCache" + i.ToString(), true)[0] as Label;
                    if (cbArray.IndexOf(lb.Text) != -1)
                        lb.BackColor = Color.Cyan;
                    else
                        lb.BackColor = Color.Transparent;
                }
            }
            stopProgressBar();
            EnableControls(true);
        }
    }
}
