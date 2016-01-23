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
            byte[] template,
            UInt32 size,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeParamIndex = 1)]
            string[] appSettings,
            System.Text.StringBuilder errorMessage, int messageSize);

        class Record
        {
            public UInt32   size;
            public byte[]   template;
            public string[] fingerList;
            public int      fingerListSize;
            public string[] appSettings;
            public System.Text.StringBuilder errorMessage;

            //public String errorMessage;
            //public String[] errorMessage = new String[1];

        }

        Record record;

        private bool IsMatchingServiceRunning
        {
            get { return backgroundWorkerMatchingService.IsBusy; }
        }

        void startMatchingServiceProcess(NFRecord template)
        {
            if (backgroundWorkerMatchingService.IsBusy)
                return;

            backgroundWorkerMatchingService.RunWorkerAsync(template);
        }

        private void backgroundWorkerMatchingService_DoWork(object sender, DoWorkEventArgs e)
        {
            if (ConfigurationManager.AppSettings["matchingProvider"] == "managed")
            {
                var fingerList = new ArrayList();

                CheckBox cb;
                for (int i = 1; i < 11; i++)
                {
                    cb = this.Controls.Find("checkBox" + i.ToString(), true)[0] as CheckBox;
                    if (cb.Checked)
                        fingerList.Add(cb.Tag);
                }

                byte[] probeTemplate = (e.Argument as NFRecord).Save().ToArray();

                var matchingServiceClient = new PSCBioIdentification.CacheMatchingService.MatchingServiceClient();
                e.Result = matchingServiceClient.match(fingerList, probeTemplate);
            }
            else
            {
                record = new Record();
                //record.size = (UInt32)template.GetSize();
                //record.template = template.Save();
                record.size = (UInt32)(e.Argument as NFRecord).GetSize();
                record.template = (e.Argument as NFRecord).Save().ToArray();
                record.errorMessage = new System.Text.StringBuilder(512);

                var ar = new ArrayList();

                //record.fingerList = new string[3] { "ri", "rm", "rr" };
                //record.fingerListSize = 3;
                CheckBox cb;
                for (int i = 1; i < 11; i++)
                {
                    cb = this.Controls.Find("checkBox" + i.ToString(), true)[0] as CheckBox;
                    if (cb.Checked)
                        ar.Add(cb.Tag);
                }
                record.fingerListSize = ar.Count;
                //record.fingerList = new string[ar.Count];
                record.fingerList = ar.ToArray(typeof(string)) as string[];

                ar.Clear();

                //record.appSettings = new System.Text.StringBuilder(4);
                //ar.Add(MyConfigurationSettings.AppSettings["serverName"]);
                //ar.Add(MyConfigurationSettings.AppSettings["dbName"]);

                ar.Add(MyConfigurationSettings.ConnectionStrings["ODBCConnectionString"].ToString());
                ar.Add(MyConfigurationSettings.AppSettings["dbFingerTable"]);
                ar.Add(MyConfigurationSettings.AppSettings["dbIdColumn"]);
                ar.Add(MyConfigurationSettings.AppSettings["dbFingerColumn"]);
                record.appSettings = ar.ToArray(typeof(string)) as string[];

                //UInt32 score = 0;
                unsafe
                {
                    fixed (UInt32* ptr = &record.size)
                    {
                        if (ConfigurationManager.AppSettings["matchingService"] == "local")
                        {
                            e.Result = match(record.fingerList, record.fingerListSize, record.template, record.size, record.appSettings, record.errorMessage, record.errorMessage.Capacity);
                        }
                        else
                        {
                            CallbackFromCacheFillingService callback = new CallbackFromCacheFillingService();
                            callback.MyEvent += MyEvent;
                            InstanceContext context = new InstanceContext(callback);

                            var matchingServiceClient = new PSCBioIdentification.MatchingService.MatchingServiceClient(context);
                            e.Result = matchingServiceClient.match(record.fingerList, record.fingerListSize, record.template, record.size, record.appSettings, ref record.errorMessage, record.errorMessage.Capacity);
                        }
                    }
                }
            }
        }

        private void backgroundWorkerMatchingService_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Application.DoEvents();

            //MessageBox.Show(toolStripProgressBar.Value.ToString());

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

                string str = string.Format("Identification {0}", score == 0 ? "failed" : string.Format("succeeded. Score: {0}", score));

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

                    if (record != null && record.errorMessage.Length != 0)
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
