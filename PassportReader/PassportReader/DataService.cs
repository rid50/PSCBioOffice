using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Configuration;

namespace PassportReaderNS
{
    partial class Form1
    {
        int _id = 0;
        void startDataServiceProcess(int id)
        {
            _id = id;

            if (backgroundWorkerDataService.IsBusy)
                return;

            Dictionary<string, string> settings = new Dictionary<string, string>();
            if (System.Configuration.ConfigurationManager.AppSettings["Enroll"] == "db")
            {
                foreach (var key in ConfigurationManager.AppSettings.AllKeys)
                {
                    settings.Add(key, ConfigurationManager.AppSettings[key]);
                }

                foreach (ConnectionStringSettings cs in ConfigurationManager.ConnectionStrings)
                {
                    settings.Add(cs.Name, cs.ConnectionString);
                }
            }

            startProgressBar();
            backgroundWorkerDataService.RunWorkerAsync(settings);
        }

        void stopDataServiceProcess()
        {
            backgroundWorkerDataService.CancelAsync();
        }

        private void backgroundWorkerDataService_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((e.Argument as Dictionary<string, string>).Count != 0)
                {
                    var db = new DAO.Database(e.Argument as Dictionary<string, string>);
                    e.Result = db.GetImage(DAO.IMAGE_TYPE.wsq, _id);
                }
                else
                {
                    var db = new DBHelper.DBUtil();
                    e.Result = db.GetImageFromWebService(IMAGE_TYPE.wsq, _id);

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private void backgroundWorkerDataService_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                toolStripStatusLabelError.ForeColor = System.Drawing.Color.Red;
                toolStripStatusLabelError.Text = e.Error.Message;
            }
            else
            {
                if ((e.Result as byte[][])[0] != null)
                {
                    processReadFingers(e.Result as byte[][]);
                } else
                {
                    toolStripStatusLabelError.ForeColor = System.Drawing.Color.Red;
                    toolStripStatusLabelError.Text = "No records found";

                    System.Windows.Forms.PictureBox pb;
                    for (int i = 0; i <= 9; i++)
                    {
                        pb = this.Controls.Find("fpPictureBox" + (i + 1 < 9 ? (i + 1).ToString() : (i + 2).ToString()), true)[0] as System.Windows.Forms.PictureBox;
                        pb.Image = null;
                    }

                    buttonLeftHand.Enabled = true;
                    buttonRightHand.Enabled = true;
                    buttonThumbs.Enabled = true;

                    TextBoxID.Focus();
                }
            }
                
            stopProgressBar();
        }
    }
}
