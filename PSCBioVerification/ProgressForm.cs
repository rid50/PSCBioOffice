using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VeryFingerSDK
{
    public partial class ProgressForm : Form
    {
        private bool stopPressed = false;

        public void setTitle(string text)
        {
            Text = text;
        }

        public bool getStopPressed()
        {
            return stopPressed;
        }

        public void setProgress(int progress)
        {
            progressBar1.Value = progress;
        }

        public ProgressForm()
        {
            InitializeComponent();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            stopPressed = true;
        }
    }
}
