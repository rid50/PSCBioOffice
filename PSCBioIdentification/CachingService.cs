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

            var client = new AppFabricCacheService.PopulateCacheServiceClient(context);

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
            var client = e.Argument as AppFabricCacheService.PopulateCacheServiceClient;
            client.Run(new string[] { });
            //client.Run(new string[] { "0" });
            _mre.WaitOne();
        }

        private void backgroundWorkerCachingService_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                LogLine("Caching service: " + e.Error.Message, true);
                ShowErrorMessage(e.Error.Message);
            }

            stopProgressBar();
            EnableControls(true);
        }
    }
}
