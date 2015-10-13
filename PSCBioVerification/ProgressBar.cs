using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace PSCBioVerification
{
    partial class Form1
    {
        private int _max = 0;

        private bool IsCapturing
        {
            get { return backgroundWorkerProgressBar.IsBusy; }
        }

        void startProgressBar()
        {
            if (IsCapturing)
                return;

            _max = toolStripProgressBar.Maximum;
            //toolStripProgressBar.Enabled = true;

            //toolStripStatusLabelError.Text = message;

            backgroundWorkerProgressBar.RunWorkerAsync();
        }

        void stopProgressBar()
        {
            if (IsCapturing)
            {
                backgroundWorkerProgressBar.CancelAsync();
            }
        }

        private void backgroundWorkerProgressBar_DoWork(object sender, DoWorkEventArgs e)
        {
            // This method will run on a thread other than the UI thread.
            // Be sure not to manipulate any Windows Forms controls created
            // on the UI thread from this method.
            backgroundWorkerProgressBar.ReportProgress(0, "Working...");
            while (true)
            {
                for (int i = 1; i < _max; i++)
                {
                    // Introduce some delay to simulate a more complicated calculation.
                    System.Threading.Thread.Sleep(100);
                    //backgroundWorkerProgressBar.ReportProgress((int)((float)(100 * i) / (float)_max), "Working...");
                    //(sender as BackgroundWorker).ReportProgress((int)((float)(100 * i) / (float)_max), "WorkingAAAAAAAA...");
                    //(sender as BackgroundWorker).ReportProgress((100 * i) / _max, "Working...");
                    (sender as BackgroundWorker).ReportProgress(i, "Working...");

                    if (backgroundWorkerProgressBar.CancellationPending)
                        break;
                }

                toolStripProgressBar.Value = 0;

                if (backgroundWorkerProgressBar.CancellationPending)
                    break;
            }

            //backgroundWorkerProgressBar.ReportProgress(100);
            //backgroundWorkerProgressBar.ReportProgress(100, "Complete!");
        }

        private void backgroundWorkerProgressBar_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //toolStripProgressBar.Increment(2);
            //toolStripProgressBar.PerformStep();

            toolStripProgressBar.Value = e.ProgressPercentage;
            if (toolStripStatusLabelError.Text == String.Empty)
                toolStripStatusLabelError.Text = e.UserState as String;

            //toolStripStatusLabelError.Text = toolStripProgressBar.Value.ToString();
        }

        private void backgroundWorkerProgressBar_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toolStripProgressBar.Value = 0;
            //toolStripProgressBar.Enabled = false;
        }
    }
}
