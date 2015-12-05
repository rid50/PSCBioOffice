using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace PassportReaderNS
{
    partial class Form1
    {
        private int _max = 0;

        void startProgressBar()
        {
            if (backgroundWorkerProgressBar.IsBusy)
                return;

            toolStripStatusLabelError.ForeColor = System.Drawing.Color.Green;
            toolStripStatusLabelError.Text = string.Empty;

            _max = toolStripProgressBar1.Maximum;
            toolStripProgressBar1.Enabled = true;

            //toolStripStatusLabelError.Text = _max.ToString();

            backgroundWorkerProgressBar.RunWorkerAsync();
        }

        void stopProgressBar()
        {
            backgroundWorkerProgressBar.CancelAsync();
        }

        private void backgroundWorkerProgressBar_DoWork(object sender, DoWorkEventArgs e)
        {
            // This method will run on a thread other than the UI thread.
            // Be sure not to manipulate any Windows Forms controls created
            // on the UI thread from this method.
            backgroundWorkerProgressBar.ReportProgress(0, "Working...");
            while (true)
            {
                for (int i = 2; i < _max; ++i)
                {
                    // Introduce some delay to simulate a more complicated calculation.
                    System.Threading.Thread.Sleep(10);
                    backgroundWorkerProgressBar.ReportProgress((100 * i) / _max, "Working...");
                    if (backgroundWorkerProgressBar.CancellationPending)
                        break;
                }
                if (backgroundWorkerProgressBar.CancellationPending)
                    break;
            }

            backgroundWorkerProgressBar.ReportProgress(100, "Complete!");
        }

        private void backgroundWorkerProgressBar_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripProgressBar1.Value = e.ProgressPercentage;
            //toolStripStatusLabel.Text = e.UserState as String;
            if (toolStripStatusLabelError.ForeColor != System.Drawing.Color.Red)
            {
                toolStripStatusLabelError.ForeColor = System.Drawing.Color.Green;
                toolStripStatusLabelError.Text = e.UserState as String;
                //toolStripStatusLabelError.Text = e.ProgressPercentage.ToString();
            }
        }

        private void backgroundWorkerProgressBar_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toolStripProgressBar1.Value = 0;
            toolStripProgressBar1.Enabled = false;
        }
    }
}
