using System;
using System.Configuration;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.ComponentModel;
using System.Xml;
using System.IO;
//using System.Windows.Forms;

using DataSourceServices;
using System.Windows.Forms;

namespace PSCBioIdentification
{
    partial class Form1
    {
        void startDataServiceProcess()
        {
            if (backgroundWorkerDataService.IsBusy)
                return;

            buttonRequest.Enabled = false;

            string param = String.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings["Enroll"] == "dblookup")
                param = "dblookup";
            else if (System.Configuration.ConfigurationManager.AppSettings["Enroll"] == "service")
                param = "service";
            else
                param = "db";

            startProgressBar();
            backgroundWorkerDataService.RunWorkerAsync(param);
        }

        private void backgroundWorkerDataService_DoWork(object sender, DoWorkEventArgs e)
        {
            // This method will run on a thread other than the UI thread.
            // Be sure not to manipulate any Windows Forms controls created
            // on the UI thread from this method.
            
            System.Threading.Thread.Sleep(1000);

            DataSource ds = null;

            try
            {
                if (e.Argument as string == "dblookup")
                    ds = new DataBaseService();
                else if (e.Argument as string == "service")
                    ds = new WebService();
                else
                    ds = new DataBaseService();

                int imageType = 0;
                if (mode == ProgramMode.PreEnrolled)
                    imageType = (int)DataSource.IMAGE_TYPE.wsq;
                else if (mode == ProgramMode.Verification || mode == ProgramMode.Identification)
                    imageType = (int)DataSource.IMAGE_TYPE.picture;

                //System.Threading.Thread.Sleep(40000);
                e.Result = ds.GetImage((DataSource.IMAGE_TYPE)imageType, this.userId);

                if (Mode == ProgramMode.PreEnrolled)
                {
                    processEnrolledData(e.Result as byte[]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void backgroundWorkerDataService_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Application.DoEvents();

            //MessageBox.Show(toolStripProgressBar.Value.ToString());
            //stopProgressBar();

            if (e.Error != null)
            {
                LogLine("Data service: " + e.Error.Message, true);
                ShowErrorMessage(e.Error.Message);
            }
            else
            {
                //if (Mode == ProgramMode.PreEnrolled)
                //{
                //    if (processEnrolledData(e.Result as byte[]))
                //    {
                //        if (radioButtonIdentify.Checked)
                //        {
                //            Mode = ProgramMode.Identification;
                //            startDataServiceProcess();          // go for a photo
                //        }
                //    }
                //}

                if (Mode == ProgramMode.PreEnrolled && radioButtonIdentify.Checked)
                {
                    Mode = ProgramMode.Identification;
                    startDataServiceProcess();          // go for a photo
                }
                else if (Mode != ProgramMode.PreEnrolled)
                {
                    using (var ms = new MemoryStream(e.Result as byte[]))
                    {
                        if (ms.Length != 0)
                            pictureBoxPhoto.Image = Image.FromStream(ms);
                        else
                            pictureBoxPhoto.Image = null;
                    }

//                    BeginInvoke(new MethodInvoker(delegate() { OnEnrollFromDataServiceCompleted(e.Result as byte[]); }));
                }
                //int i = 0;
            }

//            stopProgressBar();

            //System.Windows.Forms.Application.DoEvents();

            //buttonRequest.Enabled = true;
        }
    }
}
