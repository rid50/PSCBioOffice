namespace PSCBioVerificationFace
{
    partial class FaceForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FaceForm));
            this.nfViewSplitContainer = new System.Windows.Forms.SplitContainer();
            this.nlView1 = new Neurotec.Biometrics.Gui.NLView();
            this.nlView2 = new Neurotec.Biometrics.Gui.NLView();
            this.gbMainLog = new System.Windows.Forms.GroupBox();
            this.lblWaitingForImg = new System.Windows.Forms.Label();
            this.rtbMain = new System.Windows.Forms.RichTextBox();
            this.personId = new System.Windows.Forms.TextBox();
            this.lblPersonId = new System.Windows.Forms.Label();
            this.gbResults = new System.Windows.Forms.GroupBox();
            this.chbLiveView = new System.Windows.Forms.CheckBox();
            this.btnCapture = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.buttonRequest = new System.Windows.Forms.Button();
            this.groupBoxMode = new System.Windows.Forms.GroupBox();
            this.radioButtonIdentify = new System.Windows.Forms.RadioButton();
            this.radioButtonVerify = new System.Windows.Forms.RadioButton();
            this.radioButtonEnroll = new System.Windows.Forms.RadioButton();
            this.cbFormats = new System.Windows.Forms.ComboBox();
            this.cbCameras = new System.Windows.Forms.ComboBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelError = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.zoomSlider1 = new PSCBioVerificationFace.Common.HorizontalZoomSlider();
            this.faceQualityThresholdSlider = new PSCBioVerificationFace.Common.HorizontalZoomSlider();
            this.zoomSlider2 = new PSCBioVerificationFace.Common.HorizontalZoomSlider();
            this.nfViewSplitContainer.Panel1.SuspendLayout();
            this.nfViewSplitContainer.Panel2.SuspendLayout();
            this.nfViewSplitContainer.SuspendLayout();
            this.gbMainLog.SuspendLayout();
            this.gbResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.groupBoxMode.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // nfViewSplitContainer
            // 
            this.nfViewSplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.nfViewSplitContainer, "nfViewSplitContainer");
            this.nfViewSplitContainer.Name = "nfViewSplitContainer";
            // 
            // nfViewSplitContainer.Panel1
            // 
            resources.ApplyResources(this.nfViewSplitContainer.Panel1, "nfViewSplitContainer.Panel1");
            this.nfViewSplitContainer.Panel1.Controls.Add(this.nlView1);
            // 
            // nfViewSplitContainer.Panel2
            // 
            resources.ApplyResources(this.nfViewSplitContainer.Panel2, "nfViewSplitContainer.Panel2");
            this.nfViewSplitContainer.Panel2.Controls.Add(this.nlView2);
            this.nfViewSplitContainer.TabStop = false;
            // 
            // nlView1
            // 
            resources.ApplyResources(this.nlView1, "nlView1");
            this.nlView1.BackColor = System.Drawing.SystemColors.Control;
            this.nlView1.DetectionDetails = null;
            this.nlView1.DrawConfidenceForEyes = true;
            this.nlView1.FaceIds = null;
            this.nlView1.Image = null;
            this.nlView1.Name = "nlView1";
            this.nlView1.ShowEyesConfidence = true;
            this.nlView1.ShowMouthConfidence = true;
            this.nlView1.ShowNoseConfidence = true;
            // 
            // nlView2
            // 
            resources.ApplyResources(this.nlView2, "nlView2");
            this.nlView2.BackColor = System.Drawing.SystemColors.Control;
            this.nlView2.DetectionDetails = null;
            this.nlView2.DrawConfidenceForEyes = true;
            this.nlView2.FaceIds = null;
            this.nlView2.Image = null;
            this.nlView2.Name = "nlView2";
            this.nlView2.ShowEyesConfidence = true;
            this.nlView2.ShowMouthConfidence = true;
            this.nlView2.ShowNoseConfidence = true;
            this.nlView2.Zoom = 0.8F;
            // 
            // gbMainLog
            // 
            resources.ApplyResources(this.gbMainLog, "gbMainLog");
            this.gbMainLog.Controls.Add(this.lblWaitingForImg);
            this.gbMainLog.Controls.Add(this.rtbMain);
            this.gbMainLog.Name = "gbMainLog";
            this.gbMainLog.TabStop = false;
            // 
            // lblWaitingForImg
            // 
            resources.ApplyResources(this.lblWaitingForImg, "lblWaitingForImg");
            this.lblWaitingForImg.Name = "lblWaitingForImg";
            // 
            // rtbMain
            // 
            resources.ApplyResources(this.rtbMain, "rtbMain");
            this.rtbMain.BackColor = System.Drawing.SystemColors.Window;
            this.rtbMain.Name = "rtbMain";
            this.rtbMain.ReadOnly = true;
            this.rtbMain.TabStop = false;
            // 
            // personId
            // 
            resources.ApplyResources(this.personId, "personId");
            this.personId.Name = "personId";
            // 
            // lblPersonId
            // 
            resources.ApplyResources(this.lblPersonId, "lblPersonId");
            this.lblPersonId.Name = "lblPersonId";
            // 
            // gbResults
            // 
            this.gbResults.Controls.Add(this.chbLiveView);
            this.gbResults.Controls.Add(this.btnCapture);
            this.gbResults.Controls.Add(this.gbMainLog);
            this.gbResults.Controls.Add(this.pictureBox2);
            this.gbResults.Controls.Add(this.buttonRequest);
            this.gbResults.Controls.Add(this.groupBoxMode);
            this.gbResults.Controls.Add(this.lblPersonId);
            this.gbResults.Controls.Add(this.personId);
            resources.ApplyResources(this.gbResults, "gbResults");
            this.gbResults.Name = "gbResults";
            this.gbResults.TabStop = false;
            // 
            // chbLiveView
            // 
            resources.ApplyResources(this.chbLiveView, "chbLiveView");
            this.chbLiveView.Name = "chbLiveView";
            this.chbLiveView.UseVisualStyleBackColor = true;
            // 
            // btnCapture
            // 
            resources.ApplyResources(this.btnCapture, "btnCapture");
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.UseVisualStyleBackColor = true;
            this.btnCapture.Click += new System.EventHandler(this.btnCapture_Click);
            // 
            // pictureBox2
            // 
            resources.ApplyResources(this.pictureBox2, "pictureBox2");
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.TabStop = false;
            // 
            // buttonRequest
            // 
            resources.ApplyResources(this.buttonRequest, "buttonRequest");
            this.buttonRequest.Name = "buttonRequest";
            this.buttonRequest.UseVisualStyleBackColor = true;
            this.buttonRequest.Click += new System.EventHandler(this.buttonRequest_Click);
            // 
            // groupBoxMode
            // 
            this.groupBoxMode.Controls.Add(this.radioButtonIdentify);
            this.groupBoxMode.Controls.Add(this.radioButtonVerify);
            this.groupBoxMode.Controls.Add(this.radioButtonEnroll);
            resources.ApplyResources(this.groupBoxMode, "groupBoxMode");
            this.groupBoxMode.Name = "groupBoxMode";
            this.groupBoxMode.TabStop = false;
            // 
            // radioButtonIdentify
            // 
            resources.ApplyResources(this.radioButtonIdentify, "radioButtonIdentify");
            this.radioButtonIdentify.Name = "radioButtonIdentify";
            this.radioButtonIdentify.TabStop = true;
            this.radioButtonIdentify.UseVisualStyleBackColor = true;
            this.radioButtonIdentify.CheckedChanged += new System.EventHandler(this.radioButtonGroup_CheckedChanged);
            // 
            // radioButtonVerify
            // 
            resources.ApplyResources(this.radioButtonVerify, "radioButtonVerify");
            this.radioButtonVerify.Name = "radioButtonVerify";
            this.radioButtonVerify.UseVisualStyleBackColor = true;
            this.radioButtonVerify.CheckedChanged += new System.EventHandler(this.radioButtonGroup_CheckedChanged);
            // 
            // radioButtonEnroll
            // 
            resources.ApplyResources(this.radioButtonEnroll, "radioButtonEnroll");
            this.radioButtonEnroll.Name = "radioButtonEnroll";
            this.radioButtonEnroll.UseVisualStyleBackColor = true;
            this.radioButtonEnroll.CheckedChanged += new System.EventHandler(this.radioButtonGroup_CheckedChanged);
            // 
            // cbFormats
            // 
            resources.ApplyResources(this.cbFormats, "cbFormats");
            this.cbFormats.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFormats.FormattingEnabled = true;
            this.cbFormats.Name = "cbFormats";
            this.cbFormats.SelectedIndexChanged += new System.EventHandler(this.cbFormats_SelectedIndexChanged);
            // 
            // cbCameras
            // 
            resources.ApplyResources(this.cbCameras, "cbCameras");
            this.cbCameras.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCameras.FormattingEnabled = true;
            this.cbCameras.Name = "cbCameras";
            this.cbCameras.SelectedIndexChanged += new System.EventHandler(this.cbCameras_SelectedIndexChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelError,
            this.toolStripProgressBar});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            // 
            // toolStripStatusLabelError
            // 
            this.toolStripStatusLabelError.Name = "toolStripStatusLabelError";
            resources.ApplyResources(this.toolStripStatusLabelError, "toolStripStatusLabelError");
            this.toolStripStatusLabelError.Spring = true;
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            resources.ApplyResources(this.toolStripProgressBar, "toolStripProgressBar");
            // 
            // pictureBox3
            // 
            resources.ApplyResources(this.pictureBox3, "pictureBox3");
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.TabStop = false;
            // 
            // zoomSlider1
            // 
            this.zoomSlider1.Label = "Zoom Image";
            resources.ApplyResources(this.zoomSlider1, "zoomSlider1");
            this.zoomSlider1.Name = "zoomSlider1";
            this.zoomSlider1.ZoomValue = 1F;
            this.zoomSlider1.ZoomValueChanged += new System.EventHandler<PSCBioVerificationFace.Common.ZoomEventArgs>(this.zoomSlider1_ZoomValueChanged);
            // 
            // faceQualityThresholdSlider
            // 
            this.faceQualityThresholdSlider.Label = "Face Quality Threshold";
            resources.ApplyResources(this.faceQualityThresholdSlider, "faceQualityThresholdSlider");
            this.faceQualityThresholdSlider.Name = "faceQualityThresholdSlider";
            this.faceQualityThresholdSlider.ZoomValue = 1F;
            this.faceQualityThresholdSlider.ZoomValueChanged += new System.EventHandler<PSCBioVerificationFace.Common.ZoomEventArgs>(this.faceQualityThresholdSlider_ZoomValueChanged);
            // 
            // zoomSlider2
            // 
            this.zoomSlider2.Label = "Zoom";
            resources.ApplyResources(this.zoomSlider2, "zoomSlider2");
            this.zoomSlider2.Name = "zoomSlider2";
            this.zoomSlider2.ZoomValue = 1F;
            this.zoomSlider2.ZoomValueChanged += new System.EventHandler<PSCBioVerificationFace.Common.ZoomEventArgs>(this.zoomSlider2_ZoomValueChanged);
            // 
            // FaceForm
            // 
            this.AcceptButton = this.buttonRequest;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkKhaki;
            this.Controls.Add(this.zoomSlider1);
            this.Controls.Add(this.faceQualityThresholdSlider);
            this.Controls.Add(this.zoomSlider2);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.gbResults);
            this.Controls.Add(this.cbFormats);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.nfViewSplitContainer);
            this.Controls.Add(this.cbCameras);
            this.Name = "FaceForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FaceForm_FormClosed);
            this.Load += new System.EventHandler(this.FaceForm_Load);
            this.nfViewSplitContainer.Panel1.ResumeLayout(false);
            this.nfViewSplitContainer.Panel2.ResumeLayout(false);
            this.nfViewSplitContainer.ResumeLayout(false);
            this.gbMainLog.ResumeLayout(false);
            this.gbMainLog.PerformLayout();
            this.gbResults.ResumeLayout(false);
            this.gbResults.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.groupBoxMode.ResumeLayout(false);
            this.groupBoxMode.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer nfViewSplitContainer;
        private Neurotec.Biometrics.Gui.NLView nlView1;
        private Neurotec.Biometrics.Gui.NLView nlView2;
//        private Neurotec.Biometrics.Standards.Gui.FCView nfView1;
//        private Neurotec.Biometrics.Standards.Gui.FCView nfView2;
        private System.Windows.Forms.GroupBox gbMainLog;
        private System.Windows.Forms.Label lblWaitingForImg;
        private System.Windows.Forms.RichTextBox rtbMain;
        private System.Windows.Forms.TextBox personId;
        private System.Windows.Forms.Label lblPersonId;
        private System.Windows.Forms.GroupBox gbResults;
        private System.Windows.Forms.GroupBox groupBoxMode;
        private System.Windows.Forms.RadioButton radioButtonIdentify;
        private System.Windows.Forms.RadioButton radioButtonVerify;
        private System.Windows.Forms.RadioButton radioButtonEnroll;
        private System.Windows.Forms.Button buttonRequest;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelError;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.ComboBox cbFormats;
        private System.Windows.Forms.ComboBox cbCameras;
        private Common.HorizontalZoomSlider zoomSlider2;
        private System.Windows.Forms.Button btnCapture;
        private System.Windows.Forms.CheckBox chbLiveView;
        private Common.HorizontalZoomSlider faceQualityThresholdSlider;
        private Common.HorizontalZoomSlider zoomSlider1;
    }
}

