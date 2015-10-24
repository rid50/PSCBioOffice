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
//using System.Windows.Forms;

//using DataSourceServices;

namespace PSCBioIdentification
{
    partial class Form1
    {
        class Record
        {
            public UInt32   size;
            public byte[]   template;
            public string[] arrOfFingers;
            public int      arrOfFingersSize;
            public System.Text.StringBuilder errorMessage;
            //public String errorMessage;
            //public String[] errorMessage = new String[1];

        }

        Record record;

        void startMatchingServiceProcess(NFRecord template)
        {
            if (backgroundWorkerMatchingService.IsBusy)
                return;

            //buttonRequest.Enabled = false;

            backgroundWorkerMatchingService.RunWorkerAsync(template);
        }

        private void backgroundWorkerMatchingService_DoWork(object sender, DoWorkEventArgs e)
        {
            // This method will run on a thread other than the UI thread.
            // Be sure not to manipulate any Windows Forms controls created
            // on the UI thread from this method.

            record = new Record();
            //record.size = (UInt32)template.GetSize();
            //record.template = template.Save();
            record.size = (UInt32)(e.Argument as NFRecord).GetSize();
            record.template = (e.Argument as NFRecord).Save();
            record.errorMessage = new System.Text.StringBuilder(512);

            var ar = new System.Collections.ArrayList();

            //record.arrOfFingers = new string[3] { "ri", "rm", "rr" };
            //record.arrOfFingersSize = 3;
            CheckBox cb;
            for (int i = 1; i < 11; i++)
            {
                cb = this.Controls.Find("checkBox" + i.ToString(), true)[0] as CheckBox;
                if (cb.Checked)
                    ar.Add(cb.Tag);
            }
            record.arrOfFingersSize = ar.Count;
            //record.arrOfFingers = new string[ar.Count];
            record.arrOfFingers = ar.ToArray(typeof(string)) as string[];

            //UInt32 score = 0;
            unsafe
            {
                fixed (UInt32* ptr = &record.size)
                {
                    e.Result = match(record.arrOfFingers, record.arrOfFingersSize, record.template, record.size, record.errorMessage, record.errorMessage.Capacity);
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
                    pictureBox2.Image = Properties.Resources.checkmark;
                    Mode = ProgramMode.PreEnrolled;
                    startDataServiceProcess();
                }
                else
                {
                    pictureBox2.Image = Properties.Resources.redcross;

                    stopProgressBar();

                    if (record.errorMessage.Length != 0)
                    {
                        //retcode = false;
                        ShowErrorMessage("ERROR!!!");
                        System.Windows.Forms.MessageBox.Show(record.errorMessage.ToString());
                    }
                    else
                    {
                        this.BeginInvoke(new MethodInvoker(delegate() { startCapturing(); }));
                    }
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
