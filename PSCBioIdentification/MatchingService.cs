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

        Record record;

        Stopwatch _stw = new Stopwatch();

        private bool IsMatchingServiceRunning
        {
            get { return backgroundWorkerMatchingService.IsBusy; }
        }

        //void startMatchingServiceProcess(NSubject.FingerCollection probeFingerCollection)
        void startMatchingServiceProcess(byte[] probeTemplate)
        {
            if (backgroundWorkerMatchingService.IsBusy)
                return;

            backgroundWorkerMatchingService.RunWorkerAsync(probeTemplate);
            //backgroundWorkerMatchingService.RunWorkerAsync(probeFingerCollection);
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

            if (ConfigurationManager.AppSettings["cachingProvider"] == "managed")
            {
                //var fingerList = new ArrayList();

                //CheckBox cb;
                //for (int i = 1; i < 11; i++)
                //{
                //    cb = this.Controls.Find("checkBox" + i.ToString(), true)[0] as CheckBox;
                //    if (cb.Checked)
                //        fingerList.Add(cb.Tag);
                //}

                record = new Record();
                record.probeTemplate = e.Argument as byte[];
                //record.probeTemplate = new byte[4][];
                //byte[] probeTemplate = (e.Argument as NFRecord).Save().ToArray();
                //int k = 0;
                //foreach (var template in e.Argument as NSubject.FingerCollection)
                //{
                //    record.probeTemplate[k++] = template.Save().ToArray();
                //}

                var matchingServiceClient = new PSCBioIdentification.CacheMatchingService.MatchingServiceClient();
                e.Result = matchingServiceClient.match(fingerList, gender, record.probeTemplate);
            }
            else
            {
                record = new Record();
                //record.size = (UInt32)template.GetSize();
                //record.template = template.Save();
                record.probeTemplateSize = (UInt32)(e.Argument as NFRecord).GetSize();
                record.probeTemplate = e.Argument as byte[];
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
                            CallbackFromCacheFillingService callback = new CallbackFromCacheFillingService();
                            callback.MyEvent += MyEvent;
                            InstanceContext context = new InstanceContext(callback);

                            var matchingServiceClient = new PSCBioIdentification.UnmanagedMatchingService.MatchingServiceClient(context);
                            e.Result = matchingServiceClient.match(record.fingerList, record.fingerListSize, record.probeTemplate, record.probeTemplateSize, record.appSettings, ref record.errorMessage, record.errorMessage.Capacity);
                            //e.Result = matchingServiceClient.match(record.fingerList, record.fingerListSize, record.probeTemplate[0], record.probeTemplateSize, record.appSettings, ref record.errorMessage, record.errorMessage.Capacity);
                        }
                    }
                }
            }
        }

        private void backgroundWorkerMatchingService_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Application.DoEvents();

            //MessageBox.Show(toolStripProgressBar.Value.ToString());
            _stw.Stop();

            if (e.Error != null)
            {
                LogLine("Matching service: " + e.Error.Message, true);
                ShowErrorMessage(e.Error.Message);
                stopProgressBar();
                EnableControls(true);
            }
            else
            {
                UInt32 score = (UInt32)e.Result;

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
                        ShowErrorMessage("ERROR!!!");
                        System.Windows.Forms.MessageBox.Show(record.errorMessage.ToString());
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
}
