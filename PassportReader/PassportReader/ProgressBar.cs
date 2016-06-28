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

            _max = toolStripProgressBar.Maximum;
            toolStripProgressBar.Enabled = true;

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
            backgroundWorkerProgressBar.ReportProgress(0);
            while (true)
            {
                //System.Threading.Thread.Sleep(500);
                for (int i = 0; i <= _max; i++)
                {
                    // Introduce some delay to simulate a more complicated calculation.
                    System.Threading.Thread.Sleep(50);
                    backgroundWorkerProgressBar.ReportProgress(i);
                    if (backgroundWorkerProgressBar.CancellationPending)
                        break;
                }
                if (backgroundWorkerProgressBar.CancellationPending)
                    break;
            }
        }

        private void backgroundWorkerProgressBar_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripProgressBar.Value = e.ProgressPercentage;
            if (toolStripStatusLabelError.Text == String.Empty)
                toolStripStatusLabelError.Text = e.UserState as String;
        }

        private void backgroundWorkerProgressBar_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toolStripProgressBar.Value = 0;
        }
    }
}
