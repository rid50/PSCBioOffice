using System;
using System.Configuration;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.ComponentModel;
using System.Xml;
using System.IO;
using Neurotec.Biometrics;
//using System.Windows.Forms;

//using DataSourceServices;

namespace PSCBioIdentification
{
    partial class Form1
    {
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
           
            //UInt32 score = 0;
            unsafe
            {
                fixed (UInt32* ptr = &record.size)
                {
                    e.Result = match(record.template, record.size, record.errorMessage, record.errorMessage.Capacity);
                }
            }
        }

        private void backgroundWorkerMatchingService_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Application.DoEvents();

            //MessageBox.Show(toolStripProgressBar.Value.ToString());

            if (e.Error != null)
            {
                LogLine(e.Error.Message, true);
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
                }
                else
                    pictureBox2.Image = Properties.Resources.redcross;

                if (score > 0)
                {
                    startProgressBar();
                    startDataServiceProcess();
                }
                else
                {
                    if (record.errorMessage.Length != 0)
                    {
                        //retcode = false;
                        System.Windows.Forms.MessageBox.Show(record.errorMessage.ToString());
                        //ShowErrorMessage(record.errorMessage.ToString());
                    }

                }
            }

            stopProgressBar();
            //buttonRequest.Enabled = true;
        }
    }
}
